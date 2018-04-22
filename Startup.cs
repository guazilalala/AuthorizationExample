using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthorizationExample.Data;
using AuthorizationExample.Services;
using Microsoft.AspNetCore.Authorization;
using AuthorizationExample.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AuthorizationExample
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

			services.AddMvc()
				.AddRazorPagesOptions(options =>
				{
					options.Conventions.AuthorizeFolder("/Account/Manage");
					options.Conventions.AuthorizePage("/Account/Logout");
				});

			// Register no-op EmailSender used by account confirmation and password reset during development
			// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
			services.AddSingleton<IEmailSender, EmailSender>();

			//services.AddMvc(config=> 
			//{
			//	var policy = new AuthorizationPolicyBuilder()
			//					.RequireAuthenticatedUser()
			//					.Build();
			//	config.Filters.Add(new AuthorizeFilter(policy));
			//});

			// Authorization handlers
			services.AddScoped<IAuthorizationHandler, ContactIsOwnerAuthorizationHandler>();
			services.AddScoped<IAuthorizationHandler, ContactAdministratorsAuthorizationHandler>();
			services.AddScoped<IAuthorizationHandler, ContactManagerAuthorizationHandler>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
   
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
