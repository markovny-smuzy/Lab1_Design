using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using lab1.Models;

namespace lab1.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConcreteBookTests
    {
        private ConcreteBook _book;

        [SetUp]
        public void Setup()
        {
            _book = new ConcreteBook("Test Title", "Test Author", new[] { "Fiction" }, 2023, "Test Annotation", "1234567890");
        }

        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            Assert.That(_book.Title, Is.EqualTo("Test Title"));
            Assert.That(_book.Author, Is.EqualTo("Test Author"));
            Assert.That(_book.Genres, Is.EqualTo(new[] { "Fiction" }));
            Assert.That(_book.PublicationYear, Is.EqualTo(2023));
            Assert.That(_book.Annotation, Is.EqualTo("Test Annotation"));
            Assert.That(_book.ISBN, Is.EqualTo("1234567890"));
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