using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Http;

namespace BucketListApplication
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
			services.AddDbContext<BLContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("AzureSQLserver")));

			services.AddDefaultIdentity<BLUser>(options =>
			{
				options.SignIn.RequireConfirmedEmail = false;
				options.User.RequireUniqueEmail = true;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireDigit = false;
			})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<BLContext>();

			services.AddRazorPages();

		    services.AddDbContext<BLContext>(options =>
		            options.UseSqlServer(Configuration.GetConnectionString("BLContext")));
			//For getting the logged user
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddControllersWithViews().AddRazorRuntimeCompilation();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(	IApplicationBuilder app,
								IWebHostEnvironment env,
								UserManager<BLUser> userManager,
								RoleManager<IdentityRole> roleManager)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<BLContext>();
				DbInitializer.Initialize(context, userManager, roleManager);
			}

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
