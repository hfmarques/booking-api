using Data;

namespace BookingApi.Features.Customer.Commands;

public class CreateCustomer
{
    private readonly IUnitOfWork unitOfWork;

    public CreateCustomer(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public void Handle(Model.Customer customer)
    {
        if (customer is null)
            throw new ArgumentNullException(nameof(customer));

        if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.Phone))
            throw new ArgumentException("Name and phone are required");

        if (customer.Id != 0 &&
            unitOfWork.Customers.Get(customer.Id) is not null)
            throw new ArgumentException("Customer already exist");

        unitOfWork.Customers.Add(customer);
        unitOfWork.SaveChanges();
    }
}