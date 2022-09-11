using BookingApi.Features.Customer.Commands;
using BookingApi.Features.Customer.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Features.Customer;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> logger;
    private readonly GetCustomers getCustomers;
    private readonly GetCustomerById getCustomerById;
    private readonly CreateCustomer createCustomer;

    public CustomerController(ILogger<CustomerController> logger, GetCustomers getCustomers,
        GetCustomerById getCustomerById, CreateCustomer createCustomer)
    {
        this.logger = logger;
        this.getCustomers = getCustomers;
        this.getCustomerById = getCustomerById;
        this.createCustomer = createCustomer;
    }

    [HttpGet("GetAll")]
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

    [HttpGet("GetById/id/{id:long}")]
    public IActionResult GetById(long id)
    {
        try
        {
            var customer = getCustomerById.Handle(id);

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the customer");
        }
    }

    [HttpPost("CreateCustomer")]
    public IActionResult CreateCustomer(Model.Customer customer)
    {
        try
        {
            createCustomer.Handle(customer);
        }
        catch (Exception e) when (e is ArgumentNullException or ArgumentException)
        {
            logger.LogError(e.ToString());
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Unexpected error saving the booking");
        }

        return Created("", customer);
    }
}