using AuthorizationExample.Authorization;
using AuthorizationExample.Data;
using AuthorizationExample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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


			//ҳ����Ȩ
			services.AddMvc()
				.AddRazorPagesOptions(options =>
				{
					//options.Conventions.AuthorizeFolder("/Contacts");
					options.Conventions.AuthorizeFolder("/Account/Manage");
					options.Conventions.AuthorizePage("/Account/Logout");
				});

			//���ڲ��Խ�ɫ��Ȩ
			services.AddAuthorization(options =>
			{
				options.AddPolicy("ElevatedRights",policy=>policy.RequireRole(new string[] { Constants.ContactAdministratorsRole,Constants.ContactManagersRole}));
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
