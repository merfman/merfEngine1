using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
/// <summary>
/// The Base of every Engine object
/// </summary>
public class BaseObject
{
    /// <summary>
    /// The name of the Base Object. if not null displays for <seealso cref="ToString"/>.
    /// </summary>
    public string Name;

    /// <summary>
    /// The file path of the BaseObject.
    /// </summary>
    public string Path { get; internal set; }
    public bool IsLoaded { get; internal set; }
    public int RefrenceCounter { get; set; }

    public override string ToString() => Name == null ? base.ToString() : Name;
    public BaseObject(string? name = null)
    {
        if (name != null)
            Name = name;
    }
}
