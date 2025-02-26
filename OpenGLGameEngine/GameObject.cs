using OpenGLGameEngine.Mathmatics;
using OpenGLGameEngine.Assets;
using OpenGLGameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
/// <summary>
/// Represents an versatile object that can have many purposes.
/// </summary>
public class GameObject : BaseObject
{
    /// <summary>
    /// Holds the position, rotation and scale information as a <see cref="Mathmatics.Transform"/>.
    /// </summary>
    public Transform Transform;
    /// <summary>
    /// List of references to <see cref="Scene"/> this GameObject is a part of.
    /// </summary>
    public List<WeakReference<Scene>> Scenes;
    /// <summary>
    /// Reference to the <seealso cref="GameObject"/> Parent.
    /// </summary>
    public WeakReference<GameObject>? Parent;
    /// <summary>
    /// List of references to <seealso cref="GameObject"/> Children.
    /// </summary>
    public List<WeakReference<GameObject>> Children;
    /// <summary>
    /// List of <seealso cref="ScriptingComponent"/>.
    /// </summary>
    public List<ScriptingComponent> Components;
    /// <summary>
    /// List of <seealso cref="Asset"/> stored as <seealso cref="WeakReference"/>.
    /// </summary>
    public List<WeakReference<Asset>> Assets;
    

    public GameObject(RenderingLayer renderLayer = RenderingLayer.Default)
    {
        //TODO: add required scene

        Transform = new Transform();
        

        // Initialize the empty lists
        Children = new List<WeakReference<GameObject>>();
        Components = new List<ScriptingComponent>();
        Assets = new List<WeakReference<Asset>>();

    }
}
