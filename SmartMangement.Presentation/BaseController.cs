using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace SmartMangement.Presentation;

    [ApiController]
    [Route("/api/SmartManagement")]
    public class BaseController: Controller
    {
        [ExcludeFromCodeCoverage]
        public BaseController(): base() { }
    }

