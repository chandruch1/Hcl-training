using CoursesWebApp.Models;
using CoursesWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebApp.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentService payments) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await payments.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> MakePayment(MakePaymentRequest request)
    {
        try { return Ok(await payments.MakePaymentAsync(request.EnrollmentId, request.Mode)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
