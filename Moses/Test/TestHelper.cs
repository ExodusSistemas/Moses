using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace Moses.Test
{
    /// <summary>
    /// Helper de Métodos de Testes
    /// </summary>
    /// <seealso cref="http://manfred-ramoser.blogspot.com/"/>
    public static class TestHelper
    {
        public static object FillAttributesWithRandomValues(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo info in infos)
            {
                Type infoType = info.PropertyType;

                //poderia pegar pelos atributos customizados a partir daqui para que se delimite melhor os limites
                
                if (infoType.Equals(typeof(DateTime)))
                    info.SetValue(obj, RandomUtils.RandomDateTime(DateTime.Now, new DateTime(2014, 01, 01)), null);
                else if (infoType.Equals(typeof(String)) && info.GetValue(obj,null) == null )
                    info.SetValue(obj, info.Name + " " + RandomUtils.RandomString(50, null), null);
                else if ((infoType.Equals(typeof(long)) || infoType.Equals(typeof(Int32)) || infoType.Equals(typeof(Int64)) || infoType.Equals(typeof(int))) && !info.Name.ToLower().EndsWith("id") && !info.Name.ToLower().Equals("pk"))
                    info.SetValue(obj, RandomUtils.RandomNumber(0, 999999), null);
                else if (infoType.Equals(typeof(bool)))
                    info.SetValue(obj, RandomUtils.RandomBool(), null);
            }
            return obj;
        }

        
        /// <summary>
        /// Compara as pripriedades dos objetos
        /// </summary>
        /// <param name="firstObject"></param>
        /// <param name="secondObject"></param>
        /// <returns></returns>
        public static bool CompareObjectAttributes(object firstObject, object secondObject)
        {
            Type t1 = firstObject.GetType();
            Type t2 = secondObject.GetType();
            /*the two objects must have the same type*/
            if (!t1.Equals(t2)) return false;

            PropertyInfo[] infos1 =
                t1.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            PropertyInfo[] infos2 =
                t2.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            for (int i = 0; i < infos1.Length; i++)
            {
                /*this if is needed because if it is a datetime it should compare only the date and the time and not the ticks*/
                if (infos1[i].PropertyType.Equals(typeof(DateTime)))
                {
                    DateTime firstDate = (DateTime)infos1[i].GetValue(firstObject, null);
                    DateTime secondDate = (DateTime)infos2[i].GetValue(secondObject, null);
                    /*if the datatype in the database was date then only compare the date part of the datetime object*/
                    if (firstDate.ToString().Contains("00:00:00") || secondDate.ToString().Contains("00:00:00"))
                    {
                        if (!firstDate.Date.ToString().Equals(secondDate.Date.ToString())) return false;
                    }
                    /*otherwise compare the string representation of the two datetime objects because the ticks may differ*/
                    else
                    {
                        if (!firstDate.ToString().Equals(secondDate.ToString()))
                            return false;
                    }
                }
                else
                {
                    /*if one property value differs return false*/
                    if (!(infos1[i].GetValue(firstObject, null)).Equals(infos2[i].GetValue(secondObject, null)))
                        return false;
                }
            }

            /*when everything went fine the objects have the same values for their properties*/
            return true;
        }


        
    }
}
