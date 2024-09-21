using Genesis.Models.DTO;
using Genesis.Models.Enums;
using System;
using System.Linq.Expressions;

namespace Genesis.Repositories.Expressions
{
    public class OrderByBuilder
    {
        public static Expression<Func<T, object>> BuildExpression<T>(string fieldName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = typeof(T).GetProperty(fieldName);
            if (property == null)
                throw new InvalidDataException($"Invalid property name {fieldName} for class {typeof(T).Name}");

            var propertyExp = Expression.MakeMemberAccess(parameter, property);
            var conversion = Expression.Convert(propertyExp, typeof(object)); // casting to object for compatibility

            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }
    }
}