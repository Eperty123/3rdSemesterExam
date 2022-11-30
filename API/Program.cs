using Application;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

var mapper = new MapperConfiguration(config =>
{
    config.CreateMap<RegisterUserDTO, User>();
    config.CreateMap<LoginUserDTO, User>();
    config.CreateMap<UpdateUserDTO, User>();

    config.CreateMap<RegisterUserDTO, Client>();
    config.CreateMap<RegisterUserDTO, Coach>();

    config.CreateMap<LoginUserDTO, Client>();
    config.CreateMap<LoginUserDTO, Coach>();

    config.CreateMap<UpdateUserDTO, Client>();
    config.CreateMap<UpdateUserDTO, Coach>();
}).CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(
    "Data source=db.db"
    ));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IDbSeeder, DbSeeder>();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddCors();

var app = builder.Build();

// Build an instance of the ServiceProvider to gain access to the different dependency injected classes.
var builtService = builder.Services.BuildServiceProvider();

var dbSeeder = builtService.GetService<IDbSeeder>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    dbSeeder.SeedDevelopment();
}
else dbSeeder.SeedProduction();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
