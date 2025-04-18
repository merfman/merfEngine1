using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLGameEngine.Assets;
using Newtonsoft.Json;
using System.Resources;

namespace OpenGLGameEngine.Files;
//This class was mostly written by ChatGPT

internal class WeakReferenceConverter<T> : JsonConverter<WeakReference<T>> where T : BaseObject
{
    public override void WriteJson(JsonWriter writer, WeakReference<T> value, JsonSerializer serializer)
    {
        if (value.TryGetTarget(out T target))
        {
            if (target is BaseObject baseObj && baseObj.Path != null)
            {
                // Save to a different file
                GameResourceManager.Save(baseObj, baseObj.Path);
                // Write a reference to it
                writer.WriteValue(baseObj.Path);
            }
            else 
                serializer.Serialize(writer, target);
        }
        else
        {
            writer.WriteNull();
        }
    }
    public override WeakReference<T> ReadJson(JsonReader reader, Type objectType, WeakReference<T> existingValue, bool hasExistingValue, JsonSerializer serializer) 
    {
        if (reader.TokenType == JsonToken.String)
        {

            string path = (string)reader.Value;
            if (typeof(BaseObject).IsAssignableFrom(objectType))
            {
                BaseObject baseObj = GameResourceManager.Load<BaseObject>(path);    // Load as BaseObject
                return new WeakReference<T>((T)baseObj);                            // Cast to T
            }
            else throw new JsonSerializationException($"Cannot load non-BaseObject type {objectType.Name} from path.");
        }
        else if (reader.TokenType == JsonToken.Null)
        {
            return new WeakReference<T>(null);
        }
        else
        {
            // Optional: fallback if someone inlines the object instead of a path
            T obj = serializer.Deserialize<T>(reader);
            return new WeakReference<T>(obj);
        }
        var target = serializer.Deserialize<T>(reader);
        return new WeakReference<T>(target);

    }

    //public override WeakReference<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //{
    //    string? reference = reader.GetString();
    //    if (string.IsNullOrEmpty(reference)) return new WeakReference<T>(null);

    //    T? asset = GameResourceManager.Load<T>(reference);
    //    return new WeakReference<T>(asset);
    //}

    //public override void Write(Utf8JsonWriter writer, WeakReference<T> value, JsonSerializerOptions options)
    //{
    //    if (value.TryGetTarget(out T? target) && target is Asset asset)
    //    {
    //        writer.WriteStringValue(asset.Path); // Serialize as file path
    //    }
    //    else
    //    {
    //        writer.WriteNullValue();
    //    }
    //}
}
