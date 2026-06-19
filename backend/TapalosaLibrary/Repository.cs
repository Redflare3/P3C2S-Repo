using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TapalosaCommon
{
    public sealed class Repository<T>
    {
        private static readonly Dictionary<string, Repository<T>> _instances = new Dictionary<string, Repository<T>>(StringComparer.OrdinalIgnoreCase);
        private readonly string _filePath;

        private Repository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Path database tidak boleh kosong", nameof(filePath));
            }
            _filePath = ResolvePath(filePath);
        }

        public static Repository<T> GetInstance(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Path database tidak boleh kosong", nameof(filePath));
            }

            string resolvedPath = ResolvePath(filePath);
            lock (_instances)
            {
                if (!_instances.TryGetValue(resolvedPath, out Repository<T>? instance))
                {
                    instance = new Repository<T>(resolvedPath);
                    _instances[resolvedPath] = instance;
                }
                return instance;
            }
        }

        private static string ResolvePath(string relativePath)
        {
            if (Path.IsPathRooted(relativePath)) return relativePath;

            string baseDir = AppContext.BaseDirectory;
            string combined = Path.Combine(baseDir, relativePath);

            string? directory = Path.GetDirectoryName(combined);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return combined;
        }

        public List<T> GetAll()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<T>();
                }

                var json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<T>();
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading repository file {_filePath}: {ex.Message}");
                return new List<T>();
            }
        }

        public void Add(T item)
        {
            var items = GetAll();
            items.Add(item);
            Save(items);
        }

        public void Save(List<T> items)
        {
            try
            {
                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(items, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing repository file {_filePath}: {ex.Message}");
            }
        }
    }
}