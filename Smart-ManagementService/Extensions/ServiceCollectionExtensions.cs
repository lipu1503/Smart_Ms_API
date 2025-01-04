using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Smart_ManagementService.Controllers;
using SmartManagement.Common;
using SmartManagement.Domain.Utilities;
using SmartManagement.Infrastructure.Cache;
using SmartManagement.Infrastructure.Result;
using SmartMangement.Infrastructure.Extensions;
using System.Text;

namespace Smart_ManagementService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services, WebApplicationBuilder builder, IConfiguration configuration)
        {
            services.AddSwagger();
            services.RegisterInfrastructureServices(configuration);

            services.TryAddSingleton<IInterceptor, CacheInterceptor>();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            return services;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.CustomSchemaIds(type => type.FullName);
                var knowledge = "";
                var github = "";
                var marketPlace = "";
                var Orchestra = "";
                StringBuilder desc = new StringBuilder("API Documentation & Testing Environment for Developers and Product owners <br/>");
                desc.AppendLine(marketPlace + "<br/>");
                desc.AppendLine(knowledge + "<br/>");
                desc.AppendLine(github + "<br/>");
                desc.AppendLine(Orchestra);
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Smart Tech API Micriservice",
                    Version = "v1",
                    Description = desc.ToString(),
                });

                option.AddSecurityDefinition("SwaggerBasic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter Valid autherization Token from channel secure",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Basic"
                });

                option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "SwaggerBasic"
                            }
                        }, new string[] { }
                    }
                });
                var presName = $"{typeof(UserController).Assembly.GetName().Name}.xml";
                var presPath = Path.Combine(AppContext.BaseDirectory, presName);
                var domName = $"{typeof(Result).Assembly.GetName().Name}.xml";
                var domPath = Path.Combine(AppContext.BaseDirectory, domName);
                option.IncludeXmlComments(presPath);
                option.IncludeXmlComments(domPath);
            });
        }
        public static string GetEnvironmentName(this IServiceCollection services)
        {
            string result = string.Empty;
            string test = SmartEncryptionSuit.Decrypt(AppConfigSetting.Suit.GetValue<string>("ConnectionString:SmartConnection"));
            string[] array = test.Split(';');
            if (array is not null && array.Length != 0)
            {
                using IEnumerator<string> enumerator = (from string env in array
                                                        where env.Trim().ToUpper().StartsWith("INITIAL")
                                                        let dbName = env.Split("=")[1]
                                                        select dbName).GetEnumerator();
                if (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    result = current.Substring(current.LastIndexOf("_") + 1).ToUpper();
                }

            }
            return result;
        }
    }
    public static class ApplicationHttpContext
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static HttpContext Current
        {
            get
            {
                return _httpContextAccessor?.HttpContext;
            }
            set
            {
                _httpContextAccessor.HttpContext = value;
            }
        }
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }

}