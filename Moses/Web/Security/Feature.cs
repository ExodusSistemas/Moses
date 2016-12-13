using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moses.Security
{
    public class Feature
    {
        public Feature()
        {
                
        }

        public Feature(IFeatureInRole ff) : this( ff.Feature, ff.IndOptions)
        {
            
        }

        public Feature(IFeature f, byte? options)
        {
            this.Name        = f.Name        ;
            this.Action      = f.Action      ;
            this.Controller  = f.Controller  ;
            this.Path        = f.Path        ;
            this.Options = options;
        }

        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Path { get; set; }
        public FeatureModeOptions Mode { get; set; }
        public RoleGroupOptions GroupLevel { get; set; }
        public byte? Options { get; set; }

        //Métodos que por padrão, se tiver dentro de uma classe com FeatureSet, mas que não esteja com Feature definido, que assume Read
        private static string[] _defaultReadMethods = { "List", "DataRequested", "Details", "AutoComplete" };

        //Métodos que por padrão, se tiver dentro de uma classe com FeatureSet, mas que não esteja com Feature definido, que assume Modify
        private static string[] _defaultModifyMethods = { "Add", "Edit", "Save", "Duplicate", "Delete", "MassEditDialog", "MassEdit" };

        public static HashSet<Feature> GetFeatures(System.Reflection.Assembly assembly)
        {
            HashSet<Feature> output = new HashSet<Feature>();
            foreach (Type featureSetType in assembly.GetTypes())
            {
                //Todos os Controllers(Somente)
                if (featureSetType.IsSubclassOf(typeof(System.Web.Mvc.Controller)))
                {
                    var featureSetTypeAttribute = featureSetType.GetCustomAttributes(typeof(FeatureSetAttribute), true);
                    //se o controller tiver FeatureAttribute
                    if (featureSetTypeAttribute.Length > 0)
                    {
                        var typedFeatureSetTypeAttribute = featureSetTypeAttribute[0] as FeatureSetAttribute;
                        //métodos internos
                        var featureSetMemberTypes = featureSetType.GetMethods();
                        foreach (var m in featureSetMemberTypes)
                        {
                            //Aplica Operação para cada Operação de Acesso Existente
                            ApplyFeature(ref output, featureSetType, m , typedFeatureSetTypeAttribute);
                        }
                    }
                }
            }

            return output;
        }

        public static string SerializeList(IEnumerable<Feature> features)
        {
            StringBuilder builder = new StringBuilder();
            
            foreach (var f in features)
            {
                builder.Append(f.Path);
                builder.Append(";");
            }

            var output = builder.Remove(builder.Length - 1, 1);

            return output.ToString();
        }

        public static IEnumerable<IFeature> DeserializeList(string features)
        {
            if (features == null)
            {
                return new HashSet<IFeature>();
            }

            var featureKeys = features.Split(';');
            HashSet<IFeature> output = new HashSet<IFeature>();

            try
            {
                foreach (var f in featureKeys)
                {
                    output.Add(Moses.Web.Configuration.ApplicationConfiguration.GetApplicationFeatures().Single(q => q.Path == f));
                }
            }
            catch
            {
                throw new MosesSecurityException("Inconsistência encontrada no mapeamento de funcionalidades carregado para o Contrato.", null);
            }

            return output;
        }

        private static void ApplyFeature(ref HashSet<Feature> output, Type featureSetType, System.Reflection.MethodInfo m, FeatureSetAttribute featureSet)
        {
            var featureAttributes = m.GetCustomAttributes(typeof(FeatureAttribute), false);
            if (featureAttributes.Length > 0)
            {
                var controller = featureSetType.Name.Substring(0, featureSetType.Name.LastIndexOf("Controller"));
                var fa = featureAttributes[0] as FeatureAttribute;

                var f = new Feature() {
                    Name = fa.Name,
                    Action = m.Name,
                    Controller = controller,
                    Path = "/" + controller + "/" + m.Name,
                    Mode = fa.Mode,
                    GroupLevel = fa.GroupLevel ?? featureSet.GroupLevel 
                };
                
                output.Add(f);
            }
            else
            {
                //Read 
                if (_defaultReadMethods.Contains(m.Name))
                {
                    var controller = featureSetType.Name.Substring(0, featureSetType.Name.LastIndexOf("Controller"));
                    var f = new Feature()
                    {
                        Name = "Ler",
                        Action = m.Name,
                        Controller = controller,
                        Path = "/" + controller + "/" + m.Name,
                        Mode = FeatureModeOptions.Allow,
                        GroupLevel = featureSet.GroupLevel
                    };

                    output.Add(f);
                }

                if (_defaultModifyMethods.Contains(m.Name))
                {
                    var controller = featureSetType.Name.Substring(0, featureSetType.Name.LastIndexOf("Controller"));
                    var f = new Feature()
                    {
                        Name = "Modificar",
                        Action = m.Name,
                        Controller = controller,
                        Path = "/" + controller + "/" + m.Name,
                        Mode = FeatureModeOptions.Allow,
                        GroupLevel = featureSet.GroupLevel
                    };

                    output.Add(f);
                }

            }
        }

        

    }
}
