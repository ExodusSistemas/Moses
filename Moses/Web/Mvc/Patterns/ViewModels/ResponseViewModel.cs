using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moses.Web.Mvc.Patterns
{
    public class ResponseViewModel 
    {
        public bool success { get; set; }
        public string message { get; set; }
        public dynamic Misc { get; set; }
        public dynamic index { get; set; }
        public string referer { get; set; }

        public dynamic target { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MosesOperation operation { get; set; }

        public bool expired { get; set; }

        public ResponseViewModel()
        {
            referer = GetCurrentReferer();
        }

        public virtual string GetCurrentReferer()
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            return null;
        }

        public static JsonResult SaveResult(string message, dynamic model = null, object misc = null, object index = null, MosesOperation? operation = null)
        {
            object item = null;
            if( model is MosesBaseViewModel)
            {
                if (model == null)
                    throw new Moses.MosesConfigurationException("Erro no sistema: A configuração de Operação não foi definida.");

                operation = operation ?? model.Operation;
                try
                {
                    item = model.Item;
                }
                catch (RuntimeBinderException)
                {
                    item = new object();
                }
            }
            else
            {
                item = model;
            }

            return new JsonNetResult()
            {
                Data = new ResponseViewModel()
                {
                    success = true,
                    message = message,
                    Misc = misc,
                    index = index,
                    operation = operation ?? MosesOperation.Reload,
                    target = item
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static ResponseViewModel MultiEditResult(string message, dynamic model = null,  object misc = null, object index = null, MosesOperation? operation = null)
        {
            object item = null;
            if (model is MosesBaseViewModel)
            {
                operation = model.Operation ?? ((bool)model.IsEdit) ? MosesOperation.Edit : MosesOperation.Add;
                item = model.Item;
            }
            else
            {
                item = model;
            }

            return new ResponseViewModel()
            {
                success = true,
                message = message,
                Misc = misc,
                index = index,
                operation = operation ?? MosesOperation.MultiEdit,
                target = item
            };
        }

        public static JsonResult DeleteResult(string message, dynamic model = null, object misc = null, object index = null, MosesOperation? operation = MosesOperation.Delete)
        {

            return new JsonNetResult()
            {
                Data = new ResponseViewModel()
                {
                    success = true,
                    message = message,
                    Misc = misc,
                    index = index,
                    operation = operation.Value,
                    target = model
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        

        public static ResponseViewModel ApplySuccess(string successMessage = "Operação realizada com sucesso.", string defaultSuccessBehavior = null, object misc = null, object index = null, bool expired = false)
        {
            return new ResponseViewModel()
            {
                success = true,
                message = successMessage,
                Misc = misc,
                index = index,
                expired = expired
            };
        }
        

        public static ResponseViewModel ApplyFail(string errorMessage = "Ocorreu um erro desconhecido.", string defaultFailBehavior = null, object misc = null, object index = null, bool expired = false, Exception exception = null)
        {
            return new ResponseViewModel()
            {
                success = false,
                message = errorMessage,
                index = index ,
                expired = expired
            };
        }

        public static ResponseViewModel ApplyGenericFail()
        {
            return ApplyFail();
        }


    }
}