namespace lab1.Interfaces;

public interface IBook
{
    string Title { get; }
    string Author { get; }
    string[] Genres { get; }
    int PublicationYear { get; }
    string Annotation { get; }
    string ISBN { get; }
    bool ContainsKeyword(string keyword);
}