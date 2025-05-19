using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "Instanced Renderer", menuName = "ScriptableObjects/RenderInstanced", order = 1)]
public class PropagationDataSO : ScriptableObject
{
    public List<MatrixGroup> meshInstances = new List<MatrixGroup>();
    public List<ComputeBuffer> transformBuffers = new List<ComputeBuffer>();
    public List<MeshInfo> meshInfos = new List<MeshInfo>();
}

[System.Serializable]
public class MatrixGroup
{
    public int meshIndex;
    public List<Matrix4x4> matrices = new List<Matrix4x4>();
    
}
