using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using WhizzBase.Base;

namespace WhizzORM.Validation
{
    public class WhizzAbstractValidator<T> : AbstractValidator<T>
    {
        public IRuleBuilderInitial<T, TProperty> RuleFor<TProperty>(string propertyName)
        {
            var type = typeof(T);
            var parameterExpression = Expression.Parameter(type, "TypeParameter");

            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null) 
                throw new DbException($"The Type '{type}' does not have property '{propertyName}'.");
            
            var propertyType = propertyInfo.PropertyType;
            var genericFuncType = typeof(Func<,>).MakeGenericType(type, propertyType);
            var lambdaMethodInfo = typeof(Expression).GetMethods().First(a => a.Name == "Lambda" && a.GetParameters().Length == 2);
            var genericMethodInfo = lambdaMethodInfo.MakeGenericMethod(genericFuncType);
            var expression = genericMethodInfo.Invoke(null, new object[] { Expression.PropertyOrField(parameterExpression, propertyName), new[] { parameterExpression }}) as Expression<Func<T, TProperty>>;

            return RuleFor(expression);
        }
    }
}