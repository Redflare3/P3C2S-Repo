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
            return new List<T>(); // Return an empty list if the file does not exist
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

    public void Remove(T item)
    {
        var items = GetAll();
        items.Remove(item);
        var json = System.Text.Json.JsonSerializer.Serialize(items);
        System.IO.File.WriteAllText(filePath, json);
    }

    public void Update(List<T> items)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(items);
        System.IO.File.WriteAllText(filePath, json);
    }

    public void Clear()
    {
        System.IO.File.WriteAllText(filePath, "[]");
    }

    public void Save(List<T> items)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(items);
        System.IO.File.WriteAllText(filePath, json);
    }

}