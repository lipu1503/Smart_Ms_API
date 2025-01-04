using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SmartManagement.Common
{
    public class AppConfigSetting
    {
        public static IConfiguration Suit { get; set; }
        public static IHostEnvironment HostEnvironment { get; set; }
    }
}
