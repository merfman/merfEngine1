using OpenGLGameEngine.Assets;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.InteropServices.Marshalling;
using System.Reflection;

namespace OpenGLGameEngine.Files;
/// <summary>
/// Covers saving, loading, holding and management of loaded data
/// </summary>
// This class is named and partially developed along-side ChatGPT
public class GameResourceManager
{
    /// <summary>
    /// A Dictionary of all Loaded BaseObjects.
    /// </summary>
    public static Dictionary<string,BaseObject> _LoadedObjects = new Dictionary<string, BaseObject>();
    public static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,                       // 🔁 Enables polymorphism
        PreserveReferencesHandling = PreserveReferencesHandling.All,    // 🔁 Shared & circular references
        Formatting = Formatting.Indented,                               // Pretty print
        Converters = { new WeakReferenceConverter<Object>() },          // Enables WeakReferences
        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        }
    };

    public static T? Load<T>(string path) where T : BaseObject
    {
        if (_LoadedObjects.TryGetValue(path, out BaseObject? existing))
            return existing == null ? null : (T)existing;

        var json = File.ReadAllText(path);
        var obj = JsonConvert.DeserializeObject<T>(json, settings);

        _LoadedObjects[path] = obj;
        return obj;
    }

    public static void Save(BaseObject obj, string path)
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
    private static void saveToJson(BaseObject obj, string path)
    {
        string jsonSerial = JsonConvert.SerializeObject(obj, settings);
        File.WriteAllText(PathUtil.GetRelative(path), jsonSerial);
        Console.WriteLine("jsonSerial:");
        Console.WriteLine(jsonSerial);
    }

    //public static T? LoadFromFile<T>(string path)
    //{
    //    path = (path);
    //    return JsonSerializer.Deserialize<T>(path);
    //}


}
