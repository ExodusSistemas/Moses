using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moses.Web.Mvc.Patterns
{
    public class MosesPermissionContainer
    {
        private MembershipContext MembershipContext;

        public MosesPermissionContainer(MembershipContext MembershipContext)
        {
            // TODO: Complete member initialization
            this.MembershipContext = MembershipContext;
        }
        public virtual IMosesPermission GetPermissionSet()
        {
            throw new NotImplementedException();
        }

        public bool _updatePermissionsExecuted = false;

    }
}
