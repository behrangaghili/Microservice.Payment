using Postex.Payment.Application.Contracts.Saman;

namespace Postex.Payment.Api.Controllers;


public class PaymentSamanController : Controller
{

    [HttpGet("samanconnect")]
    public IActionResult SamanHandler([FromQuery]SamanPayModel model)
    {
        return View("~/Views/PaymentSaman/SamanTest.cshtml", model);
    }
}

