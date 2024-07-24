using BlogManagement.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public class JsonFileService
{
    private readonly string _filePath;

    public JsonFileService(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<List<BlogModel>> ReadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<BlogModel>();
        }

        var json = await File.ReadAllTextAsync(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<BlogModel>();
        }

        return JsonSerializer.Deserialize<List<BlogModel>>(json);
    }

    public async Task WriteAsync(List<BlogModel> blogs)
    {
        var json = JsonSerializer.Serialize(blogs);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<BlogModel> CreateAsync(BlogModel blog)
    {
        var blogs = await ReadAsync();
        blog.BlogId = blogs.Any() ? blogs.Max(b => b.BlogId) + 1 : 1;
        blogs.Add(blog);
        await WriteAsync(blogs);
        return blog;
    }

    public async Task<BlogModel> ReadAsync(int blogId)
    {
        var blogs = await ReadAsync();
        return blogs.FirstOrDefault(b => b.BlogId == blogId);
    }

    public async Task<BlogModel> UpdateAsync(BlogModel blog)
    {
        var blogs = await ReadAsync();
        var index = blogs.FindIndex(b => b.BlogId == blog.BlogId);
        if (index == -1)
        {
            return null;
        }
        blogs[index] = blog;
        await WriteAsync(blogs);
        return blog;
    }

    public async Task<bool> DeleteAsync(int blogId)
    {
        var blogs = await ReadAsync();
        var blog = blogs.FirstOrDefault(b => b.BlogId == blogId);
        if (blog == null)
        {
            return false;
        }
        blogs.Remove(blog);
        await WriteAsync(blogs);
        return true;
    }
}
