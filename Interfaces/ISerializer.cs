namespace lab1.Interfaces;

public interface ISerializer
{
    Task SerializeAsync<T>(T data, string filePath);
    Task<T?> DeserializeAsync<T>(string filePath);
}