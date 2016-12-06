using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Moses.Web.Mvc;
using Moses.Web.Mvc.Patterns;

namespace Moses.Web
{

    public class HandleMosesAppException : FilterAttribute, IExceptionFilter
    {
        private string _genMessage;
        private AjaxExceptionChannelOptions _channel;
        private string _channelView;


        public HandleMosesAppException(string genericMessage = "Ocorreu um erro na sua requisição. ", AjaxExceptionChannelOptions channel = AjaxExceptionChannelOptions.Json, string channelView = "Error")
        {
            _genMessage = genericMessage;
            _channel = channel;
            _channelView = channelView;
        }

        public void OnException(ExceptionContext filterContext)
        {
            string error = _genMessage;
            bool expired = filterContext.HttpContext.Session["UserId"] == null;
            string isDebug = "true";
            if (filterContext.Exception is MosesRuntimeException)
            {
                error = filterContext.Exception.Message;
            }
            else
            {
#if DEBUG
                
                try{

                    MembershipContext exceptionContext = new MembershipContext(filterContext.HttpContext);

                    new MosesLogEntry()
                    {
                        ContractId = exceptionContext.Contract != null ? exceptionContext.Contract.Id : null,
                        EntryType = MosesLogEntryOptions.Erro,
                        Description = error,
                        UserId = exceptionContext.User != null ? exceptionContext.User.Id : null,
                    }.Log();


                    if (filterContext.Exception != null && !(filterContext.Exception.GetType() == Configuration.ApplicationExceptionType))
                        Moses.Web.ApplicationMail.CreateErrorMail(HttpContext.Current.Request.Url.ToString(), filterContext.Exception, exceptionContext.User.Name, exceptionContext.Contract.Name, HttpContext.Current.Request.UrlReferrer.AbsoluteUri).SendMail();
                }
                catch{
                }

                isDebug = "false";
            
#endif

                //if (filterContext.Exception is ChangeConflictException)
                //{
                //    StringBuilder builder = new StringBuilder();
                //    builder.AppendLine();
                //
                //    ChangeConflictException cce = filterContext.Exception as ChangeConflictException;
                //
                //    error += " (Optimistic concurrency error.)";
                //
                //    error += "[";
                //    foreach (var i in cce.Data)
                //    {
                //        error += i.ToString() + ",";
                //    }
                //    error += "]";
                //
                //}


                error += ": " + filterContext.Exception.Message;
            }

#if !DEBUG
            isDebug = "false";
#endif

            ViewDataDictionary dic = new ViewDataDictionary();
            dic.Add("Message", error);
            dic.Add("IsDebug", isDebug);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (_channel == AjaxExceptionChannelOptions.Json)
                {
                    filterContext.Result = new JsonNetResult() { Data = ResponseViewModel.ApplyFail(error, expired: expired), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    filterContext.Result = new PartialViewResult() { ViewName = _channelView, ViewData = dic };
                }
            }
            else
            {
                filterContext.Result = new ViewResult() { ViewName = "Error", ViewData = dic };
            }

            filterContext.ExceptionHandled = true;

        }

        public enum AjaxExceptionChannelOptions
        {
            Json,
            Html
        }
    }

}
