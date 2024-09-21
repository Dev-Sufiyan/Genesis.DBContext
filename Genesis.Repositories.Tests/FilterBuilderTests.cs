using Genesis.Models.Enums;
using Genesis.Models.DTO;
using Genesis.Repositories.Expressions;
using System.Linq.Expressions;
using Genesis.Repositories.Tests.Models;
using Genesis.Repositories.Tests.Utils;

namespace Genesis.Repositories.Tests
{
    public class FilterBuilderTests
    {
        // Helper method to create a lambda expression
        private static Expression<Func<T, bool>> GetLambdaExpression<T>(Expression<Func<T, bool>> expression) => expression;

        [Fact]
        public void BuildExpression_IntEqual()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.Equal }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField == 16);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntNotEqual()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.NotEqual }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField != 16);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntGreaterThan()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.GreaterThan }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField > 16);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntGreaterThanOrEqual()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.GreaterThanOrEqual }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField >= 16);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntLessThan()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.LessThan }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField < 16);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_IntLessThanOrEqual()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.LessThanOrEqual }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField <= 16);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_StringStartsWith()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.StringField), Value = "Sufiyan", Comparator = ComparisonOperator.StartsWith }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.StringField.StartsWith("Sufiyan"));

            Assert.True(expression.IsEqual(expectedExpression));
        }
        [Fact]
        public void BuildExpression_StringContains()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.StringField), Value = "Sufiyan", Comparator = ComparisonOperator.Contains }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.StringField.Contains("Sufiyan"));

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_StringEndWith()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.StringField), Value = "Sufiyan", Comparator = ComparisonOperator.EndsWith }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.StringField.EndsWith("Sufiyan"));

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_MultipleConditions_And()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.Equal },
                new SearchFilters { Field = nameof(TestModel.StringField), Value = "Aadil", Comparator = ComparisonOperator.StartsWith }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField == 16 && x.StringField.StartsWith("Aadil"));

            Assert.True(expression.IsEqual(expectedExpression));
        }
        [Fact]
        public void BuildExpression_MultipleConditions_Or()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.IntField), Value = 16.ToString(), Comparator = ComparisonOperator.Equal },
                new SearchFilters { Field = nameof(TestModel.StringField), Value = "Sufiyan", Comparator = ComparisonOperator.Contains }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss, JoinOperator.JointOr);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField == 16 || x.StringField.Contains("Sufiyan"));

            Assert.True(expression.IsEqual(expectedExpression));
        }
        [Fact]
        public void BuildExpression_LongConversion()
        {
            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.LongField), Value = long.MaxValue.ToString(), Comparator = ComparisonOperator.Equal }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.LongField == long.MaxValue);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_DoubleConversion()
        {

            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.DoubleField), Value = "0.0", Comparator = ComparisonOperator.Equal }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.DoubleField == 0.0);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_BooleanConversion_True()
        {

            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.BoolField), Value = "true", Comparator = ComparisonOperator.Equal }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.BoolField == true);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_BooleanConversion_False()
        {

            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.BoolField), Value = "false", Comparator = ComparisonOperator.Equal }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.BoolField == false);

            Assert.True(expression.IsEqual(expectedExpression));
        }

        [Fact]
        public void BuildExpression_DateTimeConversion()
        {

            var SearchFilterss = new List<SearchFilters>
            {
                new SearchFilters { Field = nameof(TestModel.DateTimeField), Value = "31-01-2024", Comparator = ComparisonOperator.Equal }
            };

            var expression = FilterBuilder.BuildExpression<TestModel>(SearchFilterss);

            //Write Logic to Check Forther
        }
    }
}
