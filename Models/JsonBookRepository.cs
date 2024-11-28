using lab1.Interfaces;
using lab1.Models;

namespace lab1.Models
{
    public class JsonBookRepository : IBookCatalog
    {
        private const string FilePath = "books.json";
        private readonly List<ConcreteBook> _books;
        private readonly ISerializer _serializer;

        public JsonBookRepository(ISerializer serializer)
        {
            _serializer = serializer;
            _books = LoadBooks().Result ?? new List<ConcreteBook>();
        }

        // Асинхронная загрузка списка книг из файла
        private async Task<List<ConcreteBook>> LoadBooks()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    var books = await _serializer.DeserializeAsync<List<ConcreteBook>>(FilePath);
                    return books ?? new List<ConcreteBook>(); // Если десериализация не удалась, возвращаем пустой список
                }
                catch (Exception ex)
                {
                    // Логируем ошибку
                    Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                    return new List<ConcreteBook>();
                }
            }
            else
            {
                return new List<ConcreteBook>(); // Если файл не существует, возвращаем пустой список
            }
        }

        // Асинхронное добавление книги в каталог и сериализация в файл
        public async Task AddBookAsync(ConcreteBook book)
        {
            _books.Add(book);
            await SaveBooks();
        }

        // Сохранение книг в файл
        private async Task SaveBooks()
        {
            try
            {
                await _serializer.SerializeAsync(_books, FilePath);
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        // Поиск книг по названию
        public async Task<IEnumerable<IBook>> FindByTitleAsync(string titleFragment)
        {
            return await Task.FromResult(_books.Where(b => b.Title.Contains(titleFragment, StringComparison.OrdinalIgnoreCase)));
        }

        // Поиск книг по автору
        public async Task<IEnumerable<IBook>> FindByAuthorAsync(string authorName)
        {
            return await Task.FromResult(_books.Where(b => b.Author.Contains(authorName, StringComparison.OrdinalIgnoreCase)));
        }

        // Поиск книги по ISBN
        public async Task<IBook?> FindByISBNAsync(string isbn)
        {
            return await Task.FromResult(_books.FirstOrDefault(b => b.ISBN == isbn));
        }

        // Поиск книг по ключевым словам
        public async Task<IEnumerable<(IBook Book, List<string> KeywordsFound)>> FindByKeywordsAsync(string[] keywords)
        {
            var results = new List<(IBook, List<string>)>();

            foreach (var book in _books)
            {
                var keywordsFound = keywords.Where(k => book.ContainsKeyword(k)).ToList();
                if (keywordsFound.Any())
                {
                    results.Add((book, keywordsFound));
                }
            }

            // Теперь сортируем список результатов
            var sortedResults = results.OrderByDescending(r => r.Item2.Count).ToList();

            return await Task.FromResult(sortedResults);
        }
    }
}
