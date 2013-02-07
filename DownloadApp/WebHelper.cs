using System;
using System.IO;
using System.Net;

namespace DownloadApp
{
    public static class WebHelper
    {
        public static string GetHtml(string urlAddr)
        {
            if (urlAddr == null || string.IsNullOrEmpty(urlAddr))
            {
                throw new ArgumentNullException("urlAddr");
            }

            string result = string.Empty;

            //1.Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddr);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.75 Safari/535.7";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            //request.AllowAutoRedirect = true;
            //request.MaximumAutomaticRedirections = 200;
            request.Proxy = null;
            request.UseDefaultCredentials = true;

            //2.Add the container with the active 
            CookieContainer cc = new CookieContainer();

            //3.Must assing a cookie container for the request to pull the cookies
            request.CookieContainer = cc;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    //Close and clean up the StreamReader
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }

            return result;
        }
    }
}