using lab1.Models;
using lab1.Services;

namespace lab1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var userInput = new ConsoleUserInput();
            var userOutput = new ConsoleUserOutput();
            var serializer = new JsonSerializer();
            var bookCatalog = new JsonBookRepository(serializer);
            var menu = new Menu(userInput, userOutput, bookCatalog);
            await menu.ShowAsync();
        }
    }
}