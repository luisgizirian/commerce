using WebApi.Models;
using WebApi.Services;
using Dapr;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CatalogStoreDatabaseSettings>(
    builder.Configuration.GetSection("CatalogStoreDatabase"));

builder.Services.AddSingleton<ICatalogService, CatalogService>();

builder.Services.AddDaprClient();
builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => {
        
        options.RequireHttpsMetadata = false;

        options.Authority = builder.Configuration["Authority"];
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
            ValidateAudience = false
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Dapr subscription in [Topic] routes orders topic to this route
app.MapPost("/orders", [Topic("pubsub", "orders")] (string message) => {
    Console.WriteLine("Subscriber received : " + message);
    return Results.Ok(message);
});

app.Run();
