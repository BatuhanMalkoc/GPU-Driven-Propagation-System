using System;
using UnityEngine;
using NaughtyAttributes;


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

        for(int i = 0; i < data.meshInfos.Count; i++)
        {
            MeshInfo mesh = data.meshInfos[i];
            MatrixGroup group = data.meshInstances.Find(g => g.meshIndex == i);

            if (group == null || group.matrices.Count == 0) continue;

            ComputeBuffer buffer = data.transformBuffers[i];
            if (buffer == null) continue;

            RenderParams rp = new RenderParams(mesh.material)
            {
                worldBounds = new Bounds(Vector3.zero, 1000 * Vector3.one), // use tighter bounds
                matProps = new MaterialPropertyBlock()
            };

            rp.matProps.SetBuffer("_TransformBuffer", buffer);
            Graphics.RenderMeshPrimitives(rp, mesh.mesh, 0, group.matrices.Count);
        }

    }

}



