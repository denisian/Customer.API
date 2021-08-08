using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Customer.Api.Persistence.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Tests.TestBuilder
{
    public class CustomerDbContextBuilder
    {
        private DbConnection _sqlConnection;
        private readonly List<Persistence.Entities.Customer> _customers;

        public CustomerDbContextBuilder()
        {
            _customers = new List<Persistence.Entities.Customer>();
        }

        public CustomerDbContextBuilder UseInMemorySqlite()
        {
            _sqlConnection = new SqliteConnection("DataSource=:memory:");
            return this;
        }

        public CustomerDbContextBuilder WithCustomer(Persistence.Entities.Customer customer)
        {
            _customers.Add(customer);
            return this;
        }

        public CustomerDbContext Build()
        {
            if (_sqlConnection == null)
                throw new Exception("SqlConnection has not be initialized.");

            if (_sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();

            var options = new DbContextOptionsBuilder<CustomerDbContext>().UseSqlite(_sqlConnection).Options;
            var dbContext = new CustomerDbContext(options);
            dbContext.Database.EnsureCreated();

            // create the required data.
            dbContext.Customers.AddRange(_customers);
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
