using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace WeightTester
{
    class API
    {

        public string HttpResponse(string url, string body, string contentType, string method)
        {
            byte[] btBodys = Encoding.UTF8.GetBytes(body);

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            Stream reqStream = null;
            StreamReader streamReader = null;
            string responseContent = "";

            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = contentType;
                httpWebRequest.Accept = contentType;
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 5 * 1000;
                //httpWebRequest.ContentLength = btBodys.Length;

                if (method != "GET")
                {
                    reqStream = httpWebRequest.GetRequestStream();
                    reqStream.Write(btBodys, 0, btBodys.Length);
                }

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                responseContent = streamReader.ReadToEnd();

                return responseContent;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return responseContent;
            }
            finally
            {
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                if (reqStream != null) reqStream.Close();
                if (streamReader != null) streamReader.Close();
            }
        }
    }
}
