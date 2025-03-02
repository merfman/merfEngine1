using OpenGLGameEngine.Mathmatics;
using OpenGLGameEngine.Assets;
using OpenGLGameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Numerics;
using OpenTK.Mathematics;

namespace OpenGLGameEngine;
/// <summary>
/// Represents an versatile object that can have many purposes.
/// </summary>
public class GameObject : BaseObject
{
    //
    private List<Component> _components;

    private List<WeakReference<GameObject>> _children; //TODO: make child GameObjects also transform (and other stuff) when parent ones do (make the child ones also change when the parents do etc.)

    private Transform _localTransform;
    private Transform _globalTransform;

    /// <summary>
    /// Holds the position, rotation and scale information as a <see cref="Mathmatics.Transform"/>.
    /// </summary>
    public Transform Transform
    {
        get => _globalTransform;
        set
        {
            _localTransform = value;
            _globalTransform.OnTransformChanged += OnTransformChanged;
        }
    }

    private void OnTransformChanged(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // This should recursively loop through all children and change the transform with the parent
        if (_children != null)
            foreach (var childRef in _children)
                if (childRef.TryGetTarget(out GameObject? child))
                {
                    //child.Transform.Position -= position;
                    //child.Transform.Rotation -= rotation;
                    //child.Transform.Scale -= scale;

                    child.Transform.Position += Transform.Position;
                    child.Transform.Rotation += Transform.Rotation;
                    //child.Transform.Scale += Transform.Scale;

                }
    }

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
        Console.WriteLine($"Component of type ({typeof(T).ToString()}) Added to {this.Name ?? this.GetType().Name}");
        return component;
    }
    public void RemoveComponent<T>() where T : Component
    {

    }
    public WeakReference<GameObject> AddChild(GameObject child)
    {
        Console.WriteLine($"Child ({child.Name}) Added to {this.Name}");
        WeakReference<GameObject> childRef = new WeakReference<GameObject>(child);
        _children.Add(childRef);
        child.Parent = new WeakReference<GameObject>(this);
        return childRef;
    }
    public GameObject(string? name = null)
    {
        if (name != null)
            Name = name;

        //TODO: add required scene

        _globalTransform = new Transform();
        Transform = new Transform();
        
        // Initialize the empty lists
        _children = new List<WeakReference<GameObject>>();
        _components = new List<Component>();
        Assets = new List<WeakReference<Asset>>();
    }
}
