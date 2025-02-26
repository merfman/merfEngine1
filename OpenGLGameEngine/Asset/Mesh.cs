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

/// <summary>
/// Holds Mesh Data
/// </summary>
internal class Mesh : Asset
{
    /// <summary>
    /// This holds the vertex positions as a <seealso cref="Vector3"/>.
    /// </summary>
    public Vector3[] VertexPositions;
    /// <summary>
    /// The Vertex Indices that make up each face as an <seealso cref="int"/>.
    /// </summary>
    public int[][] FaceIndices;
    /// <summary>
    /// The normal of each Vertex as a <seealso cref="Vector3"/>.
    /// </summary>
    public Vector3[] Normals;
    /// <summary>
    /// The tangent of each Vertex as a <seealso cref="Vector3"/>.
    /// </summary>
    public Vector3[] Tangents;
    /// <summary>
    /// The texture coordenates held as UV maps. Can support more than 1 set for more advanced shading.
    /// </summary>
    public List<Vector2[]> UV;
    /// <summary>
    /// The Bone Weights per vertex.
    /// </summary>
    public float[] BoneWeights;


    /// <summary>
    /// Stores the Vertex Array Object.
    /// </summary>
    public int VAO;
    /// <summary>
    /// Stores the Vertex Buffer Object.
    /// </summary>
    public int VBO;
    /// <summary>
    /// Stores the Element Buffer Object.
    /// </summary>
    public int EBO;

    public Mesh(string path, string? name = null)
    {
        if (name == null) Name = System.IO.Path.GetFileName(path);
        LoadFromFile(path);
    }

    public override void LoadFromFile(string path)
    {
        Console.WriteLine($"Loading File {path}");
        float[] textureCoords, vertexData, normals;
        int[] faceIndices;
        (vertexData, faceIndices, normals, textureCoords) = OBJParser.Parse(path);

        List<Vector3> VerPoss, norms;
        VerPoss = norms = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int[]> faceIndices2 = new List<int[]>();

        for (int i = 2; i < vertexData.Length; i += 3)
        {
            VerPoss.Add(new Vector3(vertexData[i - 2], vertexData[i - 1], vertexData[i]));
            norms.Add(new Vector3(normals[i - 2], normals[i - 1], normals[i]));
        }
        for (int i = 2; i < faceIndices.Length; i += 3)
        {
            //TODO:add more than triangle support
            faceIndices2.Add([faceIndices[i - 2], faceIndices[i - 1], faceIndices[i]]);
        }
        for (int i = 1; i < textureCoords.Length; i += 2)
        {
            uv.Add(new Vector2(vertexData[i - 1], vertexData[i]));
        }
        VertexPositions = VerPoss.ToArray();
        UV.Add(uv.ToArray());
        FaceIndices = faceIndices2.ToArray();
        Normals = norms.ToArray();
    }
}
