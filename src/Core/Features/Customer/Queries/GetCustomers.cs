using Core.Repositories;

namespace Core.Features.Customer.Queries;

public interface IGetCustomers
{
    Task<List<Domain.Entities.Customer>> Handle();
}
public class GetCustomers(ICustomerQueryRepository queryRepository) : IGetCustomers
{
    public async Task<List<Domain.Entities.Customer>> Handle() =>
        (await queryRepository.GetAllAsync()).ToList();
}