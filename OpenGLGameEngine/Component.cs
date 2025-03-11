using OpenGLGameEngine.Mathmatics;
using OpenGLGameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
/// <summary>
/// Can Be added to any <see cref="GameObject"/> to do different things.
/// </summary>
/// <seealso cref="ScriptingComponent"/>,
/// <seealso cref="RenderComponent"/>,
/// <seealso cref="CameraComponent"/>
public abstract class Component : BaseObject
{
    /// <summary>
    /// A reference to the parent <see cref="GameObject"/>.
    /// </summary>
    [JsonIgnore] // Prevents circular serialization. 
    public GameObject GameObject { get; set; }
    public string ComponentType { get; set; }

    public Component(GameObject gameObject)
    {
        GameObject = gameObject;
        ComponentType = this.GetType().Name;
        Console.WriteLine($"new {ComponentType} created under {GameObject.Name}");
    }

}
