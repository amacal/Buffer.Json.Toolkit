using Buffer.Json.Engine.Handlers;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace Buffer.Json.Engine.Tests
{
    [TestFixture]
    public class CompactFixture
    {
        private IParser parser;
        private IHandler handler;
        private MemoryStream output;

        [SetUp]
        public void SetUp()
        {
            this.parser = new Parser(64);
            this.output = new MemoryStream();
            this.handler = new CompactHandler(this.output, 64);
        }

        [Test]
        public void WhenCompactingEmptyArrayThenThereIsNoWhiteCharactersBetweenBrackets()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[]"));

            // act
            this.parser.Parse(input, this.handler);

            // assert
            Assert.That(Encoding.UTF8.GetString(this.output.ToArray()), Is.EqualTo("[]"));
        }

        [Test]
        public void WhenCompactingSingleItemArrayThenThereIsNoWhiteCharacters()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[1]"));

            // act
            this.parser.Parse(input, this.handler);

            // assert
            Assert.That(Encoding.UTF8.GetString(this.output.ToArray()), Is.EqualTo("[1]"));
        }

        [Test]
        public void WhenCompactingTwoItemsArrayThenThereIsNoWhiteCharacterAfterComma()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[1, 2]"));

            // act
            this.parser.Parse(input, this.handler);

            // assert
            Assert.That(Encoding.UTF8.GetString(this.output.ToArray()), Is.EqualTo("[1,2]"));
        }

        [Test]
        public void WhenCompactingThreeItemsArrayThenThereIsNoWhiteCharacterAfterEachComma()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("[1, 2, 3]"));

            // act
            this.parser.Parse(input, this.handler);

            // assert
            Assert.That(Encoding.UTF8.GetString(this.output.ToArray()), Is.EqualTo("[1,2,3]"));
        }

        [Test]
        public void WhenCompactingEmptyObjectThenThereIsNoWhiteCharactersBetweenBrackets()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

            // act
            this.parser.Parse(input, this.handler);

            // assert
            Assert.That(Encoding.UTF8.GetString(this.output.ToArray()), Is.EqualTo("{}"));
        }

        [Test]
        public void WhenCompactingSinglePropertyObjectThenThisPropertyIsInTogetherWithoutAnyWhiteCharacter()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("{\r\n\t\"a\": 1\r\n}"));

            // act
            this.parser.Parse(input, this.handler);

            // assert
            Assert.That(Encoding.UTF8.GetString(this.output.ToArray()), Is.EqualTo("{\"a\":1}"));
        }

        [Test]
        public void WhenCompactingTwoPropertiesObjectThenEachPropertyIsInTogetherWithoutAnyWhiteCharacter()
        {
            // arrange
            Stream input = new MemoryStream(Encoding.UTF8.GetBytes("{\r\n\t\"a\": 1,\r\n\t\"b\": 2\r\n}"));

            // act
            this.parser.Parse(input, this.handler);

            // assert
            Assert.That(Encoding.UTF8.GetString(this.output.ToArray()), Is.EqualTo("{\"a\":1,\"b\":2}"));
        }
    }
}
