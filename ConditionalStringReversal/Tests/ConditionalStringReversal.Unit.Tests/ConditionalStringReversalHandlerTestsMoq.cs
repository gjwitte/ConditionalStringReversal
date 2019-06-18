using ConditionalStringReversal.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConditionalStringReversal.Unit.Tests
{
    [TestFixture]
    public class ConditionalStringReversalHandlerTestsMoq
    {
        private ConditionalStringReversalHandler _handler;
        private Mock<MortgageConnectDbContext> _mockContext;
        private Mock<DbSet<StringToReverse>> _mockSet;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockSet = new Mock<DbSet<StringToReverse>>();
            _mockContext = new Mock<MortgageConnectDbContext>();
            CreateMockContext();
            _handler = new ConditionalStringReversalHandler(_mockContext.Object);
        }

        private void CreateMockContext()
        {
            var stringsToReverse = new List<StringToReverse>
            {
                new StringToReverse
                {
                    DataValue = "AB1;C2*D",
                    Id = 1
                },

                new StringToReverse
                {
                    DataValue = "ABC123",
                    Id = 2
                },

                new StringToReverse
                {
                    DataValue = "!@#$%^",
                    Id = 3
                }

            }.AsQueryable();

            _mockSet.As<IQueryable<StringToReverse>>()
                .Setup(m => m.Provider)
                .Returns(stringsToReverse.Provider);

            _mockSet.As<IQueryable<StringToReverse>>()
                .Setup(m => m.Expression)
                .Returns(stringsToReverse.Expression);

            _mockSet.As<IQueryable<StringToReverse>>()
                .Setup(m => m.ElementType)
                .Returns(stringsToReverse.ElementType);

            _mockSet.As<IQueryable<StringToReverse>>()
                .Setup(m => m.GetEnumerator())
                .Returns(stringsToReverse.GetEnumerator());

            _mockContext.Setup(c => c.StringToReverse)
                .Returns(_mockSet.Object);
        }

        [Test]
        public void Handle_alphanumericStringWithSpecialCharacters_SortsCorrectly()
        {
            var response = _handler.Handle(1);

            response.Should().Be("D 2 C ; 1 B * A");

        }

        [Test]
        public void Handle_alphanumericStringWithoutSpecialCharacters_ReversesString()
        {
            var response = _handler.Handle(2);
            response.Should().Be("3 2 1 C B A");
        }

        [Test]
        public void Handle_stringOfAllSpecialCharacters_StringDoesNotChange()
        {
            var response = _handler.Handle(3);
            response.Should().Be("! @ # $ % ^");
        }

        [Test]
        public void Handle_incorrectDbInput_ThrowsException()
        {
            Action comparison = () =>
            {
                _handler.Handle(int.MaxValue);
            };

            comparison.Should().ThrowExactly<ArgumentException>();
        }
    }
}