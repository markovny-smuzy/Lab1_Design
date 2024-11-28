namespace lab1.Models;

public class ConcreteBook : Book
{
    public ConcreteBook(string title, string author, string[] genres, int publicationYear, string annotation, string isbn) : base(title, author, genres, publicationYear, annotation, isbn)
    {
    }
}