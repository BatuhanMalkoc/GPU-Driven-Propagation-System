#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
public static class YourEditorRuntime 
{
   
    static YourEditorRuntime()
    {
      
      
        SceneView.duringSceneGui += OnSceneGUI;
        
    }

    static MyToolWindow _window;
    public static void SetWindow(MyToolWindow w) => _window = w;


    static void OnSceneGUI(SceneView sceneView)
    {

        if (_window == null || _window.propagationDataSO == null)
            return;

        // Your code to handle the scene view GUI

        // Your GUI code here
        RenderInstanced(_window.propagationDataSO.meshInfos[0]);
    }
    public static void RenderInstanced(MeshInfo mesh)
    {
        if (_window.propagationDataSO == null) return;
        if(_window.propagationDataSO.meshInstances == null || _window.propagationDataSO.meshInstances.Count == 0) return;

        

        PropagationDataSO pData = _window.propagationDataSO;
        int meshIndex = pData.meshInfos.IndexOf(mesh);
        RenderParams rp = new RenderParams(mesh.material);
        rp.worldBounds = new Bounds(Vector3.zero, 1000 * Vector3.one); // use tighter bounds
        rp.matProps = new MaterialPropertyBlock();
        rp.matProps.SetBuffer("_TransformBuffer", pData.transformBuffers[meshIndex]);
        Graphics.RenderMeshPrimitives(rp, mesh.mesh, 0, pData.meshInstances[meshIndex].matrices.Count);
       
    }
}
#endif