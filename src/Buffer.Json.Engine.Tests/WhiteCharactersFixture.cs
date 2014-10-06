using Moq;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace Buffer.Json.Engine.Tests
{
    [TestFixture]
    public class WhiteCharactersFixture
    {
        private IParser parser;
        private IHandler handler;

        [SetUp]
        public void SetUp()
        {
            this.parser = new Parser(64);
            this.handler = new Mock<IHandler>().Object;
        }

        [Test]
        public void CanHandleWhiteCharactersBeforeArray()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("  []"));

            // act
            this.parser.Parse(input, this.handler);
        }

        [Test]
        public void CanHandleWhiteCharactersInsideEmptyArray()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[  ]"));

            // act
            this.parser.Parse(input, this.handler);
        }

        [Test]
        public void CanHandleWhiteCharactersBeforeFirstArrayItem()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[  1,2]"));

            // act
            this.parser.Parse(input, this.handler);
        }

        [Test]
        public void CanHandleWhiteCharactersBeforeSecondArrayItem()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[1,  2]"));

            // act
            this.parser.Parse(input, this.handler);
        }

        [Test]
        public void CanHandleWhiteCharactersAfterFirstArrayItem()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[1  ,2]"));

            // act
            this.parser.Parse(input, this.handler);
        }

        [Test]
        public void CanHandleWhiteCharactersAfterSecondArrayItem()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[1,2  ]"));

            // act
            this.parser.Parse(input, this.handler);
        }

        [Test]
        public void CanHandleWhiteCharactersAfterArray()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[]  "));

            // act
            this.parser.Parse(input, this.handler);
        }
    }
}
