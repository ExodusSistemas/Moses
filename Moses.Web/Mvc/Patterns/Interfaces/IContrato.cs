using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Web.Mvc.Patterns
{
    public interface IContract
    {
        int? Id { get; set; }

        string Name { get; set; }
    }
}
