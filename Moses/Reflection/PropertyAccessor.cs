﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Moses.Reflection
{

    /// <summary>
    /// The PropertyAccessor class provides fast dynamic access
    /// to a property of a specified target class.
    /// </summary>
    public class PropertyAccessor : IPropertyAccessor
    {
        /// <summary>
        /// Creates a new property accessor.
        /// </summary>
        /// <param name="targetType">Target object type.</param>
        /// <param name="property">Property name.</param>
        public PropertyAccessor(Type targetType, string property)
        {
            this.mTargetType = targetType;
            this.mProperty = property;
            PropertyInfo propertyInfo =
                targetType.GetProperty(property);

            //
            // Make sure the property exists
            //
            if (propertyInfo == null)
            {
                throw new
                  Exception(string.Format("Property \"{0}\" does" +
                  " not exist for type " + "{1}.", property, targetType));
            }
            else
            {
                this.mCanRead = propertyInfo.CanRead;
                this.mCanWrite = propertyInfo.CanWrite;
                this.mPropertyType = propertyInfo.PropertyType;
            }
        }

        /// <summary>
        /// Gets the property value from the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Property value.</returns>
        public object Get(object target)
        {
            if (mCanRead)
            {
                if (this.mEmittedPropertyAccessor == null)
                {
                    this.Init();
                }
                return this.mEmittedPropertyAccessor.Get(target);
            }
            else
            {
                throw new
                  Exception(string.Format("Property \"{0}\" does" +
                  " not have a get method.", mProperty));
            }
        }

        /// <summary>
        /// Gets the property value from the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Property value.</returns>
        public T Get<T>(object target) 
        {
            if (mCanRead)
            {
                if (this.mEmittedPropertyAccessor == null)
                {
                    this.Init();
                }
                object output = this.mEmittedPropertyAccessor.Get(target);
                if (output == null)
                {
                    

                    //se for tipo por valor, então retorna o valor inicial
                    if (this.PropertyType.IsValueType && !typeof(T).IsNullableType() )
                    {
                        return (T)Activator.CreateInstance(typeof(T));
                    }
                   
                }
                    
                return (T)output;
            }
            else
            {
                throw new
                  Exception(string.Format("Property \"{0}\" does" +
                  " not have a get method.", mProperty));
            }
        }

        /// <summary>
        /// Sets the property for the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="value">Value to set.</param>
        public void Set(object target, object value)
        {
            if (mCanWrite)
            {
                if (this.mEmittedPropertyAccessor == null)
                {
                    this.Init();
                }

                //
                // Set the property value
                //
                this.mEmittedPropertyAccessor.Set(target, value);
            }
            else
            {
                throw new
                  Exception(string.Format("Property \"{0}\" does" +
                  " not have a set method.", mProperty));
            }
        }

        /// <summary>
        /// Whether or not the Property supports read access.
        /// </summary>
        public bool CanRead
        {
            get
            {
                return this.mCanRead;
            }
        }

        /// <summary>
        /// Whether or not the Property supports write access.
        /// </summary>
        public bool CanWrite
        {
            get
            {
                return this.mCanWrite;
            }
        }

        /// <summary>
        /// The Type of object this property accessor was
        /// created for.
        /// </summary>
        public Type TargetType
        {
            get
            {
                return this.mTargetType;
            }
        }

        /// <summary>
        /// The Type of the Property being accessed.
        /// </summary>
        public Type PropertyType
        {
            get
            {
                return this.mPropertyType;
            }
        }

        private Type mTargetType;
        private string mProperty;
        private Type mPropertyType;
        private IPropertyAccessor mEmittedPropertyAccessor;
        private Hashtable mTypeHash;
        private bool mCanRead;
        private bool mCanWrite;

        /// <summary>
        /// This method generates creates a new assembly containing
        /// the Type that will provide dynamic access.
        /// </summary>
        private void Init()
        {
            this.InitTypes();
            // Create the assembly and an instance of the 

            // property accessor class.

            Assembly assembly = EmitAssembly();
            mEmittedPropertyAccessor =
              assembly.CreateInstance("Property") as IPropertyAccessor;
            if (mEmittedPropertyAccessor == null)
            {
                throw new Exception("Unable to create property accessor.");
            }
        }

        /// <summary>
        /// Thanks to Ben Ratzlaff for this snippet of code
        /// http://www.codeproject.com/cs/miscctrl/CustomPropGrid.asp
        /// 
        /// "Initialize a private hashtable with type-opCode pairs 
        /// so i dont have to write a long if/else statement when outputting msil"
        /// </summary>
        private void InitTypes()
        {
            mTypeHash = new Hashtable();
            mTypeHash[typeof(sbyte)] = OpCodes.Ldind_I1;
            mTypeHash[typeof(byte)] = OpCodes.Ldind_U1;
            mTypeHash[typeof(char)] = OpCodes.Ldind_U2;
            mTypeHash[typeof(short)] = OpCodes.Ldind_I2;
            mTypeHash[typeof(ushort)] = OpCodes.Ldind_U2;
            mTypeHash[typeof(int)] = OpCodes.Ldind_I4;
            mTypeHash[typeof(uint)] = OpCodes.Ldind_U4;
            mTypeHash[typeof(long)] = OpCodes.Ldind_I8;
            mTypeHash[typeof(ulong)] = OpCodes.Ldind_I8;
            mTypeHash[typeof(bool)] = OpCodes.Ldind_I1;
            mTypeHash[typeof(double)] = OpCodes.Ldind_R8;
            mTypeHash[typeof(float)] = OpCodes.Ldind_R4;
        }

        /// <summary>
        /// Create an assembly that will provide the get and set methods.
        /// </summary>
        private Assembly EmitAssembly()
        {
            //
            // Create an assembly name
            //
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "PropertyAccessorAssembly";
            //
            // Create a new assembly with one module
            //
            AssemblyBuilder newAssembly =
               Thread.GetDomain().DefineDynamicAssembly(assemblyName,
               AssemblyBuilderAccess.Run);
            ModuleBuilder newModule =
               newAssembly.DefineDynamicModule("Module");
            //
            // Define a public class named "Property" in the assembly.
            //
            TypeBuilder myType =
               newModule.DefineType("Property", TypeAttributes.Public);
            //

            // Mark the class as implementing IPropertyAccessor. 

            //

            myType.AddInterfaceImplementation(typeof(IPropertyAccessor));
            // Add a constructor

            ConstructorBuilder constructor =
               myType.DefineDefaultConstructor(MethodAttributes.Public);

            //
            // Define a method for the get operation. 
            //
            Type[] getParamTypes = new Type[] { typeof(object) };
            Type getReturnType = typeof(object);
            MethodBuilder getMethod =
              myType.DefineMethod("Get",
              MethodAttributes.Public | MethodAttributes.Virtual,
              getReturnType,
              getParamTypes);

            //
            // From the method, get an ILGenerator. This is used to
            // emit the IL that we want.
            //
            ILGenerator getIL = getMethod.GetILGenerator();

            //
            // Emit the IL. 
            //
            MethodInfo targetGetMethod = this.mTargetType.GetMethod("get_" +
                                                        this.mProperty);
            if (targetGetMethod != null)
            {
                getIL.DeclareLocal(typeof(object));
                getIL.Emit(OpCodes.Ldarg_1); //Load the first argument

                //(target object)

                //Cast to the source type

                getIL.Emit(OpCodes.Castclass, this.mTargetType);
                //Get the property value

                getIL.EmitCall(OpCodes.Call, targetGetMethod, null);
                if (targetGetMethod.ReturnType.IsValueType)
                {
                    getIL.Emit(OpCodes.Box, targetGetMethod.ReturnType);
                    //Box if necessary

                }
                getIL.Emit(OpCodes.Stloc_0); //Store it


                getIL.Emit(OpCodes.Ldloc_0);
            }
            else
            {
                getIL.ThrowException(typeof(MissingMethodException));
            }
            getIL.Emit(OpCodes.Ret);

            //
            // Define a method for the set operation.
            //
            Type[] setParamTypes = new Type[] { typeof(object), typeof(object) };
            Type setReturnType = null;
            MethodBuilder setMethod =
                myType.DefineMethod("Set",
               MethodAttributes.Public | MethodAttributes.Virtual,
               setReturnType,
               setParamTypes);
            //

            // From the method, get an ILGenerator. This is used to

            // emit the IL that we want.

            //

            ILGenerator setIL = setMethod.GetILGenerator();
            //

            // Emit the IL. 

            //

            MethodInfo targetSetMethod =
                this.mTargetType.GetMethod("set_" + this.mProperty);
            if (targetSetMethod != null)
            {
                Type paramType = targetSetMethod.GetParameters()[0].ParameterType;
                setIL.DeclareLocal(paramType);
                setIL.Emit(OpCodes.Ldarg_1); //Load the first argument 

                //(target object)

                //Cast to the source type

                setIL.Emit(OpCodes.Castclass, this.mTargetType);
                setIL.Emit(OpCodes.Ldarg_2); //Load the second argument 

                //(value object)

                if (paramType.IsValueType)
                {
                    setIL.Emit(OpCodes.Unbox, paramType); //Unbox it 

                    if (mTypeHash[paramType] != null) //and load
                    {
                        OpCode load = (OpCode)mTypeHash[paramType];
                        setIL.Emit(load);
                    }
                    else
                    {
                        setIL.Emit(OpCodes.Ldobj, paramType);
                    }
                }
                else
                {
                    setIL.Emit(OpCodes.Castclass, paramType); //Cast class

                }

                setIL.EmitCall(OpCodes.Callvirt,
                   targetSetMethod, null); //Set the property value

            }
            else
            {
                setIL.ThrowException(typeof(MissingMethodException));
            }
            setIL.Emit(OpCodes.Ret);
            //

            // Load the type

            //

            myType.CreateType();
            return newAssembly;
        }
    }
}
