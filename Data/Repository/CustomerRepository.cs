using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repository;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(DbContext context) : base(context)
    {
    }
}