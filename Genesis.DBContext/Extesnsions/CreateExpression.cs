using System.Linq.Expressions;
using System.Reflection;

namespace Genesis.DBContext.Extesnsions
{
    public static class CreateExpression
    {
        private static Expression<Func<T, bool>> CreateFilter<T>(string propertyName, string StrPropertyValue)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var property = typeof(T).GetProperty(propertyName);
            if (property == null) throw new InvalidDataException($"Invalid property name {propertyName} for class {typeof(T).Name}");
            var propertyType = property.PropertyType;

            var expLeft = Expression.MakeMemberAccess(parameter, property);
            var propertyValue = ConvertToType(propertyType, StrPropertyValue);
            var expRight = Expression.Constant(propertyValue, propertyType);
            var equals = Expression.Equal(expLeft, expRight);

            return Expression.Lambda<Func<T, bool>>(equals, parameter);
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
