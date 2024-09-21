using Genesis.Models.DTO;
using Genesis.Models.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace Genesis.Repositories.Expressions
{
    public static class FilterBuilder
    {
        public static Expression<Func<T, bool>> BuildExpression<T>(List<SearchFilters>? SearchFilters, JoinOperator joinOperator = JoinOperator.JointAnd)
        {
            if (SearchFilters == null || SearchFilters.Count == 0)
            {
                return x => true;
            }

            var initialExpression = GetExpression<T>(SearchFilters[0]);
            var parameter = initialExpression.Parameters[0];

            foreach (var searchObj in SearchFilters.Skip(1))
            {
                var newExpression = GetExpression<T>(searchObj);
                initialExpression = CombineExpressions(initialExpression, newExpression,
                                                        joinOperator == JoinOperator.JointAnd ? Expression.AndAlso : Expression.OrElse);
            }

            return initialExpression;
        }

        private static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right,
            Func<Expression, Expression, BinaryExpression> combine)
        {
            var leftBody = left.Body;
            var rightBody = right.Body;
            var parameter = left.Parameters[0];

            var combinedBody = combine(leftBody, rightBody);
            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }

        public static Expression<Func<T, bool>> GetExpression<T>(SearchFilters SearchFilters)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var property = typeof(T).GetProperty(SearchFilters.Field);
            if (property == null)
                throw new InvalidDataException($"Invalid property name {SearchFilters.Field} for class {typeof(T).Name}");

            var propertyType = property.PropertyType;

            var expLeft = Expression.MakeMemberAccess(parameter, property);
            var propertyValue = ConvertToType(propertyType, SearchFilters.Value);
            var expRight = Expression.Constant(propertyValue, propertyType);

            Expression comparison = SearchFilters.Comparator switch
            {
                ComparisonOperator.Equal => Expression.Equal(expLeft, expRight),
                ComparisonOperator.NotEqual => Expression.NotEqual(expLeft, expRight),
                ComparisonOperator.GreaterThan => Expression.GreaterThan(expLeft, expRight),
                ComparisonOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(expLeft, expRight),
                ComparisonOperator.LessThan => Expression.LessThan(expLeft, expRight),
                ComparisonOperator.LessThanOrEqual => Expression.LessThanOrEqual(expLeft, expRight),
                ComparisonOperator.StartsWith => CreateStringMethodCall("StartsWith", expLeft, expRight, propertyType),
                ComparisonOperator.EndsWith => CreateStringMethodCall("EndsWith", expLeft, expRight, propertyType),
                ComparisonOperator.Contains => CreateStringMethodCall("Contains", expLeft, expRight, propertyType),
                _ => throw new NotImplementedException($"Comparison operator '{SearchFilters.Comparator}' is not implemented.")
            };

            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        private static Expression CreateStringMethodCall(string methodName, Expression expLeft, Expression expRight, Type propertyType)
        {
            if (propertyType != typeof(string))
            {
                throw new InvalidOperationException($"{nameof(SearchFilters.Comparator)} operator is only valid for string properties.");
            }

            return Expression.Call(expLeft, typeof(string).GetMethod(methodName, new[] { typeof(string) })
                ?? throw new InvalidOperationException($"{methodName} method not found"), expRight);
        }

        public static object ConvertToType(Type targetType, string value)
        {
            if (targetType == typeof(string)) return value;

            if (targetType == typeof(DateTime))
            {
                var dateFormat = "dd-MM-yyyy";
                if (DateTime.TryParseExact(value, dateFormat, null, System.Globalization.DateTimeStyles.None, out var dateTimeValue))
                {
                    return dateTimeValue;
                }
                throw new FormatException($"Unable to parse '{value}' as a DateTime with format '{dateFormat}'.");
            }

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

        public static InvalidOperationException InvalidOperationException(string targetType, Exception? ex = null)
            => new InvalidOperationException($"Error converting value to {targetType}.", ex);
    }
}
