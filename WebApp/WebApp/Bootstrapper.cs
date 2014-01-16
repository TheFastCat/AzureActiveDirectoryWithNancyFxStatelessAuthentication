using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;

namespace WebApp
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                {
                    if (!nancyContext.Request.Query.code.HasValue)
                    {
                        return null;
                    }

                    try
                    {
                        var authorizationCode = (string)nancyContext.Request.Query.code;
                        return AADHelper.GetAuthenticatedUserIDentity(authorizationCode);
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
