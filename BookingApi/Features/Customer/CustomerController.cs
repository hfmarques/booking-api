using BookingApi.Features.Customer.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Features.Customer;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> logger;
    private readonly GetCustomers getCustomers;

    public CustomerController(ILogger<CustomerController> logger, GetCustomers getCustomers)
    {
        this.logger = logger;
        this.getCustomers = getCustomers;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var customers = getCustomers.Handle();

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the customers");
        }
    }
}