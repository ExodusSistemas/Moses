using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Moses.Web
{
    public interface IMembershipManager
    {
        
    }

    //public interface IMembershipContext : IMembershipManager
    //{
    //    MosesMembershipUser User { get; set; }
    //    Contract Contract { get; set; }
    //    bool HasContract { get; }
    //}

    /// <summary>
    /// Classe que existe apenas para marcar a interface IMembershipManager
    /// </summary>
    [Serializable]
    public class MembershipManager : IMembershipManager
    {
        [System.Xml.Serialization.XmlIgnore]
        public HttpContextBase CurrentContext { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public HttpSessionStateBase Session { get; set; }
    }
}
