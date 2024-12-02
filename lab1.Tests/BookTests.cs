using lab1.Models;
using System.Diagnostics.CodeAnalysis;

namespace lab1.Tests
{
    using NUnit.Framework;
    
    [ExcludeFromCodeCoverage]
    public class BookTests
    {
        private ConcreteBook _book;

        [SetUp]
        public void Setup()
        {
            _book = new ConcreteBook("Test Title", "Test Author", new[] { "Fiction" }, 2023, "Test Annotation", "1234567890");
        }

        [Test]
        public void ContainsKeyword_ShouldReturnTrue_WhenKeywordIsInTitle()
        {
            var result = _book.ContainsKeyword("Test Title");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ContainsKeyword_ShouldReturnTrue_WhenKeywordIsInAuthor()
        {
            var result = _book.ContainsKeyword("Test Author");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ContainsKeyword_ShouldReturnTrue_WhenKeywordIsInAnnotation()
        {
            var result = _book.ContainsKeyword("Test Annotation");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ContainsKeyword_ShouldReturnFalse_WhenKeywordIsNotInBook()
        {
            var result = _book.ContainsKeyword("Nonexistent Keyword");
            Assert.That(result, Is.False);
        }

        [Test]
        public void ContainsKeyword_ShouldBeCaseInsensitive()
        {
            var result = _book.ContainsKeyword("test title");
            Assert.That(result, Is.True);
        }
    }
}