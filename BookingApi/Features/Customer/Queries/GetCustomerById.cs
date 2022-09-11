using Data;

namespace BookingApi.Features.Customer.Queries;

public class GetCustomerById
{
    private readonly IUnitOfWork unitOfWork;

    public GetCustomerById(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Model.Customer? Handle(long id)
    {
        return unitOfWork.Customers.Get(id);
    }
}