using jwtAuthentication.Data;
using jwtAuthentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MyDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))
    });

builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler(exception => {
    exception.Run(context => 
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType= "application/json";
        var exceptionService=context.Features.Get<IExceptionHandlerFeature>();
        if(exceptionService != null)
        {
            string errorMessage = exceptionService.Error.Message;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
              Error=  errorMessage,
              StatusCode=500
            }));
        }
        else
        {
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Error = "Something went wrong please try again!",
                StatusCode = 500
            }));

        }
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
