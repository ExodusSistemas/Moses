using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Web.Mvc.Patterns
{
    public interface IUser
    {
        Guid? Id { get; set; }

        string FullName { get; set; }

        string Name { get; set; }
    }
}
