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
                if (!IsRunningTests())
                {
                    Console.Clear();
                }
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
            var input = await Task.Run(() => _userInput.ReadInput());
            int.TryParse(input, out int choice);
            return choice;
        }

        public async Task AddBookAsync()
        {
            try
            {
                _userOutput.WriteOutput("Введите название книги:");
                var title = _userInput.ReadInput();
        
                _userOutput.WriteOutput("Введите автора книги:");
                var author = _userInput.ReadInput();
        
                _userOutput.WriteOutput("Введите жанры книги (через запятую):");
                var genresInput = _userInput.ReadInput();
                var genres = genresInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        
                _userOutput.WriteOutput("Введите год публикации:");
                var year = int.Parse(_userInput.ReadInput());
        
                _userOutput.WriteOutput("Введите аннотацию:");
                var annotation = _userInput.ReadInput();
        
                _userOutput.WriteOutput("Введите ISBN:");
                var isbn = _userInput.ReadInput();
        
                // Console.Clear() вызывается только в реальной среде
                if (!IsRunningTests())
                {
                    Console.Clear();
                }
        
                var book = new ConcreteBook(title, author, genres, year, annotation, isbn);
                await _bookCatalog.AddBookAsync(book);
                _userOutput.WriteOutput("Книга добавлена в каталог.");
            }
            catch (Exception ex)
            {
                _userOutput.WriteOutput($"Произошла ошибка: {ex.Message}");
            }
        
            if (!IsRunningTests())
            {
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private bool IsRunningTests()
        {
            // Логика для определения, запущены ли тесты
            return AppDomain.CurrentDomain.FriendlyName.Contains("Test");
        }

        public async Task FindByTitleAsync()
        {
            if (!IsRunningTests())
            {
                Console.Clear();
            }
            _userOutput.WriteOutput("Введите название книги:");
            var titleFragment = _userInput.ReadInput();

            var books = await _bookCatalog.FindByTitleAsync(titleFragment);
            if (books.Any())
            {
                foreach (var book in books)
                {
                    _userOutput.WriteOutput($"Название: {book.Title}, Автор: {book.Author}");
                }
            }
            else
            {
                _userOutput.WriteOutput("Книги не найдены.");
            }
            
            if (!IsRunningTests())
            {
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private async Task FindByAuthorAsync()
        {
            if (!IsRunningTests())
            {
                Console.Clear();
            }
            _userOutput.WriteOutput("Введите имя автора:");
            var authorName = _userInput.ReadInput();
            var books = await _bookCatalog.FindByAuthorAsync(authorName);
            if (books.Any())
            {
                foreach (var book in books) 
                {
                    _userOutput.WriteOutput($"Название: {book.Title}, Автор: {book.Author}");
                }
            }
            else
            {
                _userOutput.WriteOutput("Книги не найдены.");
            }
            if (!IsRunningTests())
            {
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private async Task FindByISBNAsync()
        {
            if (!IsRunningTests())
            {
                Console.Clear();
            }
            _userOutput.WriteOutput("Введите ISBN книги:");
            var isbn = _userInput.ReadInput();

            var book = await _bookCatalog.FindByISBNAsync(isbn);
            if (book != null)
            {
                _userOutput.WriteOutput($"Название: {book.Title}, Автор: {book.Author}");
            }
            else
            {
                _userOutput.WriteOutput("Книги не найдены.");
            }
            if (!IsRunningTests())
            {
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private async Task FindByKeywordsAsync()
        {
            if (!IsRunningTests())
            {
                Console.Clear();
            }
            _userOutput.WriteOutput("Введите ключевые слова (через запятую):");
            var keywords = _userInput.ReadInput().Split(',');

            var results = await _bookCatalog.FindByKeywordsAsync(keywords);
            if (results.Any())
            {
                foreach (var (book, foundKeywords) in results)
                {
                    _userOutput.WriteOutput($"Название: {book.Title}, Найдено ключевых слов: {string.Join(", ", foundKeywords)}");
                }
            }
            else
            {
                _userOutput.WriteOutput("Книги не найдены.");
            }
            if (!IsRunningTests())
            {
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        public async Task ExitAsync()
        {
            _userOutput.WriteOutput("Выход из программы. До свидания!");
            if (!IsRunningTests())
            {
                await Task.Delay(1000);
                Environment.Exit(0);
            }
            else
            {
                await Task.CompletedTask;
            }
        }
    }
}
