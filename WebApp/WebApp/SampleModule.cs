
namespace WebApp
{
    public class SampleModule : Nancy.NancyModule
    {
        public SampleModule()
        {
            Get["/"] = _ => "Hello World!";
            Get["/login"] = _ =>
            {
                return new Nancy.Responses.
                    RedirectResponse(AADHelper.GetAuthorizationURL());
            };
        }
    }
}
