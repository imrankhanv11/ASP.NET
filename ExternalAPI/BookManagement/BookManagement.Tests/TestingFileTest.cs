using BookManagement.ServiceLayer;
using BookManagement.ServiceLayer.Interfaces;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.Tests.TestData;

namespace BookManagement.Tests
{
    public class TestingFileTest
    {
        private readonly Mock<ITestingService> _mockService;
        private readonly TestingFile _service;

        public TestingFileTest()
        {
            _mockService = new Mock<ITestingService>();
            _service = new TestingFile(_mockService.Object);
        }

        // Theroy
        [Theory]
        [InlineData(2,3,5,false)]
        [InlineData(0,0,0,false)]
        [InlineData(1,1,2,true)]
        [InlineData(-1, -1, -2, true)]  
        [InlineData(5, 6, 11, false)]
        [InlineData(-5, 2, -3, false)]
        public void EvenOfAdd_ShouldReturnTrue_WhenInputIsEven(int a, int b, int expected, bool output)
        {

            _mockService.Setup(s => s.AddNumbers(a, b)).Returns(expected);

            // Act
            var result = _service.EvenOfAdd(a, b);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(output);
                _mockService.Verify(s => s.AddNumbers(a, b), Times.Once);
            }
        }

        //-------------------------------------------------------------------------------------------------

        // MemberData (listof Object)
        public static IEnumerable<object[]> EvenOfAddData => new List<object[]>
        {
            new object[] { 1, 2, 3, false },
            new object[] { 0, 2, 2, true },
            new object[] {-1,+1, 0, false}
        };

        [Theory]
        [MemberData(nameof(EvenOfAddData))]
        public void EvenOfAdd_ShouldReturnTrue_WhenInputIsEven222(int a, int b, int expected, bool output)
        {

            _mockService.Setup(s => s.AddNumbers(a, b)).Returns(expected);

            // Act
            var result = _service.EvenOfAdd(a, b);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(output);
                _mockService.Verify(s => s.AddNumbers(a, b), Times.Once);
            }
        }
        //-----------------------------------------------------------------------------------------------------

        // memberdata (method)
        public static IEnumerable<object[]> GetConcatData()
        {
            yield return new object[] { "Hello", "World", "HelloWorld" };
            yield return new object[] { "Test", "123", "Test123" };
            yield return new object[] { "A", "B", "AB" };
        }

        [Theory]
        [MemberData(nameof(GetConcatData))]
        public void Concat_ShouldCombineStrings(string first, string second, string expected)
        {
            var result = _service.concatString(first, second);

            result.Should().Be(expected);
        }
        //---------------------------------------------------------------------------------------

        // memeberData (from another Class)
        [Theory]
        [MemberData(nameof(EvenOfAddDataClass.EvenOfAddDataClassObj), MemberType = typeof(EvenOfAddDataClass))]
        public void EvenOfAdd_ShouldReturnTrue_WhenInputIsEven222333(int a, int b, int expected, bool output)
        {

            _mockService.Setup(s => s.AddNumbers(a, b)).Returns(expected);

            // Act
            var result = _service.EvenOfAdd(a, b);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(output);
                _mockService.Verify(s => s.AddNumbers(a, b), Times.Once);
            }
        }
    }
}
