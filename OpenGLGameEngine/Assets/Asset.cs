using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Assets;

//[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")] // Enables polymorphic serialization
//[JsonDerivedType(typeof(Material), typeDiscriminator: "derived")]
public abstract class Asset : BaseObject
{
    public string Path { get; internal set; }
    public bool IsLoaded { get; internal set; }

    public virtual void LoadFromFile(string path)
    {
        Console.WriteLine($"Loading File {path}");
    }
}
