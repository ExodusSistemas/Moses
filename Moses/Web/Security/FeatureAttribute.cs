using Moses;
using Moses.Extensions;
using Moses.Web;
using Moses.Web.Mvc;
using Moses.Web.Mvc.Patterns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moses.Security
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FeatureSetAttribute : AuthorizeAttribute
    {
        public FeatureSetAttribute()
        {
            Feature = new Feature();
        }

        private Feature Feature { get; set; }

        public string Name { get { return Feature.Name; } set { Feature.Name = value; } }
        public bool Public { get; set; }
        public bool IsLoginPage { get; set; }

        /// <summary>
        /// Determina o Nível do Grupo de Acordo com as definições em RoleGroupOptions.
        /// </summary>
        public RoleGroupOptions GroupLevel { get { return Feature.GroupLevel; } set { Feature.GroupLevel = value; } }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (Public)
              return true;

            if (httpContext.Session != null)
            {
                var availableFeatures = Feature.DeserializeList(httpContext.Session["MyFeatures"] as string); 

                foreach (var f in availableFeatures)
                {
                    if (httpContext.Request.Path.StartsWith( f.Path) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var msg = "A operação não foi autorizada. O perfil de acesso é insuficiente ou sua sessão expirou.";

                filterContext.Result = new JsonNetResult() { Data = ResponseViewModel.ApplyFail(msg), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

    }

    #region SystemRole Code Assist

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RoleActionBaseAttribute : Attribute
    {
        public RoleActionBaseAttribute(string featureKey, params RoleGroupOptions[] systemRoleGroups)
        {
            Roles = systemRoleGroups;
            FeatureKey = featureKey;
            FeatureMode = FeatureModeOptions.Allow;
        }

        public RoleActionBaseAttribute(string featureKey, FeatureModeOptions mode, params RoleGroupOptions[] systemRoleGroups)
        {
            Roles = systemRoleGroups;
            FeatureKey = featureKey;
            FeatureMode = mode;
        }

        public FeatureModeOptions FeatureMode { get; set; }
        public string FeatureKey { get; set; }
        public RoleGroupOptions[] Roles { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AllowAttribute : RoleActionBaseAttribute
    {
        public AllowAttribute(params RoleGroupOptions[] systemRoleGroups)
            : base("*", FeatureModeOptions.Allow, systemRoleGroups)
        {

        }

        public AllowAttribute(string featureKey, params RoleGroupOptions[] systemRoleGroups)
            : base(featureKey, FeatureModeOptions.Allow, systemRoleGroups)
        {
            
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RestrictAttribute : RoleActionBaseAttribute
    {
        public RestrictAttribute(string featureKey, FeatureModeOptions mode, params RoleGroupOptions[] systemRoleGroups)
            : base(featureKey, mode, systemRoleGroups)
        {
            
        }
            
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DenyAttribute : RoleActionBaseAttribute
    {
        public DenyAttribute(string featureKey, params RoleGroupOptions[] systemRoleGroups)
            : base(featureKey, FeatureModeOptions.Deny, systemRoleGroups)
        {
            
        }

    }

    #endregion


    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class FeatureAttribute : Attribute
    {

        public FeatureAttribute()
        {

        }

        public FeatureAttribute(string name) : this(name, null)
        {

        }

        public FeatureAttribute(string name, string filterExpression)
        {
            Name = name;
            FilterExpression = filterExpression;
        }

        public string FilterExpression { get; set; }

        /// <summary>
        /// Açucar Sintático
        /// </summary>
        /// <param name="description"></param>
        public FeatureAttribute(FeatureOptions featureDefaultDescription)
        {
            Name = GetAccessAttribute(featureDefaultDescription);
        }

        private static string GetAccessAttribute(FeatureOptions e)
        {
            var memInfo = typeof(FeatureOptions).GetMember(e.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                return ((DescriptionAttribute)attributes[0]).Description;

            return null;
        }

        public string Name { get; set; }

        /// <summary>
        /// Permite acesso de escrita ao criador do registro mesmo que a feature permita somente leitura em outras circunstâncias.
        /// </summary>
        public FeatureModeOptions Mode { get; set; }

        /// <summary>
        /// Determina o Nível do Grupo de Acordo com as definições em RoleGroupOptions.
        /// </summary>
        public RoleGroupOptions? GroupLevel { get; set; }
    }

    /// <summary>
    /// Define Frequent Fast-Use Features for functionality configuration
    /// </summary>
    public enum FeatureOptions
    {
        [Description("Ler")]
        Read,
        [Description("Modificar")]
        Modify,
        [Description("Executar")]
        Execute,
    }

    


}