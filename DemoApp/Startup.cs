using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace DemoApp
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
            services.AddRazorPages(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var services = new ServiceCollection();
            string certif = "MyNewCertificate.pfx";
            X509Certificate2 certificate = new X509Certificate2(certif,"123");
            //  Path.Combine(env.ContentRootPath, "MyNewCertificate.pfx"), "123");


            //X509Certificate2 cert = null;
            //using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            //{
            //    certStore.Open(OpenFlags.ReadOnly);
            //    X509Certificate2Collection certCollection = certStore.Certificates.Find(
            //        X509FindType.FindByThumbprint,
            //        // Replace below with your cert's thumbprint
            //        "CB781679561914B7539BE120EE9C4F6780579A86",
            //        false);
            //    // Get the first cert with the thumbprint
            //    if (certCollection.Count > 0)
            //    {
            //        cert = certCollection[0];

            //    }
            //}

            //// Fallback to local file for development
            //if (cert == null)
            //{
            //    cert = new X509Certificate2(Path.Combine(env.ContentRootPath, "example.pfx"), "exportpassword");

            //}

            var identityServer = services.AddIdentityServer().AddInMemoryPersistedGrants();

            if (env.IsDevelopment())
            {
                identityServer.AddDeveloperSigningCredential();
            }
            else
            {
                identityServer.AddSigningCredential(certificate);
            }
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

           // app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
