using UnityEngine;
using System.Collections.Generic;


[ExecuteAlways]
public static class PropagationSystemm 
{
    private static PropagationDataSO data;
    private static int meshCount = 0;
    private static bool isInitialized = false;
    public static void Initialize(PropagationDataSO propagationData)
    {
        Debug.Log("Initialized");
        data = propagationData;
        meshCount = data.meshInfos.Count;
        data.OnMeshInfosChanged += OnMeshTypeListChanged;
        isInitialized = true;
        
    }
   public static bool GetIfInitialized()
    {
        if (isInitialized)
        {
            if(data == null)
            {
                Debug.LogError("PropagationDataSO is null");
                isInitialized = false;
                return false;
            }
            else
            {
                return true;
            }

        }
        else
        {
            if(data != null)
            {
                Debug.LogError("is initalized gözükmüyor ama data var ne alaka");

                isInitialized = true;
                return true;
            }
        }

        return false;
    }
    public static void SetupBuffers() //Her Yeni deðer eklendiðinde çalýþmasý lazým
    {
        ReleaseBuffers();
        data.transformBuffers.Clear();
        if(data.meshInfos.Count ==0)
        {
            Debug.Log("There is no mesh info");
            return;
        }
        for (int i = 0; i < data.meshInfos.Count; i++)
        {
            if (data.meshInstances[i].matrices.Count > 0)
            {
                MatrixGroup group = data.frusturedMeshInstances[0];
                var buf = new ComputeBuffer(group.matrices.Count, sizeof(float) * 16);
                data.transformBuffers.Add(buf);
                data.transformBuffers[i].SetData(group.matrices);
            }
        }
    }
    private static void ReleaseBuffers()
    {
        for (int i = 0; i < data.transformBuffers.Count; i++)
        {
            var buf = data.transformBuffers[i];
            if (buf != null)
            {
                buf.Release();
            }
        }
    }
    public static void OnBrushStroke(StrokeData strokeData)
    {
       
        if (!strokeData.isEraseMode)
        {
         
            Debug.Log(strokeData.meshIndex + "indexli Mesh");
            AddTransformList(strokeData.meshIndex,strokeData.matrices);
            
        }
    }

    private static void AddTransformList(int paintedMeshIndex,List<Matrix4x4> transformsToAdd)
    {
        SetupTransformList();
        Debug.Log("Transform Listesine ekleme yapýlmaya çalýþýldý ve mesh Count: "+meshCount);

       
        data.meshInstances[paintedMeshIndex].matrices.AddRange(transformsToAdd);
        SetupBuffers();
    }
    private static void RemoveTransformList()
    {

    }
    private static bool IsTransformListAvailable(int index)
    {
        bool isAvailable = false;
        if(data.meshInstances == null || data.meshInstances.Count == 0)
        {
            SetupTransformList();
        }

        if (data.meshInstances != null && data.meshInstances[index] != null)
        {
            isAvailable = data.meshInstances[index].matrices.Count < 999999;// Maks Count
        }
        return isAvailable;
    }
    private static void SetupTransformList()
    {
       
            for (int i = 0; i < data.meshInfos.Count; i++)
            {
                bool exists = data.meshInstances.Exists(group => group.meshIndex == i);
                if (!exists)
                {
                    MatrixGroup group = new MatrixGroup(i);
                    data.meshInstances.Add(group);
                }
            }

      
        
    }

    public static void OnMeshTypeListChanged()
    {
        meshCount = data.meshInfos.Count;
        Debug.Log("Yeni mesh count: " + meshCount);
        SetupTransformList();
        SetupBuffers();
    }

    public static void Reset()
    {
        Debug.Log("PropagationSystemm Reset");
        ReleaseBuffers();
        data = null;
        meshCount = 0;
        isInitialized = false;
    }
}

public struct StrokeData
{
    public int meshIndex;
    public List<Matrix4x4> matrices;
    public bool isEraseMode;
    public StrokeData(int meshIndex, List<Matrix4x4> matrices,bool isEraseMode)
    {
        this.meshIndex = meshIndex;
        this.matrices = matrices;
        this.isEraseMode = isEraseMode;
    }
}
