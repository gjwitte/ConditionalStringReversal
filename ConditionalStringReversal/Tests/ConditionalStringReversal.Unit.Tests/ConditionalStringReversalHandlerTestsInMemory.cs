using ConditionalStringReversal.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace ConditionalStringReversal.Unit.Tests
{
    [TestFixture]
    public class ConditionalStringReversalHandlerTestsInMemory
    {
        private ConditionalStringReversalHandler _handler;
        private MortgageConnectDbContext _dbContext;

        [OneTimeSetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MortgageConnectDbContext>();
            optionsBuilder.UseInMemoryDatabase("testContext");
            var options = optionsBuilder.Options;
            _dbContext = new MortgageConnectDbContext(options);
            
            _handler = new ConditionalStringReversalHandler(_dbContext);
        }

        [Test]
        public void Handle_alphanumericStringWithSpecialCharacters_SortsCorrectly()
        {
            var dataValue = "AB1;C2*D";

            _dbContext.StringToReverse.Add(new StringToReverse
            {
                DataValue = dataValue
            });

            _dbContext.SaveChanges();

            var dbId = _dbContext.StringToReverse.First(s => s.DataValue == dataValue).Id;

            var response = _handler.Handle(dbId);

            response.Should().Be("D 2 C ; 1 B * A");
        }

        [Test]
        public void Handle_alphanumericStringWithoutSpecialCharacters_ReversesString()
        {
            var dataValue = "ABC123";

            _dbContext.StringToReverse.Add(new StringToReverse
            {
                DataValue = dataValue
            });

            _dbContext.SaveChanges();

            var dbId = _dbContext.StringToReverse.First(s => s.DataValue == dataValue).Id;

            var response = _handler.Handle(dbId);

            response.Should().Be("3 2 1 C B A");
        }

        [Test]
        public void Handle_stringOfAllSpecialCharacters_StringDoesNotChange()
        {
            var dataValue = "!@#$%^";

            _dbContext.StringToReverse.Add(new StringToReverse
            {
                DataValue = dataValue
            });

            _dbContext.SaveChanges();

            var dbId = _dbContext.StringToReverse.First(s => s.DataValue == dataValue).Id;

            var response = _handler.Handle(dbId);

            response.Should().Be("! @ # $ % ^");
        }

        [Test]
        public void Handle_incorrectDbInput_ThrowsException()
        {
            var dbId = int.MaxValue;

            Action comparison = () =>
            {
                _handler.Handle(dbId);
            };

            comparison.Should().ThrowExactly<ArgumentException>();
        }
    }
}
