using System;
using System.Drawing;
using System.IO;
using System.Net;

public static class WebHelper
{
    public static string GetHtml(string urlAddr)
    {
        if (urlAddr == null || string.IsNullOrEmpty(urlAddr))
        {
            throw new ArgumentNullException("urlAddr");
        }

        string result;

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

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        {
            result = sr.ReadToEnd();
            //Close and clean up the StreamReader
            sr.Close();
        }
        return result;
    }

    public static Image GetImage(string urlAddr)
    {
        if (urlAddr == null || string.IsNullOrEmpty(urlAddr))
        {
            throw new ArgumentNullException("urlAddr");
        }

        string result;

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

        return Image.FromStream(((HttpWebResponse)request.GetResponse()).GetResponseStream());
    }
}
