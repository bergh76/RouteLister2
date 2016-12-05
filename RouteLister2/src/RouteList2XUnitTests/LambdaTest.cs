using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace RouteList2XUnitTests
{
    public class LambdaTest
    {
        [Fact]
        public void TestLambda()
        {
            //Arrange
            List<TestClass> collection = new List<TestClass>() {
                new TestClass() { TestOne = 2 },
                new TestClass() { TestTwo="3"},
                new TestClass() { TestThree=4}
            };
            IQueryable<TestClass> testar = collection.Where(x => x.TestOne == 2).AsQueryable();
            //Act
            var expression = Test<TestClass>("3", "TestTwo");
            //collection.Where()
            //var result = collection.Where(Test<TestClass>("3", "TestTwo"));
            //Assert
            Assert.True(true);
        }

        private class TestClass
        {
            public int TestOne { get; set; }
            public string TestTwo { get; set; }
            public double TestThree { get; set; }
        }
        private Expression<Func<T, bool>> Test<T>(object o, string PropertyName)
        {
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "entity");

            var lambda = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Property(parameter, PropertyName),
                        Expression.Constant(o)
                    )
                , parameter);
            return lambda;
        }
    }
}
