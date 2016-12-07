using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Moses.Security
{
    public interface IFeatureInRole
    {
        IFeature Feature { get; set; }
        byte? IndOptions { get; set; }
    }

    /// <summary>
    /// Restraint options from the available features based on pre-defined aspects
    /// </summary>
    public enum FeatureModeOptions
    {
        Allow,
        Deny,
        RestrictToAuthor, //Restringe EXCLUSIVAMENTE ao autor do registro
        RestrictToOwner, //Restring ao Autor e a todos os Roles de Nível 0 , 1 e 2 (proprietários, administradores e gerentes)
        RestrictToConstraint, //Restring as entidades que possuem restrições para aquele usuário (Ex. Centro de Custo e Conta Contábil)
    }

    public class FeatureAccess
    {
        public IFeature Feature { get; set; }
        public int MyProperty { get; set; }
        public int ContractAccess { get; set; }
        public int UserAccess { get; set; }
    }

    public interface IFeature
    {
        string Name { get; set; }
        string Action { get; set; }
        string Controller { get; set; }
        string Path { get; set; }
        FeatureModeOptions Mode { get; set; }
        RoleGroupOptions GroupLevel { get; set; }
        byte? Options { get; set; }
    }

    
}