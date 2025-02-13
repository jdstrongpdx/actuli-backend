using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

using Actuli.Api.DbContext;
using Actuli.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                builder.Configuration.Bind("AzureAd", options);
                options.Events = new JwtBearerEvents();

                /// <summary>
                /// Below you can do extended token validation and check for additional claims, such as:
                ///
                /// - check if the caller's tenant is in the allowed tenants list via the 'tid' claim (for multi-tenant applications)
                /// - check if the caller's account is homed or guest via the 'acct' optional claim
                /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
                ///
                /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
                /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
                /// </summary>

                //options.Events.OnTokenValidated = async context =>
                //{
                //    string[] allowedClientApps = { /* list of client ids to allow */ };

                //    string clientappId = context?.Principal?.Claims
                //        .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

                //    if (!allowedClientApps.Contains(clientappId))
                //    {
                //        throw new System.Exception("This client is not authorized");
                //    }
                //};
            }, options => { builder.Configuration.Bind("AzureAd", options); });


builder.Services.AddDbContext<ApplicationUserDbContext>(options =>
{
    options.UseInMemoryDatabase("ActuliDb");
});

builder.Services.AddControllers();

// Add Swagger for API documentation
if (environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

// Configure CORS for React App 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with React app URL
            .AllowAnyMethod() // Allow GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader() // Allow all headers
            .AllowCredentials(); // Allow cookies/credentials
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Since IdentityModel version 5.2.1 (or since Microsoft.AspNetCore.Authentication.JwtBearer version 2.2.0),
    // Personal Identifiable Information is not written to the logs by default, to be compliant with GDPR.
    // For debugging/development purposes, one can enable additional detail in exceptions by setting IdentityModelEventSource.ShowPII to true.
    // Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable CORS policy for React frontend
app.UseCors("AllowReactApp");

// app.UseHttpsRedirection();
app.UseMiddleware<SecurityMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
