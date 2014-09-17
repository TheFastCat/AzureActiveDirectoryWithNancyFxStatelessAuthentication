using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace WebApp
{
    using Nancy.Security;

    /// <summary>
    /// This static class is used to encapsulate Active Directory Authentication Library (ADAL) operations against Azure Active Directory (AAD) required for user authentication.
    /// </summary>
    public static class AADHelper
    {
        /// <summary>
        /// Return the URL to use when redirecting an incoming client to authenticate via Azure Active Directory
        /// </summary>
        /// <returns>a string URL representing an authentication endpoint for AAD</returns>
        public static string GetAuthorizationURL()
        {
            // compose the URL that will redirect the incoming client to be authenticated 
            // this url contains various arguments Azure Active Directory consumes via oauth2 
            // in order to determine a client is a person who has access to the Azure Active Directory
            // (and therefore applications configured within it)
            string authorizationUrl = string.Format("https://login.windows.net/{0}/"+
            "oauth2/authorize?api-version=1.0&response_type=code&client_id={1}&"+
            "resource={2}&redirect_uri={3}",
            AAD.TENANT_ID,
            AAD.CLIENT_ID,
            AAD.APP_ID_URI,
            AAD.REPLY_URL);

            return authorizationUrl;
        }

        /// <summary>
        /// Variables and configured settings within Azure Active Directory ('AAD') that are required by Azure Active Directory Authentication Library 
        /// ('ADAL'/ Microsoft.IdentityModel.Clients.ActiveDirectory http://goo.gl/EzRE6d) in order to consume authentication service from AAD from this 
        /// client application.
        /// </summary>
        /// <remarks>
        /// Grouped, documented here for simplicity
        /// </remarks>
        public struct AAD
        {
            /// <summary>
            /// an Azure Active Directory "tenant" (aka Domain) which is identified by us as a key (which can be found via) :
            ///
            /// Azure Portal -> 
            ///    Active Directory (pick the one containing the api/resource app and the client app) ->
            ///           Applications ->
            ///              View Endpoints (at the bottom, center of the screen)
            ///
            /// the TENANT_ID appears sandwiched within the various end point urls
            /// </summary>
            public static readonly string TENANT_ID
                = "[YOUR TENANT ID HERE]"; // eg f921887b-99eb-4505-b420-850208ac5dd7
            /// <summary>
            ///  this is the 'clientID' associated with THIS application (which in our case is a web app client, not a native client), 
            ///  as configured within Azure Active Directory, and associated as a client for the resource (we want to access).
            ///  AAD uses this to identify the client application.
            /// </summary>
            public static readonly string CLIENT_ID
                = "[YOUR CLIENT ID HERE]"; // eg "2e437f98-239b-4e31-a23b-c13d53f0d155
            /// <summary>
            /// 'APP ID URI' of resource we want to access, as configured within Azure Active Directory (and associated as a resource for THIS application (the client application))
            /// AAD uses this to identify the resource application.
            /// </summary>
            public static readonly string APP_ID_URI
                = "[YOUR API ID URI HERE]"; // eg Https://SalesApplication.onmicrosoft.com/WebApplication
            /// <summary>
            /// reply url to send the authorization code  :
            ///
            ///   (1) the URL needs to be : this app's host url. 
            ///   (2) the URL needs to be : configured within AAD to match our AAD client application's 'REPLY URL' 
            ///   (3) the URL *should be* : ssl
            ///   
            /// </summary>
            /// <remarks>
            /// As a best practice the the callback URI (aka redirect -- the URI Azure Active Directory will return its authentication token/"code" to)
            /// should be https, not http. This necessitates that our Nancy web client app (READ:this application) handle reception over https/SSL.
            /// Configuring Nancy for SSL is much easier to accomplish when hosted via IIS/SystemWeb. Self-hosted Nancy
            /// requires a lot of overhead to configure SSL that IIS handles implicitly. That wasn't done for this tutorial for the sake of simplification.
            /// </remarks>
            /// <seealso cref="http://goo.gl/VXRR4c"/>
            /// <seealso cref="http://goo.gl/4zDuD"/>
            /// <seealso cref="http://goo.gl/Psep0"/>
            /// <seealso cref="http://goo.gl/5t6fg8"/>
            public static readonly string REPLY_URL
                = "http://localhost:1234/Authenticated";
            /// <summary>
            /// This is the 'secret' configured within AAD to associate the calling code with the configured application
            /// </summary>
            /// <remarks>you will have to update this with one specific to your own AAD tenant/application</remarks>
            public static readonly string CLIENT_KEY
                = "[YOUR CLIENT KEY]"; // eg ZqWtcEqRx9SjQTvq6qDRRxnZV4+UdamzcULw00gwUH4=
        }

        /// <summary>
        /// Acquires an IUserIdentity from Azure Active Directory using the argument authorizationCode.
        /// </summary>
        /// <param name="authorizationCode">An authorization code provided by Azure Active Directory used to retrieve an IUserIdentity</param>
        /// <returns>Returns an IUserIdentity representing a successfully authenticated Azure Active Directory user who has privileges for this configured application</returns>
        public static IUserIdentity GetAuthenticatedUserIDentity(string authorizationCode)
        {
            var authenticationContext = new AuthenticationContext(string.Format("https://login.windows.net/{0}", AAD.TENANT_ID));
            var clientCredential      = new ClientCredential(AAD.CLIENT_ID, AAD.CLIENT_KEY);
            var authenticationResult  = authenticationContext.AcquireTokenByAuthorizationCode(authorizationCode, new Uri(AAD.REPLY_URL), clientCredential);
            return new UserIdentity(authenticationResult.UserInfo);
        }
    }
}