using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenGLGameEngine.Mathmatics;
/// <summary>
/// 
/// </summary>
public class Transform : BaseObject
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public Vector3 Front;
    public Vector3 Up;
    public Vector3 Right;
}
