using System.Threading;
using Customer.Api.Handler.Customer;
using Customer.Api.Persistence.Data;
using Customer.Api.Persistence.Entities;
using Customer.Api.Tests.TestBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Customer.Api.Tests.Handlers
{
    [TestClass]
    public class GetCustomerTest
    {
        private CustomerDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            _dbContext = new CustomerDbContextBuilder()
                .UseInMemorySqlite()
                .WithCustomer(new Persistence.Entities.Customer()
                {
                    CustomerId = 1,
                    FirstName = "Denis",
                    LastName = "Klyucherov",
                    Status = new Status()
                    {
                        StatusId = 1,
                        Description = "Current"
                    }
                })
                .Build();
        }

        [TestMethod, Description("Must return customer when customerId exists")]
        public void Should_GetCustomerResponse_WhenCustomerExists()
        {
            var request = new GetCustomerRequest()
            {
                CustomerId = "1"
            };

            var getCustomerHandler = new GetCustomerHandler(_dbContext);

            var response = getCustomerHandler.Handle(request, CancellationToken.None).Result;

            Assert.IsNotNull(response, "Response does not contain elements");
            Assert.AreEqual("Denis", response.FirstName, "Invalid FirstName returned");
        }
    }
}
