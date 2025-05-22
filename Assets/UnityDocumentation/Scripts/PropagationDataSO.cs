using System;
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
    [HideInInspector][SerializeField] private int _previousMeshInfosCount = 0;
    public event Action OnMeshInfosChanged;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Sadece Inspector’da meshInfos.Count deðiþtiðinde çalýþsýn
        if (meshInfos.Count != _previousMeshInfosCount)
        {
            
            _previousMeshInfosCount = meshInfos.Count; 
            OnMeshInfosChanged?.Invoke();
            EditorUtility.SetDirty(this);      // Deðiþiklikleri kaydetmek için
            AssetDatabase.SaveAssets();
        }
    }
#endif

}
[System.Serializable]
public class MatrixGroup
{
    public int meshIndex;
    public List<Matrix4x4> matrices = new List<Matrix4x4>();
    
    public MatrixGroup(int meshIndex)
    {
        this.meshIndex = meshIndex;
    }
}

[Serializable]
public struct MeshInfo
{
    [NaughtyAttributes.ShowAssetPreview(100, 100)]
    public Mesh mesh;
    public string meshName;
    public Material material;
    public int subMeshIndex;
    public MeshInfo(Mesh mesh, string name, Material material, int subMeshIndex)
    {
        this.mesh = mesh;
        this.material = material;
        this.subMeshIndex = subMeshIndex;
        this.meshName = name;
    }
}
