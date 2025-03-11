using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OpenGLGameEngine.Assets;

namespace OpenGLGameEngine.Files;
// this file was mostly written by ChatGPT
/// <summary>
/// 
/// </summary>
public class JsonSaveLoad
{
    private static readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true, // Pretty-print JSON for readability
        IncludeFields = true, // Ensures all fields are included
        Converters =
        {
            new WeakReferenceConverter<Shader>(), // Auto-handle Shader weak refs
            new WeakReferenceConverter<Texture>() // Auto-handle Texture weak refs
        }
    };
    
    // Generic save method
    public static void Save<T>(T obj, string filePath)
    {
        string json = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, json);
    }

    // Generic load method
    public static T? Load<T>(string filePath) where T : class
    {
        if (!File.Exists(filePath))
            return null;

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, options);
    }
}
