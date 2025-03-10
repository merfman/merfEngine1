using OpenGLGameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
/// <summary>
/// 
/// </summary>
public class Scene : BaseObject
{
    /// <summary>
    /// List of <see cref="GameObject"/>s in the scene;
    /// </summary>
    private List<GameObject> _gameObjects;
    //public List<GameObject> GameObjects;

    /// <summary>
    /// <see cref="GameObject"/> that holds <see cref="Scene"/>
    /// </summary>
    public GameObject SceneObject;

    /// <summary>
    /// 
    /// </summary>
    public CameraComponent ActiveCamera;

    public Scene(string? name = null) : base(name)
    {
        SceneObject = new GameObject(name);
        _gameObjects = new List<GameObject>();
    }

    public void AddGameObject(ref GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }

    /// <summary>
    /// Saves the Scene to a json file.
    /// </summary>
    /// <param name="path"></param>
    public void SaveSceneToFile(string path) => throw new NotImplementedException();

    /// <summary>
    /// Initializes a scene and loads it from a json file.
    /// </summary>
    /// <param name="path"></param>
    static public void LoadSceneFromFile(string path) => throw new NotImplementedException();
}
