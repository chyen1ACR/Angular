using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;
using AutoMapper;
using ChapterPortal.DAL;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace ChapterPortal
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
            services.AddCors();
            services.AddControllers();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddSingleton<IConfiguration>(Configuration);

            //set up session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(20);  //TimeSpan.FromSeconds(10)
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CustomeAutomapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //end of session setting 

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                       .AddCookie(o => o.LoginPath = new PathString("/Index"));
            services.AddMemoryCache();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddRazorPages();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".woff"] = "font-woff";
            provider.Mappings[".woff2"] = "font-woff2";

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts")),
                RequestPath = new PathString("/Resources"),
                ContentTypeProvider = provider
            });
            
            app.UseHttpsRedirection();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }


            app.UseSession();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            // Make sure you call this before calling app.UseMvc()
            app.UseCors(
                options => options.WithOrigins(Configuration["App:CorsOrigins"])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
            );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "Angular";
                
                /*
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
                */
               
            });



        }
    }
}
