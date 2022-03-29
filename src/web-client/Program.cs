var appName = "Web Client";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddCustomSerilog();
builder.Services.AddDaprClient();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("c", client => {
    client.BaseAddress = new Uri("http://gateway");
});
builder.Services.AddScoped<ICmmrcApi, CmmrcApi>();

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

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
