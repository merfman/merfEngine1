using OpenGLGameEngine.Assets;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.InteropServices.Marshalling;

namespace OpenGLGameEngine.Files;
/// <summary>
/// Covers saving, loading, holding and management of loaded data
/// </summary>
// This class is named and partially developed along-side ChatGPT
public class GameResourceManager
{
    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string,BaseObject> _LoadedDynamicObjects;

    public Dictionary<string, Asset> _LoadedAssets;


    public void SaveToFile(object obj, string path)
    {
        string fileExtension = Path.GetExtension(path);
        switch(fileExtension)
        {
            case ".json":
                saveToJson(obj, path);
                break;
            default:
                throw new NotImplementedException($"Error: Cannot Save, Unknown File extension \"{fileExtension}\"");
        }
    }
    private void saveToJson(object obj, string path)
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true, // Pretty-print JSON for readability
            IncludeFields = true, // Ensures all fields are included
            Converters =
            {
                new WeakReferenceConverter<Shader>(), // Auto-handle Shader weak refs
                new WeakReferenceConverter<Texture>() // Auto-handle Texture weak refs
            }
        };
        string jsonSerial = JsonSerializer.Serialize(obj, options);
        Console.WriteLine("jsonSerial:");
        Console.WriteLine(jsonSerial);
        File.WriteAllText(path, jsonSerial);
    }

    //public T LoadAsset<T>(string path) where T : Asset
    //{
        
    //}

}
