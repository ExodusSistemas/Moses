using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Reflection
{
    public static class ReflectionHelper
    {
        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType && type.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }
    }
}
