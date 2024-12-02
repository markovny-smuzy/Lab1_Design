using System;
using System.Diagnostics.CodeAnalysis;
using lab1.Interfaces;
using lab1.Models;
using lab1.Services;
using Moq;

namespace lab1.Tests.Services
{
    using NUnit.Framework;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    public class MenuTests
    {
        private Mock<IUserInput> _userInputMock;
        private Mock<IUserOutput> _userOutputMock;
        private Mock<IBookCatalog> _bookCatalogMock;
        private Menu _menu;

        [SetUp]
        public void SetUp()
        {
            _userInputMock = new Mock<IUserInput>();
            _userOutputMock = new Mock<IUserOutput>();
            _bookCatalogMock = new Mock<IBookCatalog>();
            _menu = new Menu(_userInputMock.Object, _userOutputMock.Object, _bookCatalogMock.Object);
        }

        [Test]
        public async Task AddBookAsync_ShouldAddBookAndNotifyUser()
        {
            // Arrange
            var title = "Тестовая книга";
            var author = "Тестовый автор";
            var genres = new[] { "Фантастика" };
            var year = 2023;
            var annotation = "Тестовая аннотация";
            var isbn = "123-4567890123";

            _userInputMock.SetupSequence(ui => ui.ReadInput())
                .Returns(title)
                .Returns(author)
                .Returns(string.Join(",", genres))
                .Returns(year.ToString())
                .Returns(annotation)
                .Returns(isbn);

            _bookCatalogMock.Setup(bc => bc.AddBookAsync(It.IsAny<ConcreteBook>())).Returns(Task.CompletedTask);

            // Act
            await _menu.AddBookAsync();

            // Assert
            _bookCatalogMock.Verify(bc => bc.AddBookAsync(It.Is<ConcreteBook>(b =>
                    b.Title == title &&
                    b.Author == author &&
                    b.Genres.SequenceEqual(genres) &&
                    b.PublicationYear == year &&
                    b.Annotation == annotation &&
                    b.ISBN == isbn)),
                Times.Once);

            _userOutputMock.Verify(ou => ou.WriteOutput("Книга добавлена в каталог."), Times.Once);
        }

        [Test]
        public async Task FindByTitleAsync_ShouldDisplayFoundBooks()
        {
            // Arrange
            var titleFragment = "Тест";
            var books = new List<ConcreteBook>
            {
                new ConcreteBook("Тестовая книга", "Тестовый автор", new[] { "Фантастика" }, 2023, "Тестовая аннотация", "123-4567890123")
            };
        
            _userInputMock.Setup(ui => ui.ReadInput()).Returns(titleFragment);
            _bookCatalogMock.Setup(bc => bc.FindByTitleAsync(titleFragment)).ReturnsAsync(books);
        
            // Act
            await _menu.FindByTitleAsync();
        
            // Assert
            _userOutputMock.Verify(ou => ou.WriteOutput(It.IsAny<string>()), Times.Exactly(2)); // Проверяем, что было 2 вызова
        }
    }
}
