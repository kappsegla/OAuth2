using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebAPIWithIdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()   //https://offering.solutions/blog/articles/2017/02/07/difference-between-addmvc-addmvcore/
            .AddAuthorization()
            .AddJsonFormatters();

            //AddIdentityServer registers the IdentityServer services in DI. It also registers an in-memory store for runtime state. This is useful for development scenarios. For production scenarios you need a persistent or shared store like a database or cache for that. See the EntityFramework quickstart for more information.
            //The AddDeveloperSigningCredential extension creates temporary key material for signing tokens. Again this might be useful to get started, but needs to be replaced by some persistent key material for production scenarios.
            services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryApiResources(Config.GetApiResources())
            //.AddClientStore<MyClientStore>()
            .AddInMemoryClients(Config.GetClients());
            //.AddTestUsers(Config.GetUsers());




            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseMvc();

            app.UseIdentityServer();  //In Configure the middleware is added to the HTTP pipeline.
        }
    }
}
