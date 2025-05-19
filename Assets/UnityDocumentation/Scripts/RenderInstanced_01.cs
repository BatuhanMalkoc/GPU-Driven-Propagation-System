
using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using Unity.Collections;
/*
[ExecuteAlways]
public class RenderInstanced_01 : MonoBehaviour
{
    public Material material;
    public Mesh mesh;
    public int spawnCount = 10000;


    Dictionary<int,List<Matrix4x4>> transformListDict = new Dictionary<int, List<Matrix4x4>>(); //0 for mesh1, 1 for mesh2, etc.
    public List<ComputeBuffer> transformBuffers = new List<ComputeBuffer>();

    [SerializeField] private List<MeshInfo> meshInfos = new List<MeshInfo>();

    public int meshCount = 2;

    [SerializeField] private PropagationDataSO p;


    void Start()
    {
 

        meshCount = p.meshInfos.Count;

        for (int i = 0; i < meshInfos.Count; i++)
        {
            
            if(!p.transformListDict.ContainsKey(i))
            {
                p.transformListDict[i] = new List<Matrix4x4>();
  
            }
        }

        CreateComputeBuffers();
        for (int i = 0; i < p.meshInfos.Count; i++)
        {
            CreateRandomPositions(i);
            SetComputeBufferData(i);
        }

    }
    void Update()
    {
      /*  for (int i = 0; i < meshCount; i++)
        {
            RenderInstanced(p.meshInfos[i]);
        }
      */
    //}


    /*void OnDestroy()
    {
        for(int i = 0; i < p.transformBuffers.Count; i++)
        {
                p.transformBuffers[i]?.Release();
        }
    }

    private void CreateRandomPositions(int meshIndex)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 100f;
            Quaternion rot = Quaternion.Euler(Random.insideUnitSphere * 360f);
            Vector3 scale = Vector3.one * Random.Range(0.5f, 1.5f);
            Matrix4x4 trs = Matrix4x4.TRS(pos, rot, scale);

            p.transformListDict[meshIndex].Add(trs);
            //instanceData[meshInfos[meshIndex]].Add(new InstanceData(trs, 0));
        }
    }

    public void SetComputeBufferData(int meshIndex)
    {
        p.transformBuffers[meshIndex].SetData(p.transformListDict[meshIndex]);
    }


    public void CreateComputeBuffers()
    {
 

        for(int i =0; i < meshCount; i++)
        {
            p.transformBuffers.Add(new ComputeBuffer(100 , sizeof(float) * 16));
        }
    }


    public void RenderInstanced(MeshInfo mesh)
    {
        RenderParams rp = new RenderParams(mesh.material);
        rp.worldBounds = new Bounds(Vector3.zero, 1000 * Vector3.one); // use tighter bounds
        rp.matProps = new MaterialPropertyBlock();
        rp.matProps.SetBuffer("_TransformBuffer", transformBuffers[meshInfos.IndexOf(mesh)]);
        Graphics.RenderMeshPrimitives(rp,mesh.mesh,0,spawnCount);
    }
}



*/

public struct InstanceData
{
    public Matrix4x4 transform;
    public int meshIndex; // 0–4 arasýnda deðer alýr

    public InstanceData(Matrix4x4 transform, int meshIndex)
    {
        this.transform = transform;
        this.meshIndex = meshIndex;
    }
}
    