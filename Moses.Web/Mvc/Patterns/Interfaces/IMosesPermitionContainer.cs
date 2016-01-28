using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moses.Web.Mvc.Patterns
{
    public interface IMosesPermissionContainer
    {
        MembershipContext MembershipContext { get; set; }
    }
}
