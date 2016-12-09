using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace NancyDemo1
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {     

        //need this to enable CORS so it's a standalone server
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {

            //CORS Enable
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");

            });
        }
    }
}