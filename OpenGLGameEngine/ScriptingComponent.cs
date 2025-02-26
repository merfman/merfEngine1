using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLGameEngine.Mathmatics;
using OpenTK.Mathematics;

namespace OpenGLGameEngine;
/// <summary>
/// 
/// </summary>
public abstract class ScriptingComponent : Component
{
    public virtual void Start() { }
    public virtual void Awake() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void OnDestroy() { }
    public virtual GameObject Instantiate(Vector3 position, Quaternion rotation, Vector3 size) => new GameObject();
    public virtual GameObject Instantiate(Transform transform) => new GameObject();

    public void AddComponent<T>() { }
    
    public ScriptingComponent GetComponent<T>()
    {
        return this;
    }
    public void Destroy() { }
}
