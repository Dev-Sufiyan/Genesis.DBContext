using Genesis.Models.Enums;
using Genesis.DBContext.Expressions;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Genesis.DBContext.Tests
{
    public class ComparisonBuilderTests
    {
        public class Person
        {
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        [Fact]
        public void BuildExpression_EqualOperator_ReturnsCorrectExpression()
        {
            // Arrange
            var propertyName = "Name";
            var propertyValue = "Alice";
            var operatorType = ComparisonOperator.Equal;

            // Act
            var expression = ComparisonBuilder.BuildExpression<Person>(propertyName, propertyValue, operatorType);
            var compiled = expression.Compile();
            var person = new Person { Name = "Alice" };

            // Assert
            Assert.True(compiled(person));
        }

        [Fact]
        public void BuildExpression_StartsWithOperator_ReturnsCorrectExpression()
        {
            // Arrange
            var propertyName = "Name";
            var propertyValue = "Al";
            var operatorType = ComparisonOperator.StartsWith;

            // Act
            var expression = ComparisonBuilder.BuildExpression<Person>(propertyName, propertyValue, operatorType);
            var compiled = expression.Compile();
            var person = new Person { Name = "Alice" };

            // Assert
            Assert.True(compiled(person));
        }

        [Fact]
        public void ConvertToType_ValidConversion_ReturnsCorrectValue()
        {
            // Arrange
            var targetType = typeof(int);
            var value = "123";

            // Act
            var result = ComparisonBuilder.ConvertToType(targetType, value);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(123, result);
        }

    }
}
