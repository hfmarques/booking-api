using Core.Repositories;

namespace Core.Features.Customer.Queries;

public interface IGetCustomerById
{
    Task<Domain.Entities.Customer?> Handle(long id);
}
public class GetCustomerById(ICustomerQueryRepository queryRepository) : IGetCustomerById
{
    public async Task<Domain.Entities.Customer?> Handle(long id) =>
        await queryRepository.GetByIdAsync(id);
}