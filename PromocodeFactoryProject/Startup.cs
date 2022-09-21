using Core;
using Core.Repositories;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromocodeFactoryProject.Mappers;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using PromocodeFactoryProject.ErrorHandling;
using BusinessLogic;
using BusinessLogic.Abstractions;
using BusinessLogic.Services;

namespace PromocodeFactoryProject
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
            services.AddControllers(options=> options.Filters.Add(new HttpResponseExceptionFilter())).
                AddMvcOptions(options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                });
            services.AddScoped(typeof(IRepository<Employee>), typeof(EFRepository<Employee>));
            services.AddScoped(typeof(IRepository<Role>), typeof(EFRepository<Role>));
            services.AddScoped(typeof(IRepository<Preference>), typeof(EFRepository<Preference>));
            services.AddScoped(typeof(IRepository<Customer>), typeof(CustomerRepository));
            services.AddScoped(typeof(IRepository<PromoCode>), typeof(EFRepository<PromoCode>));
            services.AddScoped(typeof(IRepository<Partner>), typeof(EFRepository<Partner>));
            services.AddScoped<IDbInitializer, EFDbInitializer>();

            services.AddScoped<IEmployeeMapper, EmployeeMapper>();
            services.AddScoped<ICustomerMapper, CustomerMapper>();
            services.AddScoped<IPartnerMapper, PartnerMapper>();
            services.AddScoped<IPromoCodeMapper, PromoCodeMapper>();

            services.AddScoped<ICurrentDateTimeProvider, CurrentDateTimeProvider>();

            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddDbContext<DataContext>(x=>
            {
                x.UseSqlite("FileName = PromocodeDataBase.sqlite; ");
                x.UseLazyLoadingProxies();
                
            }
            );

            services.AddOpenApiDocument(options =>
            {
                options.DocumentName = "PromoCode Factory API Doc";
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });
            app.UseReDoc();
            app.UseRouting();

            app.UseMiddleware<ApplicationVersionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbInitializer.Initialize();
        }
    }
}
