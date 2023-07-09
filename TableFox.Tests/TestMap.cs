using TableFox.Test.ClassTests;
using System.Data;
using TableFox.Kernel;
using FluentAssertions;
using TableFox.Kernel.Extensions;

namespace TableFox.Tests
{
    public class TestMap
    {
        private DataTable DataTable { get; set; }
        public TestMap()
        {
            DataTable = new DataTable();

            DataTable.AddRow(
                Field.Of("UserId", 1),
                Field.Of("UserName", "Alex"),
                Field.Of("BusinessProductId", 1),
                Field.Of("BusinessProductName", "Nvidia"),
                Field.Of("BusinessUserId", 1),
                Field.Of("BusinessUserName", "BusinessNet"),
                Field.Of("ProductId", 1),
                Field.Of("ProductName", "3080"),
                Field.Of("ComponentId", 1))
            .AddRow(
                Field.Of("UserId", 1),
                Field.Of("UserName", "Alex"),
                Field.Of("BusinessProductId", 1),
                Field.Of("BusinessProductName", "Nvidia"),
                Field.Of("BusinessUserId", 1),
                Field.Of("BusinessUserName", "BusinessNet"),
                Field.Of("ProductId", 2),
                Field.Of("ProductName", "4080"),
                Field.Of("ComponentId", 2))
            .AddRow(
                Field.Of("UserId", 2),
                Field.Of("UserName", "gituser"),
                Field.Of("BusinessProductId", 1),
                Field.Of("BusinessProductName", 1),
                Field.Of("BusinessUserId", 1),
                Field.Of("BusinessUserName", 1),
                Field.Of("ProductId", 2),
                Field.Of("ProductName", "Prueba2"),
                Field.Of("ComponentId", 2))
            .AddRow(
                Field.Of("UserId", 1),
                Field.Of("UserName", 1),
                Field.Of("BusinessProductId", 1),
                Field.Of("BusinessProductName", 1),
                Field.Of("BusinessUserId", 1),
                Field.Of("BusinessUserName", 1),
                Field.Of("ProductId", 3),
                Field.Of("ProductName", "Prueba3"),
                Field.Of("ComponentId", 33333));
        }

        [Fact]
        public void IsNotNullCollection()
        {
            var result = DataTable.Map<User>();
            result.Should().NotBeNull();
        }

        [Fact]
        public void HaveItems()
        {
            var result = DataTable.Map<User>();
            result.Should().NotHaveCount(0);
        }

        [Fact]
        public void IsNotNull()
        {
            var result = DataTable.Map<User>();
            result.FirstOrDefault().Should().NotBeNull();
        }

        [Fact]
        public void IntegrityBasicClass()
        {
            var result = DataTable.Map<User>();
            result.FirstOrDefault().UserName.Should().Be("Alex");
        }

        [Fact]
        public void IntegrityComplexClass()
        {
            var result = DataTable.Map<User>();
            result.FirstOrDefault().Business.BusinessUserName.Should().Be("BusinessNet");
        }

        [Fact]
        public void IntegrityCollectionClass()
        {
            var result = DataTable.Map<User>();
            result.FirstOrDefault().Products.FirstOrDefault().ProductName.Should().Be("3080");
        }

        [Fact]
        public void IntegrityNestedCollectionClass()
        {
            var result = DataTable.Map<User>();
            result.FirstOrDefault().Products.FirstOrDefault().Components.FirstOrDefault().ComponentId.Should().Be(1);
        }
    }
}