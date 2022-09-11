using Data;

namespace BookingApi.Features.Customer.Queries;

public class GetCustomers
{
    private readonly IUnitOfWork unitOfWork;

    public GetCustomers(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public List<Model.Customer> Handle()
    {
        return unitOfWork.Customers.GetAll().ToList();
    }
}