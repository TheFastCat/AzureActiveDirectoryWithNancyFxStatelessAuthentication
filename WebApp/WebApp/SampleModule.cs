
namespace WebApp
{
    /// <summary>
    /// Module containing a login route as a launching point for Azure Active Directory authentication prompting.
    /// </summary>
    /// <remarks>
    /// These are the only unsecured routes for the application; secure routes redirect here in order to authenticate unauthorized users
    /// </remarks>
    public class SampleModule : Nancy.NancyModule
    {
        public SampleModule()
        {
            Get["/"] = _ => "Hello World!";
            Get["/login"] = _ =>
            {
                // send a request to Azure AAD via oauth2 using a URL we create containing arguments.
                // AAD will in turn prompt the user (via web dialog) to authenticate themselves...
                // only after providing VALID CREDENTIALS for a user existing within the  AAD.TENANT_ID (aka 'domain'/'directory')
                // AAD will return an authorization code to REPLY_URL. This authorization code can then be used to retrieve 
                // a security token.
                // (see SecureModule.cs for reception of this authorization code and its use to retrieve an authentication token)
                return new Nancy.Responses.
                    RedirectResponse(AADHelper.GetAuthorizationURL());
            };
        }
    }
}
