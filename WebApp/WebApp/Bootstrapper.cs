using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;

namespace WebApp
{
    /// <summary>
    /// See NancyFx's documentation http://goo.gl/HeXsp
    /// </summary>
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        /// <summary>
        /// For our use case this method exists for the purpose of enabling Nancy's Stateless authentication (http://goo.gl/Dtxhve)
        /// </summary>
        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            // At request startup we modify the request pipelines to
            // include stateless authentication
            //
            // Configuring stateless authentication is simple. Just use the 
            // NancyContext to get the apiKey. Then, use the authorization code to get 
            // your user's identity from Azure Active Directory via ADAL.
            //
            // If the authorization code required to do this is missing, NancyModules
            // secured via RequiresAuthentication() cannot be invoked...
            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                {
                    // the only way a user will be authenticated is if a request contains an authentication code
                    // attached to it...
                    if (!nancyContext.Request.Query.code.HasValue)
                    {
                        return null; // by returning null we essentially do not authenticate the incoming request
                    }

                    try
                    {
                        //for now, we will pull the apiKey from the querystring, 
                        //but you can pull it from any part of the NancyContext
                        var authorizationCode = (string)nancyContext.Request.Query.code;
                        return AADHelper.GetAuthenticatedUserIDentity(authorizationCode);//inject a user's identity (retrieved from AAD) into Nancy via StatelessAuthenticationConfiguration
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                });

            StatelessAuthentication.Enable(pipelines, configuration);
        }
    }
}
