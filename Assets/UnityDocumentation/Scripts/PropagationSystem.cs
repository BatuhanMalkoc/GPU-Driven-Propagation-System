using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class PropagationSystem : MonoBehaviour
{
    [SerializeField] private PropagationDataSO data;
    [SerializeField] private bool isDebugMode = false;
    private void Start()
    {
        CreateTransformGroups();
        CreateBuffers();
    }

    [NaughtyAttributes.Button("Reset Transform")]
    public void ResetSO()
    {
        data.transformBuffers.Clear();
        data.meshInstances.Clear();
        
    }



    [NaughtyAttributes.Button("Create Random")]
    public void CreateRandom()
    {
        CreateTransformGroups();
        CreateBuffers();

        Vector3 pos = Random.insideUnitSphere * 1f;
        Quaternion rot = Quaternion.Euler(Random.insideUnitSphere * 360f);
        Vector3 scale = Vector3.one * Random.Range(0.5f, 1.5f);
        Matrix4x4 trs = Matrix4x4.TRS(pos, rot, scale);

        for(int i = 0; i < data.meshInfos.Count; i++)
        {
            OnPainted(data.meshInfos[i], new List<Matrix4x4> { trs });
            Debug.Log("Yapýldý" + i);
        }
       

#if UNITY_EDITOR
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
#endif
    }

    void CreateTransformGroups()
    {
        for (int i = 0; i < data.meshInfos.Count; i++)
        {
            bool exists = data.meshInstances.Exists(group => group.meshIndex == i);
            if (!exists)
            {
                MatrixGroup group = new MatrixGroup { meshIndex = i };
                data.meshInstances.Add(group);
            }
        }
    }

    void CreateBuffers()
    {
        data.transformBuffers.Clear();

        for (int i = 0; i < data.meshInfos.Count; i++)
        {
            var buf = new ComputeBuffer(10, sizeof(float) * 16);
            data.transformBuffers.Add(buf);
        }
    }


    public void OnPainted(MeshInfo paintedMesh, List<Matrix4x4> meshTransforms)
    {
        int paintedMeshIndex = data.meshInfos.IndexOf(paintedMesh);
        MatrixGroup group = data.meshInstances.Find(g => g.meshIndex == paintedMeshIndex);

        if (group != null)
        {
            group.matrices.AddRange(meshTransforms);
            UpdateTransformBuffers(paintedMeshIndex);
        }

        if (isDebugMode)
        {
            DebugNewPositions(paintedMeshIndex);
        }
    }

    public void UpdateTransformBuffers(int meshIndex)
    {
        MatrixGroup group = data.meshInstances.Find(g => g.meshIndex == meshIndex);
        if (group == null) return;

        // 1) Mevcut buffer’ý serbest býrak
        var old = data.transformBuffers[meshIndex];
        if (old != null)
            old.Release();

        // 2) Yeni buffer’ý oluþtur
        var list = group.matrices;
        var newBuf = new ComputeBuffer(list.Count, sizeof(float) * 16);

        // 3) Veriyi GPU’ya gönder
        newBuf.SetData(list);

        // 4) Listeye kaydet
        data.transformBuffers[meshIndex] = newBuf;
    }

    [NaughtyAttributes.Button("Debug New Positions")]
    void DebugNewPositions(int meshIndex = 0)
    {
        MatrixGroup group = data.meshInstances.Find(g => g.meshIndex == meshIndex);
        if (group == null) return;

        foreach (Matrix4x4 matrix in group.matrices)
        {
            Debug.Log(matrix);
        }
    }

    public void ReleaseAllBuffers()
    {
        for (int i = 0; i < data.transformBuffers.Count; i++)
        {
            data.transformBuffers[i]?.Release();
        }
        data.transformBuffers.Clear();
    }

    private void OnDestroy()
    {
        ReleaseAllBuffers();
    }

    private void OnDisable()
    {
        ReleaseAllBuffers();
    }
}
