using OpenGLGameEngine.Mathmatics;
using OpenGLGameEngine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
/// <summary>
/// 
/// </summary>
/// <remarks> RenderLayer came from a suggestion by ChatGPT. </remarks>
public enum RenderingLayer
{
    /// <summary>
    /// Special layer for non-rendered objects
    /// </summary>
    Hidden = -1,
    /// <summary>
    /// 
    /// </summary>
    Default = 0
}
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
    /// List of <seealso cref="Component"/>.
    /// </summary>
    public List<Component> Components;
    /// <summary>
    /// List of <seealso cref="Asset"/> stored as <seealso cref="WeakReference"/>.
    /// </summary>
    public List<WeakReference<Asset>> Assets;
    /// <summary>
    /// A <see cref="RenderingLayer"/> that determines when or if the <see cref="GameObject"/> is rendered.
    /// </summary>
    /// <remarks> RenderLayer as an enum came from a suggestion by ChatGPT. </remarks>
    public RenderingLayer RenderLayer;

    public GameObject(RenderingLayer renderLayer = RenderingLayer.Default)
    {
        //TODO: add required scene

        Transform = new Transform();
        
        RenderLayer = RenderingLayer.Default;

        // Initialize the empty lists
        Children = new List<WeakReference<GameObject>>();
        Components = new List<Component>();
        Assets = new List<WeakReference<Asset>>();

    }
}
