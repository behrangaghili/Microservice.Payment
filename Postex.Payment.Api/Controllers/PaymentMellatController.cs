namespace Postex.Payment.Api.Controllers;


public class PaymentMellatController : Controller
{

    [HttpGet("mellatconnect/{refId}")]
    public IActionResult MellatHandler(string refId,string mobileNo)
    {
        ViewBag.MobileNo = mobileNo;
        return View("Default",refId);
    }
}

