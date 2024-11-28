using lab1.Interfaces;

namespace lab1.Services;

public class ConsoleUserInput : IUserInput
{
    public string ReadInput() => Console.ReadLine() ?? string.Empty;

}