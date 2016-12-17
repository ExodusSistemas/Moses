using System.Web.Mvc;
using Moses.Extensions;

namespace Moses.Web.Mvc.Controls
{
    public static class HtmlHelperExtensions
    {
        public static MosesNamespace Moses(this HtmlHelper helper) => 
            new MosesNamespace();
    }
}

