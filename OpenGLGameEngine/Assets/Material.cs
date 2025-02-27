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

        if (ColorMap != null && ColorMap.TryGetTarget(out Texture? colorMap)) 
            shader.SetSampler2D("textureSampler", colorMap, 0);
        if (NormalMap != null && NormalMap.TryGetTarget(out Texture? normalMap))
            shader.SetSampler2D("textureSampler", normalMap, 0);
        if (AOMap != null && AOMap.TryGetTarget(out Texture? aoMap))
            shader.SetSampler2D("textureSampler", aoMap, 0);
        if (RoughnessMap != null && RoughnessMap.TryGetTarget(out Texture? roughnessMap))
            shader.SetSampler2D("textureSampler", roughnessMap, 0);

        shader.SetInt("material.diffuse", 0);
        shader.SetInt("material.specular", 1);
        shader.SetInt("material.normal", 2);
        shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));

        Shader = new WeakReference<Shader>(shader);
    }
    public override void LoadFromFile(string path)
    {
        //TODO: add a save and load to json file type
    }

}
