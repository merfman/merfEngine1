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
    public string Path { get; private set; }
    public bool IsLoaded { get; private set; }

    public virtual void LoadFromFile(string path)
    {
        Console.WriteLine($"Loading File {path}");
    }
}
