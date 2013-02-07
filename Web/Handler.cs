using System.IO;
using System.Web;

public class Handler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request["thumb"].IndexOf('.') == 0)
        {
            context.Response.StatusCode = 500;
            context.Response.End();
            return;
        }

        if (!File.Exists(context.Server.MapPath("~/Icons/" + context.Request["thumb"])))
        {
            //http://us.media.blizzard.com/wow/icons/18/<%#Eval("ReagentIcon")%>.jpg
            WebHelper.GetImage("http://us.media.blizzard.com/wow/icons/18/" + context.Request["thumb"]).Save(context.Server.MapPath("~/Icons/" + context.Request["thumb"]));

        }

        context.Response.ContentType = "image/jpeg";
        context.Response.WriteFile("~/Icons/" + context.Request["thumb"]);
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}