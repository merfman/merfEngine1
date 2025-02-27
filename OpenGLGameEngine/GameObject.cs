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
    //
    private List<Component> _components;

    private List<WeakReference<GameObject>> _children;//TODO: make child GameObjects also transform (and other stuff) when parent ones do (make the child ones also change when the parents do etc.)

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
    public List<WeakReference<GameObject>> Children => _children;
    /// <summary>
    /// List of <seealso cref="ScriptingComponent"/>.
    /// </summary>
    public List<Component> Components => _components;
    /// <summary>
    /// List of <seealso cref="Asset"/> stored as <seealso cref="WeakReference"/>.
    /// </summary>
    public List<WeakReference<Asset>> Assets;
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <remarks> This class was partially written by ChatGPT. </remarks>
    public T AddComponent<T>(params object[] args) where T : Component
    {
        // Create the component dynamically, passing `this` as the first argument
        object[] constructorArgs = new object[] { this }.Concat(args).ToArray();

        T component = (T)Activator.CreateInstance(typeof(T), constructorArgs)!;
        _components.Add(component);
        return component;
    }
    public void RemoveComponent<T>() where T : Component
    {

    }
    public GameObject()
    {
        //TODO: add required scene

        Transform = new Transform();
        
        // Initialize the empty lists
        _children = new List<WeakReference<GameObject>>();
        _components = new List<Component>();
        Assets = new List<WeakReference<Asset>>();
    }
}
