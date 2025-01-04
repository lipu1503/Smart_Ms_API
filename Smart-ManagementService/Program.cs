using Microsoft.AspNetCore.Mvc.Versioning;
using Smart_ManagementService.Extensions;
using SmartManagement.Common;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsetting." + builder.Environment.EnvironmentName + ".json", optional: true)
    .AddEnvironmentVariables().Build();
AppConfigSetting.Suit = configuration;
AppConfigSetting.HostEnvironment = builder.Environment;
// Add services to the container.

builder.Services.AddInternalServices(builder, configuration);
builder.Services.AddMvcCore();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
var environment = AppConfigSetting.Suit.GetValue<string>("Environment");
if (environment is not null && (environment.Contains("Dev") || environment.Contains("LOCAL")))
{
    app.UseSwagger();
    app.UseSwaggerUI(C =>
    {
        C.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Management API Documentation");
    });
}
ApplicationHttpContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
