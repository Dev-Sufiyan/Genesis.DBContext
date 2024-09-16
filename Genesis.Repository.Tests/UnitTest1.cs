using Genesis.Models.Enums;
using Genesis.Models.DTO;
using Genesis.Repository.Expressions;
using Genesis.Repository.Tests;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Genesis.Repository.Tests
{
    public class ComparisonBuilderTests
    {
        // Helper method to create a lambda expression
        private static Expression<Func<T, bool>> CreateLambdaExpression<T>(Expression<Func<T, bool>> expected)
        {
            return expected as Expression<Func<T, bool>>;
        }

        [Fact]
        public void BuildExpression_IntEqual()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.Equal }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.IntField == 16);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntNotEqual()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.NotEqual }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.IntField != 16);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntGreaterThan()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.GreaterThan }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.IntField > 16);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntGreaterThanOrEqual()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.GreaterThanOrEqual }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.IntField >= 16);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntLessThan()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.LessThan }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.IntField < 16);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntLessThanOrEqual()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.LessThanOrEqual }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.IntField <= 16);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_StringStartsWith()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.StringField), Value = "John", Comparator = ComparisonOperator.StartsWith }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.StringField.StartsWith("John"));

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_MultipleConditions()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.Equal },
                new SearchParam { Field = nameof(TestModel.StringField), Value = "John", Comparator = ComparisonOperator.StartsWith }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.IntField == 16 && x.StringField.StartsWith("John"));

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }
        [Fact]
        public void BuildExpression_LongConversion()
        {
            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.LongField), Value = long.MaxValue.ToString(), Comparator = ComparisonOperator.Equal }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.LongField == long.MaxValue);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_DoubleConversion()
        {

            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.DoubleField), Value = "0.0", Comparator = ComparisonOperator.Equal }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.DoubleField == 0.0);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_BooleanConversion_True()
        {

            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.BoolField), Value = "true", Comparator = ComparisonOperator.Equal }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.BoolField == true);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_BooleanConversion_False()
        {

            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.BoolField), Value = "false", Comparator = ComparisonOperator.Equal }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);
            var expectedExpression = CreateLambdaExpression<TestModel>(x => x.BoolField == false);

            Assert.True(ExpressionsAreEqual(expression, expectedExpression));
        }

        [Fact]
        public void BuildExpression_DateTimeConversion()
        {

            var searchParams = new List<SearchParam>
            {
                new SearchParam { Field = nameof(TestModel.DateTimeField), Value = "31-01-2024", Comparator = ComparisonOperator.Equal }
            };

            var expression = ComparisonBuilder.BuildExpression<TestModel>(searchParams);

            //Write Logic to Check Forther
        }

        // Utility method to check if two expressions are equal
        private static bool ExpressionsAreEqual(Expression expr1, Expression expr2)
        {
            if (expr1 == null || expr2 == null) return false;
            return expr1.ToString() == expr2.ToString();
        }
    }
}
