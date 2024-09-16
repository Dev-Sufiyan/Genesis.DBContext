using Genesis.Models.DTO;
using Genesis.Models.Enums;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Genesis.Repository.Expressions
{
    public static class ComparisonBuilder
    {
        public static Expression<Func<T, bool>> BuildExpression<T>(List<SearchParam> searchParam)
        {
            if (searchParam == null || searchParam.Count == 0)
            {
                return x => true;
            }

            var initialExpression = BuildExpression<T>(searchParam[0]);
            var parameter = Expression.Parameter(typeof(T), "x");

            foreach (var searchObj in searchParam.Skip(1))
            {
                var newExpression = BuildExpression<T>(searchParam);
                initialExpression = CombineExpressions(initialExpression, newExpression, Expression.AndAlso);
            }
            return Expression.Lambda<Func<T, bool>>(initialExpression, parameter);

        }

        private static Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, Func<Expression, Expression, BinaryExpression> combine)
        {
            var leftBody = left.Body;
            var rightBody = right.Body;
            var parameter = left.Parameters[0];

            var combinedBody = combine(leftBody, rightBody);
            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }

        public static Expression<Func<T, bool>> BuildExpression<T>(SearchParam searchParam)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var property = typeof(T).GetProperty(searchParam.Field);
            if (property == null) throw new InvalidDataException($"Invalid property name {searchParam.Field} for class {typeof(T).Name}");
            var propertyType = property.PropertyType;

            var expLeft = Expression.MakeMemberAccess(parameter, property);
            var propertyValue = ConvertToType(propertyType, searchParam.Value);
            var expRight = Expression.Constant(propertyValue, propertyType);

            Expression comparison = searchParam.Comparator switch
            {
                ComparisonOperator.Equal => Expression.Equal(expLeft, expRight),
                ComparisonOperator.NotEqual => Expression.NotEqual(expLeft, expRight),
                ComparisonOperator.GreaterThan => Expression.GreaterThan(expLeft, expRight),
                ComparisonOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(expLeft, expRight),
                ComparisonOperator.LessThan => Expression.LessThan(expLeft, expRight),
                ComparisonOperator.LessThanOrEqual => Expression.LessThanOrEqual(expLeft, expRight),
                ComparisonOperator.StartsWith => (propertyType == typeof(string)
                               ? Expression.Call(expLeft, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }) ?? throw new Exception(), expRight)
                               : throw new InvalidOperationException("StartsWith operator is only valid for string properties.")),
                _ => throw new NotImplementedException($"Comparison operator '{searchParam.Comparator}' is not implemented.")
            };
            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }


        public static object ConvertToType(Type targetType, string value)
        {
            if (targetType == typeof(string)) return value;

            var parseMethod = targetType.GetMethod("Parse", new[] { typeof(string) });

            if (parseMethod != null)
            {
                try
                {
                    return parseMethod.Invoke(null, new[] { value }) ?? throw InvalidOperationException(targetType.Name);
                }
                catch (TargetInvocationException ex)
                {
                    throw InvalidOperationException(targetType.Name, ex.InnerException);
                }
            }

            throw new NotSupportedException($"Conversion to {targetType.Name} is not supported.");
        }

        public static InvalidOperationException InvalidOperationException(string tagetType, Exception? ex = null) => new InvalidOperationException($"Error converting value to {tagetType}.", ex);

    }
}
