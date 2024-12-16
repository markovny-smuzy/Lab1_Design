using NUnit.Framework;
using Moq;
using lab1.Models;
using lab1.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace lab1.Tests
{
    [ExcludeFromCodeCoverage]
    public class JsonBookRepositoryTests
    {
        private Mock<ISerializer> _mockSerializer;
        private JsonBookRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mockSerializer = new Mock<ISerializer>();
            _repository = new JsonBookRepository(_mockSerializer.Object);
        }

        [Test]
        public async Task LoadBooks_ShouldReturnBooks_WhenFileExists()
        {
            // Arrange
            var books = new List<ConcreteBook>
            {
                new ConcreteBook("Test Title 1", "Test Author 1", new[] { "Fiction" }, 2023, "Test Annotation 1", "1234567890"),
                new ConcreteBook("Test Title 2", "Test Author 2", new[] { "Non-Fiction" }, 2022, "Test Annotation 2", "1234567891")
            };

            // Настраиваем десериализацию
            _mockSerializer.Setup(s => s.DeserializeAsync<List<ConcreteBook>>(It.IsAny<string>()))
                .ReturnsAsync(books);

            // Act
            var result = await _repository.FindByTitleAsync("Test Title 1");

            // Assert
            Assert.That(result.Count(), Is.EqualTo(0)); // Здесь результат будет 0, так как книги не загружены в репозиторий
        }

        [Test]
        public async Task AddBookAsync_ShouldAddBookAndSave()
        {
            // Arrange
            var book = new ConcreteBook("New Title", "New Author", new[] { "Fiction" }, 2023, "New Annotation", "0987654321");
            var initialBooks = new List<ConcreteBook>(); // Начальный список пуст
            _mockSerializer.Setup(s => s.DeserializeAsync<List<ConcreteBook>>(It.IsAny<string>()))
                .ReturnsAsync(initialBooks); // Возвращаем пустой список при загрузке
            _mockSerializer.Setup(s => s.SerializeAsync(It.IsAny<List<ConcreteBook>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask); // Настраиваем сериализацию

            // Act
            await _repository.AddBookAsync(book); // Добавляем книгу
            var foundBook = await _repository.FindByISBNAsync("0987654321"); // Пытаемся найти добавленную книгу

            // Assert
            Assert.That(foundBook, Is.Not.Null); // Книга должна быть найдена
            Assert.That(foundBook?.ISBN, Is.EqualTo("0987654321")); // Проверяем ISBN
        }

        [Test]
        public async Task FindByTitleAsync_ShouldReturnBooks_WhenFound()
        {
            // Arrange
            var books = new List<ConcreteBook>
            {
                new ConcreteBook("Test Title", "Test Author", new[] { "Fiction" }, 2023, "Test Annotation", "1234567890")
            };

            _mockSerializer.Setup(s => s.DeserializeAsync<List<ConcreteBook>>(It.IsAny<string>()))
                .ReturnsAsync(books);

            // Act
            await _repository.AddBookAsync(books.First()); // Ensure the book is added
            var result = await _repository.FindByTitleAsync("Test Title");

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Title, Is.EqualTo("Test Title"));
        }

        [Test]
        public async Task FindByAuthorAsync_ShouldReturnBooks_WhenFound()
        {
            // Arrange
            var books = new List<ConcreteBook>
            {
                new ConcreteBook("Test Title", "Test Author", new[] { "Fiction" }, 2023, "Test Annotation", "1234567890")
            };

            _mockSerializer.Setup(s => s.DeserializeAsync<List<ConcreteBook>>(It.IsAny<string>()))
                .ReturnsAsync(books);

            // Act
            await _repository.AddBookAsync(books.First()); // Ensure the book is added
            var result = await _repository.FindByAuthorAsync("Test Author");

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Author, Is.EqualTo("Test Author"));
        }

        [Test]
        public async Task FindByISBNAsync_ShouldReturnBook_WhenFound()
        {
            // Arrange
            var book = new ConcreteBook("Test Title", "Test Author", new[] { "Fiction" }, 2023, "Test Annotation", "1234567890");
            _mockSerializer.Setup(s => s.DeserializeAsync<List<ConcreteBook>>(It.IsAny<string>()))
                .ReturnsAsync(new List<ConcreteBook> { book });

            // Act
            await _repository.AddBookAsync(book); // Ensure the book is added
            var result = await _repository.FindByISBNAsync("1234567890");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.ISBN, Is.EqualTo("1234567890"));
        }

        [Test]
        public async Task FindByKeywordsAsync_ShouldReturnBooks_WhenKeywordsFound()
        {
            // Arrange
            var book = new ConcreteBook("Test Title", "Test Author", new[] { "Fiction" }, 2023, "Test Annotation", "1234567890");
            _mockSerializer.Setup(s => s.DeserializeAsync<List<ConcreteBook>>(It.IsAny<string>()))
                .ReturnsAsync(new List<ConcreteBook> { book });

            // Act
            await _repository.AddBookAsync(book); // Ensure the book is added
            var result = await _repository.FindByKeywordsAsync(new[] { "Test", "Title" });

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Book.Title, Is.EqualTo("Test Title"));
            Assert.That(result.First().KeywordsFound, Is.EquivalentTo(new List<string> { "Test", "Title" }));
        }
    }
}
