using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var appName = "Web Client";
var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.AddCustomSerilog();
builder.Services.AddDaprClient();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("apientry", client => {
    client.BaseAddress = new Uri(configuration["GATEWAY_ADDR"]);
});
builder.Services.AddScoped<ICmmrcApi, CmmrcApi>();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options => {
        options.Authority = configuration["Authority"];

        options.RequireHttpsMetadata = false;

        options.ClientId = "interactive";
        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("catalog.api");
        options.Scope.Add("verification");
        options.Scope.Add("offline_access");
        options.ClaimActions.MapJsonKey("roles", "roles");
        // options.ClaimActions.MapJsonKey("email_verified", "email_verified");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role
        };
        
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages()
    .RequireAuthorization();

try
{
    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Host terminated unexpectedly ({ApplicationName})...", appName);
}
finally
{
    Serilog.Log.CloseAndFlush();
}
