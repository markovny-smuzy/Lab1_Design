using lab1.Interfaces;
using lab1.Models;

namespace lab1.Services
{
    public class Menu : IMenu
    {
        private readonly IUserInput _userInput;
        private readonly IUserOutput _userOutput;
        private readonly IBookCatalog _bookCatalog;

        private readonly Dictionary<int, Func<Task>> _menuActions;

        public Menu(IUserInput userInput, IUserOutput userOutput, IBookCatalog bookCatalog)
        {
            _userInput = userInput;
            _userOutput = userOutput;
            _bookCatalog = bookCatalog;

            _menuActions = new Dictionary<int, Func<Task>>
            {
                { 1, AddBookAsync },
                { 2, FindByTitleAsync },
                { 3, FindByAuthorAsync },
                { 4, FindByISBNAsync },
                { 5, FindByKeywordsAsync },
                { 6, ExitAsync }
            };
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear(); // Очистка консоли перед каждым отображением меню
                DisplayMenu();
                var choice = await GetUserChoiceAsync();
                if (_menuActions.ContainsKey(choice))
                {
                    await _menuActions[choice]();
                }
                else
                {
                    _userOutput.WriteOutput("Неверный выбор. Пожалуйста, попробуйте снова.");
                }
            }
        }

        private void DisplayMenu()
        {
            _userOutput.WriteOutput("Добро пожаловать в каталог книг! Выберите действие, введя номер соответствующего пункта:");
            _userOutput.WriteOutput("1. Добавить книгу в каталог");
            _userOutput.WriteOutput("2. Найти книгу по названию");
            _userOutput.WriteOutput("3. Найти книгу по имени автора");
            _userOutput.WriteOutput("4. Найти книгу по ISBN");
            _userOutput.WriteOutput("5. Найти книги по ключевым словам");
            _userOutput.WriteOutput("6. Выйти");
        }

        private async Task<int> GetUserChoiceAsync()
        {
            _userOutput.WriteOutput("Ваш выбор:");
            var input = _userInput.ReadInput();
            int.TryParse(input, out int choice);
            return choice;
        }

        private async Task AddBookAsync()
        {
            Console.Clear();
            _userOutput.WriteOutput("Введите название книги:");
            var title = _userInput.ReadInput();

            _userOutput.WriteOutput("Введите имя автора:");
            var author = _userInput.ReadInput();

            _userOutput.WriteOutput("Введите жанры (через запятую):");
            var genres = _userInput.ReadInput().Split(',');

            _userOutput.WriteOutput("Введите год публикации книги:");
            var year = int.Parse(_userInput.ReadInput());

            _userOutput.WriteOutput("Введите аннотацию книги:");
            var annotation = _userInput.ReadInput();

            _userOutput.WriteOutput("Введите ISBN книги:");
            var isbn = _userInput.ReadInput();

            var book = new ConcreteBook(title, author, genres, year, annotation, isbn);
            await _bookCatalog.AddBookAsync(book);

            _userOutput.WriteOutput("Книга добавлена в каталог.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private async Task FindByTitleAsync()
        {
            Console.Clear();
            _userOutput.WriteOutput("Введите название книги:");
            var titleFragment = _userInput.ReadInput();

            var books = await _bookCatalog.FindByTitleAsync(titleFragment);
            foreach (var book in books)
            {
                _userOutput.WriteOutput($"Название: {book.Title}, Автор: {book.Author}");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private async Task FindByAuthorAsync()
        {
            Console.Clear();
            _userOutput.WriteOutput("Введите имя автора:");
            var authorName = _userInput.ReadInput();

            var books = await _bookCatalog.FindByAuthorAsync(authorName);
            foreach (var book in books)
            {
                _userOutput.WriteOutput($"Название: {book.Title}, Автор: {book.Author}");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private async Task FindByISBNAsync()
        {
            Console.Clear();
            _userOutput.WriteOutput("Введите ISBN книги:");
            var isbn = _userInput.ReadInput();

            var book = await _bookCatalog.FindByISBNAsync(isbn);
            if (book != null)
            {
                _userOutput.WriteOutput($"Название: {book.Title}, Автор: {book.Author}");
            }
            else
            {
                _userOutput.WriteOutput("Книга не найдена.");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private async Task FindByKeywordsAsync()
        {
            Console.Clear();
            _userOutput.WriteOutput("Введите ключевые слова (через запятую):");
            var keywords = _userInput.ReadInput().Split(',');

            var results = await _bookCatalog.FindByKeywordsAsync(keywords);
            foreach (var (book, foundKeywords) in results)
            {
                _userOutput.WriteOutput($"Название: {book.Title}, Найдено ключевых слов: {string.Join(", ", foundKeywords)}");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private async Task ExitAsync()
        {
            _userOutput.WriteOutput("Выход из программы. До свидания!");
            Environment.Exit(0); // Завершить программу
        }
    }
}
