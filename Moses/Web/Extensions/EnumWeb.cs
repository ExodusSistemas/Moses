using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Moses.Extensions
{
    public static class EnumWeb
    {
        public static IEnumerable<SelectListItem> GetSelectList(this Type enumType, int? selectedValue = null) {
            return enumType.GetEnumSelector().Select(q => new SelectListItem() { Value = q.Id.ToString(), Text = q.value, Selected = selectedValue == q.Id } );
        }
    }
}
