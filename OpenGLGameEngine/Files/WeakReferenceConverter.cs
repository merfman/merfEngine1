using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenGLGameEngine.Assets;

namespace OpenGLGameEngine.Files;
// This class was mostly written by ChatGPT

internal class WeakReferenceConverter<T> : JsonConverter<WeakReference<T>> where T : class
{
    public override WeakReference<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? reference = reader.GetString();
        if (string.IsNullOrEmpty(reference)) return new WeakReference<T>(null);

        T? asset = GameResourceManager.LoadFromFile<T>(reference);
        return new WeakReference<T>(asset);
    }

    public override void Write(Utf8JsonWriter writer, WeakReference<T> value, JsonSerializerOptions options)
    {
        if (value.TryGetTarget(out T? target) && target is Asset asset)
        {
            writer.WriteStringValue(asset.Path); // Serialize as file path
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
