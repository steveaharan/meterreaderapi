using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using EN.Sek.Meter.Entities;
using EN.Sek.Meter.BLL;
using EN.Sek.Meter.DAL;

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
		services.AddControllers();

		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(
				Configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly("MeterReadingApi")));

		services.AddScoped<IMeterReadingManager, MeterReadingManager>();
		services.AddScoped<IAccountDataProvider, AccountDataProvider>();
		services.AddScoped<IMeterReadingDataProvider, MeterReadingDataProvider>();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			// endpoints.MapControllers();
		});
	}
}