
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

namespace Moses.Web
{
    public class PostData
    {

        private List<PostDataParam> m_Params;

        public List<PostDataParam> Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }

        public PostData()
        {
            m_Params = new List<PostDataParam>();

            // Add sample param
            m_Params.Add(new PostDataParam("email", "MyEmail", PostDataParamType.Field));
        }


        /// <summary>
        /// Returns the parameters array formatted for multi-part/form data
        /// </summary>
        /// <returns></returns>
        public string GetPostData()
        {
            // Get boundary, default is --AaB03x
            //string boundary = ConfigurationManager.AppSettings["ContentBoundary"].ToString();

            StringBuilder sb = new StringBuilder();
            //foreach (PostDataParam p in m_Params)
            //{
            //    sb.AppendLine(boundary);

            //    if (p.Type == PostDataParamType.File)
            //    {
            //        sb.AppendLine(string.Format("Content-Disposition: file; name=\"{0}\"; filename=\"{1}\"", p.Name, p.FileName));
            //        sb.AppendLine("Content-Type: text/plain");
            //        sb.AppendLine();
            //        sb.AppendLine(p.Value);
            //    }
            //    else
            //    {
            //        sb.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", p.Name));
            //        sb.AppendLine();
            //        sb.AppendLine(p.Value);
            //    }
            //}

            //sb.AppendLine(boundary);

            return sb.ToString();
        }
    }

    public enum PostDataParamType
    {
        Field,
        File
    }

    public class PostDataParam
    {

        public PostDataParam(string name, string value, PostDataParamType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public string Name;
        public string FileName;
        public string Value;
        public PostDataParamType Type;
    }

    public class ConvertApi
    {

        //private string _apikey;
        //private string _url;

        //static string HttpPost(string url, string[] paramName, string[] paramVal)
        //{
        //  HttpWebRequest req = WebRequest.Create(new Uri(url)) 
        //                       as HttpWebRequest;
        //  req.ContentType = "multipart/form-data";
        //  req.Method = "POST";

        //  PostData pData = new PostData();

        //  byte[] buffer = encoding.GetBytes(pData.GetPostData());

        //  // Set content length of our data
        //  req.ContentLength = buffer.Length;

        //  // Dump our buffered postdata to the stream, booyah
        //  oStream = req.GetRequestStream();
        //  oStream.Write(buffer, 0, buffer.Length);
        //  oStream.Close();

        //  // get the response
        //  oResponse = (HttpWebResponse)req.GetResponse();


        //  // Build a string with all the params, properly encoded.
        //  // We assume that the arrays paramName and paramVal are
        //  // of equal length:
        //  StringBuilder paramz = new StringBuilder();
        //  for (int i = 0; i < paramName.Length; i++) {
        //    paramz.Append(paramName[i]);
        //    paramz.Append("=");
        //    paramz.Append(HttpUtility.UrlEncode(paramVal[i]));
        //    paramz.Append("&");
        //  }

        //  // Encode the parameters as form data:
        //  byte[] formData =
        //      UTF8Encoding.UTF8.GetBytes(paramz.ToString());
        //  req.ContentLength = formData.Length;

        //  // Send the request:
        //  using (Stream post = req.GetRequestStream())  
        //  {  
        //    post.Write(formData, 0, formData.Length);  
        //  }

        //  // Pick up the response:
        //  string result = null;
        //  using (HttpWebResponse resp = req.GetResponse()
        //                                as HttpWebResponse)  
        //  {  
        //    StreamReader reader = 
        //        new StreamReader(resp.GetResponseStream());
        //    result = reader.ReadToEnd();
        //  }

        //  return result;
        //}


        /// <param name="apikey">The API key which can be found here: https://cloudconvert.org/user/profile</param>
        //public ConvertApi(string apikey = "442742913") //key padrão da conta ExodusSistemas
        //{
        //    _apikey = apikey;
        //    _url = "http://do.convertapi.com/Word2Pdf";
        //}

    }

}