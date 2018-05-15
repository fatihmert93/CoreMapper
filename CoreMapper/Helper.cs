using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CoreMapper
{
    public static class Helper
    {
        private static int _typeCounter;
        private static readonly ModuleBuilder _moduleBuilder;

        public static bool IsPrimitive(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
                typeInfo = type.GetTypeInfo();
            }

            return !typeInfo.IsGenericType && (typeInfo.IsValueType || type == typeof(string));
        }

        public static object GetPropertyValue(object obj, string propName)
        {
            var propertyInfo = obj.GetType().GetRuntimeProperties().FirstOrDefault(p => p.Name == propName);

            return propertyInfo?.GetValue(obj, null);
        }

        public static object GetFieldValue(object obj, string fieldName)
        {
            var fieldInfo = obj.GetType().GetRuntimeFields().FirstOrDefault(f => f.Name == fieldName);

            return fieldInfo.GetValue(obj);
        }

        public static IEnumerable<MapperMember> GetMapperFields(Type type, bool onlyWritable = false)
        {
            var properties = type.GetRuntimeProperties().Where(v => v.GetMethod.IsPublic && !v.GetMethod.IsStatic);
            if (onlyWritable)
            {
                properties = properties.Where(v => v.CanWrite && v.SetMethod.IsPublic);
            }

            var fields = type.GetRuntimeFields().Where(v => v.IsPublic && !v.IsStatic);

            return fields.Select(f => new MapperMember(f.Name, f.FieldType, f))
                .Concat(properties.Select(p => new MapperMember(p.Name, p.PropertyType, p)));
        }

        public static List<string> GetMemberPath(Expression exp)
        {
            var retVal = new List<string>();

            if (exp is MethodCallExpression callExp && callExp.Method.DeclaringType == typeof(Enumerable) &&
                callExp.Method.Name == "Select")
            {
                retVal.AddRange(GetMemberPath(callExp.Arguments[0]));
                retVal.AddRange(GetMemberPath(((LambdaExpression)callExp.Arguments[1]).Body));
                return retVal;
            }

            if (exp is MemberExpression memberExp)
            {
                retVal.AddRange(GetMemberPath(memberExp.Expression));
                retVal.Add(memberExp.Member.Name);
                return retVal;
            }

            if (exp is UnaryExpression unaryExp)
                return GetMemberPath(unaryExp.Operand);

            if (exp is LambdaExpression lambdaExpression)
                return GetMemberPath(lambdaExpression.Body);

            return retVal;
        }

        public static IEnumerable<IncludePath> ParseIncludes(IEnumerable<IEnumerable<string>> includes)
        {
            return includes.Where(i => i.Any())
                .GroupBy(i => i.First(), i => i.Skip(1))
                .Select(g => new IncludePath(g.Key, ParseIncludes(g.ToList())));
        }

        public static IEnumerable<IncludePath> ParseIncludes<TIn>(
            IEnumerable<Expression<Func<TIn, object>>> expressions)
        {
            return ParseIncludes(expressions.Select(e => GetMemberPath(e.Body)));
        }

        public static IEnumerable<IncludePath> GetIncludes(IQueryable query)
        {
            return new IncludeVisitor().GetIncludes(query);
        }

        public static int GenerateHashCode(object o1, object o2)
        {
            var h1 = o1.GetHashCode();
            var h2 = o2.GetHashCode();
            return ((h1 << 5) + h1) ^ h2;
        }

        public static int GenerateHashCode(object o1, object o2, object o3)
        {
            return GenerateHashCode(GenerateHashCode(o1, o2), o3);
        }

    }
}
