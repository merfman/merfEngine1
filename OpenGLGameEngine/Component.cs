using OpenGLGameEngine.Mathmatics;
using OpenGLGameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public GameObject GameObject { get; set; }
}
