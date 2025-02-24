using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace OpenGLGameEngine.Asset;

internal class Mesh : Asset
{
    public Vector3[] VertexPositions;
    public int[] FaceIndices;
    public float[] Normals;
    public float[] Tangents;
    public List<float[]> UV;
    public float[] BoneWeights;
    public int[] BoneIndices;

    public int VAO; // Vertex Array Object 
    public int VBO; // Vertex Buffer Object
    public int EBO; // Element Buffer Object

    public Mesh(string path, string? name = null)
    {
        if (name == null) Name = System.IO.Path.GetFileName(path);
        LoadFromFile(path);
    }

    public override void LoadFromFile(string path)
    {
        Console.WriteLine($"Loading File {path}");
        float[] textureCoords, vertexData;
        (vertexData, this.FaceIndices, this.Normals, textureCoords) = OBJParser.Parse(path);
        UV.Add(textureCoords);

        List<Vector3> VerPoss = new List<Vector3>();

        for (int i = 2; i < vertexData.Length; i += 3)
        {
            VerPoss.Add(new Vector3(vertexData[i - 2], vertexData[i - 1], vertexData[i]));
        }
        VertexPositions = VerPoss.ToArray();

        throw new NotImplementedException();
    }
}
