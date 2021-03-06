﻿using Quiniela.Data;
using Quiniela.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Quiniela.Dao;
using Quiniela.Dao.DaoImpl;
using AutoMapper;

namespace Quiniela
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
            services.AddDbContext<QuinielaContext>(
                options => options
                    .UseLoggerFactory(MyLoggerFactory)
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();
            services.AddAutoMapper();
            services.AddScoped<IQuinielaServices, QuinielaService>();
            services.AddScoped<IUserDAO, UserDAO>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[]
        {
            new ConsoleLoggerProvider((category, level)
                => category == DbLoggerCategory.Database.Command.Name
                   && level == LogLevel.Information, true)
        });
    }
}
