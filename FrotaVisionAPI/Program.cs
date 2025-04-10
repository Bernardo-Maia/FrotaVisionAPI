using Microsoft.EntityFrameworkCore;
using FrotaVisionAPI.Models;
using FrotaVisionAPI;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//builder.Services.AddDbContext<AppDBContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("MySqlDatabase"),
//        new MySqlServerVersion(new Version(8, 0, 41)) // Ajuste para a versão do seu MySQL
//    )
//);

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresql"))
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
