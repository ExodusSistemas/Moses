
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Web.Extensions
{
    
    public class ExcludeEntityKeyContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            return properties.FilterEntities().Where(p => !p.PropertyType.IsGenericType || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() != typeof(Moses.Data.EntitySet<>))).ToList();
            
        }

        
    }

    public static class ExcludeEntityKeyHelper{
        
        /// <summary>
        /// Exclui da lista todas as variáveis que representam entidades associadas
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IList<JsonProperty> FilterEntities(this IList<JsonProperty> properties)
        {
            List<JsonProperty> output = new List<JsonProperty>(properties);
            foreach (var property in properties)
            {
                var memberInfos = property.DeclaringType.GetMember(property.PropertyName);
                if (memberInfos.Length == 1)
                {
                    //AssociationAttribute attr = memberInfos[0].GetCustomAttributes(typeof(AssociationAttribute), false).FirstOrDefault() as AssociationAttribute;
                    //
                    //if (attr != null)
                    //    if (attr.IsForeignKey == true)
                    //        output.Remove(property);
                }
                
            }

            return output;
        }

    }
}

