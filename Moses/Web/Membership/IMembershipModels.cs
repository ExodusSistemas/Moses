using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moses.Security;

namespace Moses.Membership
{

    public interface IMembership
    {
        Microsoft.AspNet.Identity.IUser User { get; set; }
        Microsoft.AspNet.Identity.IRole Role { get; set; }
        IFederatedContract Contract { get; set; }
        bool HasContract { get; }
        bool IsOwner { get; }
        string Email { get; }
        string FullName { get; }
        string RoleName { get; }
        Guid Id { get; }
        List<IFeature> Permissions { get; set; }
    }

    public interface IFederatedContract
    {
        int Id { get; set; }
        string Name { get; set; }
    }

    public interface IMembershipContext : IMembership
    {
        IEnumerable<IFeature> Features { get; set; }
        IFederatedContract FullContract { get; set; }
        IQueryable<IFederatedContract> Contracts { get; set; }
        HttpSessionStateBase Session { get; }
        HttpContextBase CurrentContext { get; }

        /// <summary>
        /// Defines the module name for multi module system access
        /// </summary>
        string ModuleName { get; set; }

        /// <summary>
        /// Defines if a feature is available for the current Membership Context : Ex. ("Contrato", "Read")
        /// </summary>
        /// <param name="featureSet">Name of the Controller Witch the feature belongs</param>
        /// <param name="name">Name of the feature. (Ex. "Modify")</param>
        /// <returns></returns>
        bool HasFeature(string featureSet, string name);

        bool IsConstrained();

        bool IsContaMovimentoConstrained();

    }
}