using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using MediatR;
using VacationRental.Database;
using Microsoft.EntityFrameworkCore;
using VacationRental.BusinessLogic.Models.Rentals;
using VacationRental.BusinessLogic.Models.Bookings;

var MyAllowSpecificOrigins = "MyPolicy";
const string swaggerTitle = "Vacation Rental";
const string swaggerVersion = "v1";
const string swaggerUrl = "/swagger/v1/swagger.json";
const string swaggerName = "Vacation Rental v1";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                      });
});

{
    var services = builder.Services;

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(configure =>
    {
        configure.SwaggerDoc(swaggerVersion, new OpenApiInfo
        {
            Title = swaggerTitle,
            Version = swaggerVersion
        });
    });
    services.AddDbContext<VRContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
    services.AddMediatR(typeof(BookingViewModel));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts => opts.SwaggerEndpoint(swaggerUrl, swaggerName));
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
