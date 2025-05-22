using System;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using Unity.VisualScripting;


public static class PropagationRenderer
{
    public static void RenderEditor(PropagationDataSO data)
    {
        RenderInternal(data);
    }
    public static void RenderRuntime(PropagationDataSO data)
    {
        RenderInternal(data);
    }

     private static void RenderInternal(PropagationDataSO data)
    {

        if (data == null || data.meshInfos.Count == 0) return;


       

        for (int i = 0; i < data.meshInfos.Count; i++)
        {
            MeshInfo mesh = data.meshInfos[i];
            MatrixGroup group = data.meshInstances.Find(g => g.meshIndex == i);
            data.frusturedMeshInstances[i]  = FrustrumCulling(group, i, mesh.mesh);

            if (data.frusturedMeshInstances == null || data.frusturedMeshInstances[i].matrices.Count == 0) continue;


            ComputeBuffer buffer = data.transformBuffers[i];
            if (buffer == null) continue;

            RenderParams rp = new RenderParams(mesh.material)
            {
                worldBounds = new Bounds(Vector3.zero, 1000 * Vector3.one), // use tighter bounds
                matProps = new MaterialPropertyBlock()
            };
         
            rp.matProps.SetBuffer("_TransformBuffer", buffer);
            Graphics.RenderMeshPrimitives(rp, mesh.mesh, 0, data.frusturedMeshInstances[i].matrices.Count);
        }

    }

    private static MatrixGroup FrustrumCulling(MatrixGroup allMeshPoints,int index,Mesh mesh)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        Bounds meshBounds = mesh.bounds;
        MatrixGroup FrustrumGroup = new();
        FrustrumGroup.matrices = new List<Matrix4x4>();
        for (int i = 0; i < allMeshPoints.matrices.Count; i++)
        {
            Matrix4x4 matrix = allMeshPoints.matrices[i];


          meshBounds.center = matrix.GetColumn(3);
            if (GeometryUtility.TestPlanesAABB(planes,meshBounds))
            {
                FrustrumGroup.matrices.Add(allMeshPoints.matrices[i]);
              

            }
        }

     

        return FrustrumGroup;
    }


}



