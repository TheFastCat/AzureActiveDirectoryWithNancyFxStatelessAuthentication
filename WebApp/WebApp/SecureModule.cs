using Nancy;
using Nancy.Responses;
using System;

namespace WebApp
{
    public class SecureModule : Nancy.NancyModule
    {
        public SecureModule()
        {
            Before += ctx =>
            {
                if (ctx.Request.Query.error.HasValue)
                {
                    string errorDesc = 
                        string.Format("{0}\n\n{1}\n\n{2}",
                        ctx.Request.Query.error,
                        ctx.Request.Query.error_description);

                    Context.Response            = Response.AsText(errorDesc);
                    Context.Response.StatusCode = HttpStatusCode.Forbidden;
                    return Context.Response;
                }

                return ctx.CurrentUser == null ||
                       String.IsNullOrWhiteSpace(ctx.CurrentUser.UserName)
                    ? new RedirectResponse("/login")
                    // else allow request to continue unabated
                    : null;
            };

            Get["/Private"] = _ =>
            {
                return "Secret stuff!";
            };
            Get["/Authenticated"] = _ =>
            {
                return "Hello " + Context.CurrentUser.UserName + "!";
            };
        }
    }
}