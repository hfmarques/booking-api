using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerQueryRepository(DbContext context) : QueryRepository<Customer>(context), ICustomerQueryRepository;