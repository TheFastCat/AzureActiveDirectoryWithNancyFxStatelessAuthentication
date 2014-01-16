using Nancy;
using Nancy.Responses;
using System;

namespace WebApp
{
    /// <summary>
    /// Routes defined here require an authenticated user in order to access ; the module will redirect
    /// unauthenticated users to the login route.
    /// </summary>
    public class SecureModule : Nancy.NancyModule
    {
        public SecureModule()
        {
            // this hook will redirect all matched routes in the module to the /login route if 
            // the user hasn't been authenticated yet. Removing this hook will not redirect the users
            // and they will just receive a 402 Unauthorized StatusCode (and a blank browser).
            Before += ctx =>
            {
                // in the case that AAD returns an error we should display it
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
            // route useful for demonstrating secured routes redirect unauthorized users to /login => (ane then back to) => "/"
            Get["/Private"] = _ =>
            {
                return "Secret stuff!";
            };
            // this is the RETURN URL configured within AAD that gets invoked after successfully authenticating a user
            // Context.CurrentUser is assigned within Bootstrapper.cs by using the authorization code AAD returned to us to
            // retrieve user information (from AAD) and then injecting it into Nancy via Nancy.Authentication.Stateless
            Get["/Authenticated"] = _ =>
            {
                return "Hello " + Context.CurrentUser.UserName + "!";
            };
        }
    }
}