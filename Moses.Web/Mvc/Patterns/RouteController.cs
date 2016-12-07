using Moses.Web.Mvc.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Moses.Web.Mvc.Patterns
{
    
    public class RouteController : MosesController
    {
        public ActionResult App()
        {
            return View(nameof(App));
        }

        public ActionResult Reload()
        {
            return View(nameof(App));
        }

        public ActionResult Report()
        {
            return View();
        }

        // GET: Route
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

    }

    public class PublicRouteController : MosesController
    {
        public override MosesRequestType PageType
        {
            get
            {
                return MosesRequestType.SitePage; 
            }
        }

        public ActionResult App()
        {
            return View(nameof(App));
        }

    }
}