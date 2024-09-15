using Genesis.Models.Enums;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;

namespace Genesis.DBContext.Expressions
{
    public static class ComparisonBuilder
    {
        public static Expression<Func<T, bool>> BuildExpression<T>(string propertyName, string StrPropertyValue, ComparisonOperator comOperator)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var property = typeof(T).GetProperty(propertyName);
            if (property == null) throw new InvalidDataException($"Invalid property name {propertyName} for class {typeof(T).Name}");
            var propertyType = property.PropertyType;

            var expLeft = Expression.MakeMemberAccess(parameter, property);
            var propertyValue = ConvertToType(propertyType, StrPropertyValue);
            var expRight = Expression.Constant(propertyValue, propertyType);

            Expression comparison = comOperator switch
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
                _ => throw new NotImplementedException($"Comparison operator '{comOperator}' is not implemented.")
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
