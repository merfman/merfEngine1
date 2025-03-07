using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace OpenGLGameEngine.Mathmatics;
/// <summary>
/// 
/// </summary>
/// <remarks> Copy/Pasted from last project, intend on replacing with new one</remarks>
public class Transform : BaseObject
{
    public event Action<Vector3, Quaternion, Vector3>? OnTransformChanged; // Event triggered when Transform changed

    private Vector3 _position = new Vector3(0, 0, 0); //TODO: move the initilaization to the constructor

    private Quaternion _rotation = new Quaternion(0, 0, 0, 1);

    private Vector3 _scale = new Vector3(1, 1, 1);

    // direction pointing frontwards from the Transform
    private Vector3 _front = -Vector3.UnitZ;

    // direction pointing upwards from the Transform
    private Vector3 _up = Vector3.UnitY;

    // direction pointing out of side of the Transform
    private Vector3 _right = Vector3.UnitX;

    // Rotation around the X axis (radians)
    private float _pitch;

    // Rotation around the Y axis (radians)
    private float _yaw;

    // Rotation around the Z axis (radians)
    private float _roll;


    public Transform() { }
    public Transform(Vector3? position, Quaternion? rotation = null, Vector3? scale = null) => 
        (Position, Rotation, Scale) = (position ?? Vector3.Zero, rotation ?? new Quaternion(0, 0, 0, 1), scale ?? new Vector3(1, 1, 1));
    public Transform(Vector3? position = null, Vector3? rotation = null, Vector3? scale = null) =>
        (Position, Rotation, Scale) = (position ?? Vector3.Zero, Quaternion.FromEulerAngles(rotation ?? Vector3.Zero) , scale ?? Vector3.One);

    public Vector3 Position
    {
        get => _position;
        set
        {
            OnTransformChanged?.Invoke(_position, Rotation, Scale);
            _position = value;
        }
    }
    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            UpdateVectors();
            UpdateEulerAngles();
        }
    }
    public Vector3 Scale
    {
        get => _scale;
        set
        {
            OnTransformChanged?.Invoke(Position, Rotation, Scale);
            _scale = value;
        }
    }

    public Vector3 Front => _front;
    public Vector3 Up => _up;
    public Vector3 Right => _right;

    public float Pitch
    // We convert from degrees to radians as soon as the property is set to improve performance.
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            _pitch = MathHelper.DegreesToRadians(value);
            UpdateVectors();
            UpdateQuaternion();
        }
    }
    public float Yaw
    // We convert from degrees to radians as soon as the property is set to improve performance.
    {
        get => MathHelper.RadiansToDegrees(_yaw);
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
            UpdateVectors();
            UpdateQuaternion();
        }
    }

    public float Roll
    {
        get => MathHelper.RadiansToDegrees(_roll);
        set
        {
            _roll = MathHelper.DegreesToRadians(value);
            UpdateVectors();
            UpdateQuaternion();
        }
    }

    public static Transform operator +(Transform left, Transform right)
    {
        left.Position += right.Position;
        left.Rotation += right.Rotation;
        left.Scale += right.Scale;
        return left;
    }
    public static Transform operator -(Transform left, Transform right)
    {
        left.Position -= right.Position;
        left.Rotation -= right.Rotation;
        left.Scale -= right.Scale;
        return left;
    }
    public override bool Equals(object? obj)
    {
        if (obj is Transform transform)
        {
            if (!transform.Position.Equals(this.Position)) return false;
            if (!transform.Rotation.Equals(this.Rotation)) return false;
            if (!transform.Scale.Equals(this.Scale)) return false;
        }
        return true;
    }
    public static bool operator ==(Transform left, Transform right) => left.Equals(right);
    public static bool operator !=(Transform left, Transform right) => !(left == right);

    //called when any Euler Angles change, this makes sure the Quaternion stays synced
    private void UpdateQuaternion()
    {
        _rotation = Quaternion.FromEulerAngles(_pitch, _yaw, _roll);
        OnTransformChanged?.Invoke(Position, _rotation, Scale);
    }

    //called when the Quaternion is changed, this makes sure the Euler Angles stay synced
    private void UpdateEulerAngles()
    {
        OnTransformChanged?.Invoke(Position, _rotation, Scale);
        (_pitch, _yaw, _roll) = _rotation.ToEulerAngles();
    }

    private void UpdateVectors()
    // This function is going to update the direction vectors using some of the math learned in the web tutorials.
    {
        // First, the front matrix is calculated using some basic trigonometry.
        _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
        _front.Y = MathF.Sin(_pitch);
        _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

        // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
        _front = Vector3.Normalize(_front);

        // Calculate both the right and the up vector using cross product.
        // Note that we are calculating the right from the global up; this behaviour might
        // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
        _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        _up = Vector3.Normalize(Vector3.Cross(_right, _front));
    }

    public Matrix4 GetLookAtMatrix() => Matrix4.LookAt(Position, Position + Front, Up);

    public override string ToString()
    {
        return $"Position:({Position.ToString()}), Rotation:({Rotation.ToString()}), Scale:({Scale.ToString()})";
    }
}
