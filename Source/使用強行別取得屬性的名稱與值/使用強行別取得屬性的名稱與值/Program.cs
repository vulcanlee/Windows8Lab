using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace 使用強行別取得屬性的名稱與值
{
    class Program
    {
        static void Main(string[] args)
        {
            fooClass fooclass = new fooClass()
            {
                 MyProperty1 = "123",
                  MyProperty2 = 4565,
            };
            Console.WriteLine(StaticReflection.GetMemberName<fooClass>(x=>x.MyProperty1));
            PrintProperty(() => fooclass.MyProperty1);
            PrintProperty(() => fooclass.MyProperty2);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.AddProperty(() => fooclass.MyProperty1);
            dic.AddProperty(() => fooclass.MyProperty2);
            foreach (var item in dic)
            {
                Console.WriteLine("Dic Item {0} : {1}", item.Key, item.Value);
            }
            Console.Read();
        }

        public static void PrintProperty<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            string propertyName = member.Member.Name;
            T value = e.Compile()();
            Console.WriteLine("{0} : {1}", propertyName, value);
        }

    }

    public static class DirectionExt
    {
        public static void AddProperty<T>(this Dictionary<string, string> dictionary, Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;

            dictionary.Add(member.Member.Name, e.Compile()().ToString());
        }

    }
    public class fooClass
    {
        public string MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
    }

    public static class StaticReflection
    {
        public static string GetMemberName<T>(
            this T instance,
            Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression);
        }

        public static string GetMemberName<T>(
            Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        public static string GetMemberName<T>(
            this T instance,
            Expression<Action<T>> expression)
        {
            return GetMemberName(expression);
        }

        public static string GetMemberName<T>(
            Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        private static string GetMemberName(
            Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression =
                    (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression =
                    (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string GetMemberName(
            UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression =
                    (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand)
                .Member.Name;
        }
    }
}
