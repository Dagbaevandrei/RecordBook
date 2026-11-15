using System.Text.Json;
using System.Xml.Serialization;
using RecordBook.Models;

namespace RecordBook.Services;

public static class XmlService
{
    public static Models.RecordBook LoadFromJson(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Файл не найден: {path}");

        var json = File.ReadAllText(path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };
        return JsonSerializer.Deserialize<Models.RecordBook>(json, options)
               ?? throw new InvalidDataException("Не удалось десериализовать JSON.");
    }

    public static void SaveToXml(Models.RecordBook book, string path)
    {
        var serializer = new XmlSerializer(typeof(Models.RecordBook));
        using var writer = new StreamWriter(path);
        serializer.Serialize(writer, book);
    }
}
