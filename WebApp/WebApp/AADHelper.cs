using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace WebApp
{
    using Nancy.Security;

    public static class AADHelper
    {
        public static string GetAuthorizationURL()
        {
            string authorizationUrl = string.Format("https://login.windows.net/{0}/"+
            "oauth2/authorize?api-version=1.0&response_type=code&client_id={1}&"+
            "resource={2}&redirect_uri={3}",
            AAD.TENANT_ID,
            AAD.CLIENT_ID,
            AAD.APP_ID_URI,
            AAD.REPLY_URL);

            return authorizationUrl;
        }

        public struct AAD
        {
            public static readonly string TENANT_ID  
                = "f721817b-99eb-4505-b220-850208ab5dd7";
            public static readonly string CLIENT_ID
                = "2e427f98-139b-4e41-a23b-c13f53f0d155";
            public static readonly string APP_ID_URI 
                = "https://SalesApplication.onmicrosoft.com/WebAppResource";
            public static readonly string REPLY_URL
                = "http://localhost:1234/Authenticated";
            public static readonly string CLIENT_KEY
                = "ZOWtcEqRx9SjQTvq6qDrRnnZV4+UdaxzbULw00gwUH4=";
        }

        public static IUserIdentity GetAuthenticatedUserIDentity(string authorizationCode)
        {
            var authenticationContext = new AuthenticationContext(string.Format("https://login.windows.net/{0}", AAD.TENANT_ID));
            var clientCredential      = new ClientCredential(AAD.CLIENT_ID, AAD.CLIENT_KEY);
            var authenticationResult  = authenticationContext.AcquireTokenByAuthorizationCode(authorizationCode, new Uri(AAD.REPLY_URL), clientCredential);
            return new UserIdentity(authenticationResult.UserInfo);
        }
    }
}