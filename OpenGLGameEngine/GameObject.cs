﻿using OpenGLGameEngine.Mathmatics;
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
        set => _localTransform = value;
    }

    /// <summary>
    /// List of references to <see cref="Scene"/> this GameObject is a part of.
    /// </summary>
    public List<WeakReference<Scene>> Scenes;
    /// <summary>
    /// Reference to the <see cref="GameObject"/> Parent.
    /// </summary>
    public WeakReference<GameObject>? Parent;
    /// <summary>
    /// List of references to <see cref="GameObject"/> Children. To add a Child use <see cref="AddChild(GameObject)"/>
    /// </summary>
    public List<WeakReference<GameObject>> Children => _children; //TODO: make it so children cant be added with children.Add(child);
    /// <summary>
    /// List of <see cref="Component"/>. To add a <see cref="Component"/> use <see cref="AddComponent{T}(object[])"/>
    /// </summary>
    public List<Component> Components => _components; //TODO: make it so components cant be added with components.Add(component);

    /// <summary>
    /// List of <seealso cref="Asset"/> stored as <seealso cref="WeakReference"/>.
    /// </summary>
    public List<WeakReference<Asset>> Assets;
    
    /// <summary>
    /// Adds a Component to the <see cref="GameObject"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    public T AddComponent<T>(params object[] args) where T : Component
    {
        // This class was partially written by ChatGPT.
        // Create the component dynamically, passing `this` as the first argument
        object[] constructorArgs = new object[] { this }.Concat(args).ToArray();

        T component = (T)Activator.CreateInstance(typeof(T), constructorArgs)!;
        _components.Add(component);
        Console.WriteLine($"Component of type ({typeof(T).ToString()}) Added to {this.Name ?? this.GetType().Name}");
        return component;
    }
    public void RemoveComponent<T>() where T : Component //TODO: add functionality
    {

    }
    public WeakReference<GameObject> AddChild(GameObject child)
    {
        Console.WriteLine($"Child ({child.Name}) Added to {this.Name}");
        WeakReference<GameObject> childRef = new WeakReference<GameObject>(child);
        _children.Add(childRef);
        child.Parent = new WeakReference<GameObject>(this);
        _globalTransform.OnTransformChanged += child.OnParentTransformChanged;
        return childRef;
    }

    private void OnParentTransformChanged(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        //if there is a parent(which there should always be if this method is called), then it offsets the transform to the parents transform
        if (Parent.TryGetTarget(out GameObject? parent))
        {
            _globalTransform.Rotation = parent.Transform.Rotation;
            _globalTransform.Rotation += _localTransform.Rotation;
            _globalTransform.Position = parent.Transform.Position;
            _globalTransform.Position += 
                                        (_globalTransform.Front * _localTransform.Position.X) + 
                                        (_globalTransform.Up * _localTransform.Position.Y) + 
                                        (_globalTransform.Right * _localTransform.Position.Z);
        }
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
