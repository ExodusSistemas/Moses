using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;

namespace Moses.Web.Mvc
{
    public class DateTimePtBrModelBinder : IModelBinder
    {

        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var dtContainter = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (dtContainter == null)
                return null;

            var date = dtContainter.AttemptedValue;
          
            if (String.IsNullOrEmpty(date))
                return null;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue(bindingContext.ModelName));

            try
            {
                return DateTime.Parse(date);
            }

            catch (Exception)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, String.Format("\"{0}\" is invalid.", bindingContext.ModelName));
                return null;
            }

        }

        #endregion
    }
}
