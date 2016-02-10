using Moses.Web.Mvc.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Web
{
    public class MembershipRole{

    }

    public partial class MembershipRoleManager
    {
        private MembershipContext MembershipContext;

        public MembershipRoleManager(MembershipContext MembershipContext)
        {
            // TODO: Complete member initialization
            this.MembershipContext = MembershipContext;
        }

        public List<MembershipRole> GetAvailableRoles()
        {
            throw new NotImplementedException();
        }
    }

    
}
