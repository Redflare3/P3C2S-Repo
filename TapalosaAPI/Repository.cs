using System.Collections.Generic;
using System.Runtime.CompilerServices;
public class Repository<T>
{
    private string filePath;

    public Repository(string filePath)
    {
        this.filePath = filePath;
    }

    public List<T> GetAll()
    {
        if (!System.IO.File.Exists(filePath))
        {
            return new List<T>();
        }

        var json = System.IO.File.ReadAllText(filePath);
        return System.Text.Json.JsonSerializer.Deserialize<List<T>>(json);
    }

    public void Add(T item)
    {
        var items = GetAll();
        items.Add(item);
        var json = System.Text.Json.JsonSerializer.Serialize(items);
        System.IO.File.WriteAllText(filePath, json);
    }

    public void Save(List<T> items)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(items);
        System.IO.File.WriteAllText(filePath, json);
    }

}