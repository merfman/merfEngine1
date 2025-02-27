using OpenGLGameEngine.Rendering;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Assets;
public class Material : Asset
{
    public WeakReference<Shader> Shader;

    public WeakReference<Texture>? ColorMap;
    public WeakReference<Texture>? NormalMap;
    public WeakReference<Texture>? AOMap;
    public WeakReference<Texture>? RoughnessMap;
    public Color4 BaseColor;
    public Vector3 Normal;
    public float Roughness;
    public float Shininess;
    public float Alpha;

    public Material(Shader shader)
    {
        BaseColor = new Color4(127.0f, 127.0f, 127.0f, 255.0f);
        //shader.set
        //set Shader settings here
        Shader = new WeakReference<Shader>(shader);
    }
    public override void LoadFromFile(string path)
    {
        //TODO: add a save and load to json file type
    }

}
