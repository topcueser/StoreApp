using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<StoreContext>(x => x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(MappingProfiles));

            // Startup kalabalık olmasın. Kendimiz extension yazarak onun içine dahil ettik.
            services.AddApplicationServices();
            // Swagger için kendi yazdığımız extension. Startup kalabalık olmasın
            services.AddSwaggerDocumentation();

            // API dışarıdan erişim sağlayacak adres ve özelliklerini belirledik.
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // custom olarak oluşturduğumuz exception sınıfı üzerinden hatalar gösterilecek
            // IsDevelopment() içerisindeki default hata olan app.UseDeveloperExceptionPage() kapattık ve if (env.IsDevelopment()) ı kaldırdık
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseRouting();
            app.UseStaticFiles();

            //Yukarıda ekledigimiz CORS ozelligi
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            // Startup kalabalık olmasın diye yazdığımız extension
            app.UseSwaggerDocumention();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
