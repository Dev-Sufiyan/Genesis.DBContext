using Genesis.Models.DTO;
using Genesis.Models.Enums;
using Genesis.Repositories.Expressions;
using Genesis.Repositories.Tests.Models;
using Genesis.Repositories.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Repositories.Tests
{
    public class OrderByBuilderTest
    {
        private static Expression<Func<T, object>> GetLambdaExpression<T>(Expression<Func<T, object>> expression) => expression;

        [Fact]
        public void BuildExpression_OrderBy()
        {
            var expression = OrderByBuilder.BuildExpression<TestModel>(nameof(TestModel.IntField));
            var expectedExpression = GetLambdaExpression<TestModel>(x => x.IntField);

            Assert.True(expression.IsEqual(expectedExpression));
        }
    }
}
