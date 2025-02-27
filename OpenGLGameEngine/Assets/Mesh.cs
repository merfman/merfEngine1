using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace OpenGLGameEngine.Assets;

/// <summary>
/// Holds Mesh Data
/// </summary>
internal class Mesh : Asset
{
    private bool _isInitialized = false;
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
        base.LoadFromFile(path);
        (float[] vertexData, int[] faceIndices, float[] normals, float[] tangentData, float[] textureCoords) = OBJParser.Parse(path);

        bindRenderObjects(InterleaveData(vertexData, normals, tangentData, textureCoords), faceIndices);
        
        List<Vector3> VerPoss, norms;
        VerPoss = norms = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int[]> faceIndices2 = new List<int[]>();
        UV = new List<Vector2[]>();

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="normals"></param>
    /// <param name="tangents"></param>
    /// <param name="texCoords"></param>
    /// <returns></returns>
    /// <remarks>Written By ChatGPT</remarks>
    private float[] InterleaveData(float[] vertices, float[] normals, float[] tangents, float[] texCoords)
    {
        int vertexCount = vertices.Length / 3;
        float[] interleavedData = new float[vertexCount * 11]; // 11 floats per vertex

        for (int i = 0; i < vertexCount; i++)
        {
            int vertexOffset = i * 3;
            int normalOffset = i * 3;
            int tangentOffset = i * 3;
            int texCoordOffset = i * 2;
            int interleavedOffset = i * 11;

            // Position (XYZ)
            interleavedData[interleavedOffset] = vertices[vertexOffset];
            interleavedData[interleavedOffset + 1] = vertices[vertexOffset + 1];
            interleavedData[interleavedOffset + 2] = vertices[vertexOffset + 2];

            // Normal (XYZ)
            interleavedData[interleavedOffset + 3] = normals[normalOffset];
            interleavedData[interleavedOffset + 4] = normals[normalOffset + 1];
            interleavedData[interleavedOffset + 5] = normals[normalOffset + 2];

            // Tangent (XYZ)
            interleavedData[interleavedOffset + 6] = tangents[tangentOffset];
            interleavedData[interleavedOffset + 7] = tangents[tangentOffset + 1];
            interleavedData[interleavedOffset + 8] = tangents[tangentOffset + 2];

            // Texture Coordinates (UV)
            interleavedData[interleavedOffset + 9] = texCoords[texCoordOffset];
            interleavedData[interleavedOffset + 10] = texCoords[texCoordOffset + 1];
        }

        return interleavedData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="indices"></param>
    private void bindRenderObjects(float[] vertices, int[] indices)
    {
        if (_isInitialized) return;

        VAO = GL.GenVertexArray();
        VBO = GL.GenBuffer();
        EBO = GL.GenBuffer();

        GL.BindVertexArray(VAO);

        // Upload vertex data
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        // Upload index data
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

        // Define vertex attributes
        int stride = 11 * sizeof(float);

        // Position (location = 0)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
        GL.EnableVertexAttribArray(0);

        // Normal (location = 1)
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // Tangent (location = 2)
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);

        // Texture Coordinates (location = 3)
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, stride, 9 * sizeof(float));
        GL.EnableVertexAttribArray(3);

        GL.BindVertexArray(0);//TODO: is this needed?

        GL.BindVertexArray(VAO);


        _isInitialized = true;
    }
}
