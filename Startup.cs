using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SpCoreMiner.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace SpCoreMiner
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
            services.AddControllersWithViews();

            // ثبت سرویس RfidService
            services.AddSingleton(new SpCoreMiner.Services.RfidService("/usr/bin/python3", "/var/www/html/Main/wwwroot/read-rfid.py", "/var/www/html/Main/wwwroot/write-rfid.py"));



            services.AddControllersWithViews();

            services.AddRouting(options => options.LowercaseUrls = true);
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddSingleton<LanguageService>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");




            //services.AddSingleton< RfidService>(sp => new RfidService());


            services.AddControllersWithViews();

			services.AddSingleton<LanguageService>();

			services.AddLocalization(options => options.ResourcesPath = "Resources");


			services.AddMvc()
				.AddViewLocalization()
				.AddDataAnnotationsLocalization(options =>
				{
					options.DataAnnotationLocalizerProvider = (type, factory) =>
					{

						var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);

						return factory.Create("SharedResource", assemblyName.Name);

					};

				});

			//Cross-origin policy to accept request from localhost:8084.
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					x => x.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader());
			});

			services.AddRazorPages();

			//services.AddRecaptcha(new RecaptchaOptions
			//{
			//    SiteKey = Configuration["Recaptcha:SiteKey"],
			//    SecretKey = Configuration["Recaptcha:SecretKey"]
			//});

			services.AddHttpContextAccessor();

			services.Configure<RequestLocalizationOptions>(
				options =>
				{
					var supportedCultures = new List<CultureInfo>
						{
							new CultureInfo("ar-AR"),
							new CultureInfo("en-US"),
							new CultureInfo("de-DE"),
							new CultureInfo("es-ES"),
							new CultureInfo("fa-IR"),
							new CultureInfo("fr-FR"), 
							new CultureInfo("it-IT"),
							new CultureInfo("tr-TR"),
						};



					options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

					options.SupportedCultures = supportedCultures;
					options.SupportedUICultures = supportedCultures;
					options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

				});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(locOptions.Value);

            app.UseFileServer();
            app.UseCors("CorsPolicy");



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
