using BookingApi.Features.Customer.Commands;
using Data;
using Data.Repository;

namespace BookingApi.UnitTests.Features.Customer.Commands;

[TestFixture]
public class CreateCustomerTests
{
#nullable disable
    private Mock<IUnitOfWork> unitOfWork;
    private Mock<ICustomerRepository> customerRepository;
    private CreateCustomer createCustomer;
    private Model.Customer customer;
#nullable enable

    [SetUp]
    public void SetUp()
    {
        customerRepository = new Mock<ICustomerRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Customers).Returns(customerRepository.Object);

        createCustomer = new CreateCustomer(unitOfWork.Object);
        customer = new Model.Customer();
    }

    [Test]
    public void Handle_CustomerIsNull_ThrowsArgumentNullException()
    {
        Assert.That(() => createCustomer.Handle(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void Handle_NameIsEmpty_ThrowsArgumentException()
    {
        customer.Name = string.Empty;

        Assert.That(() => createCustomer.Handle(customer), Throws.ArgumentException);
    }

    [Test]
    public void Handle_PhoneIsEmpty_ThrowsArgumentException()
    {
        customer.Phone = string.Empty;

        Assert.That(() => createCustomer.Handle(customer), Throws.ArgumentException);
    }

    [Test]
    public void Handle_CustomerAlreadyExist_ThrowsArgumentException()
    {
        customerRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns(new Model.Customer());
        
        Assert.That(() => createCustomer.Handle(customer), Throws.ArgumentException);
    }

    [Test]
    public void Handle_CustomerCanBeSaved_ThrowsNothing()
    {
        customer.Name = "a";
        customer.Phone = "0";
        
        Assert.That(() => createCustomer.Handle(customer), Throws.Nothing);
    }
}