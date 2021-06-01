using akvelon_test_data;
using akvelon_test_service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;


namespace akvelon_test
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
            // Getting connection string form the appsettings json
            services.AddDbContext<DbCtx>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Connecting our services from akvelon-test-service project for data manipulations
            services.AddScoped<ITaskItemOperations, TaskItemService>();
            services.AddScoped<IProjectOperations, ProjectService>();

            services.AddControllers()
                .AddNewtonsoftJson(o =>
                o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // swagger enable
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Akv-test",
                    Description = "Swagger for akv-test app",                    
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Anton Bolotnov",
                        Email = "antonbolotnov@gmail.com"
                        //Url = new System.Uri("http://google.com")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}
