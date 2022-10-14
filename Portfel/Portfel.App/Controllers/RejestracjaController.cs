using Microsoft.AspNetCore.Mvc;

namespace Portfel.App.Controllers
{
    public class RejestracjaController : Controller
    {
        private readonly ILogger <RejestracjaController> _logger;

        public RejestracjaController(ILogger<RejestracjaController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
