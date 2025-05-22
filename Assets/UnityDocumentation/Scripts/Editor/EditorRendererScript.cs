#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
public static class EditorPropagationHandler 
{
    static EditorPropagationHandler()
    { 
        SceneView.duringSceneGui += OnSceneGUI;     
    }

    static PropagationToolWindow _window;
    static PropagationSystem _pSystem;
    public static void SetWindow(PropagationToolWindow w) => _window = w;
    public static void SetPropagationSystem(PropagationSystem p) => _pSystem = p;
    static void OnSceneGUI(SceneView sceneView)
    {
        if (_window == null || _window.propagationDataSO == null)
            return;
        PropagationRenderer.RenderEditor(_window.propagationDataSO);
        Debug.Log("Ha gayret");
       Ray ray =  HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Transform spawnPosition = Physics.Raycast(ray, out RaycastHit hit) ? hit.transform : null;

        if (spawnPosition != null)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(hit.point, hit.normal, _window.brushSize);
            Handles.Label(hit.point + Vector3.up * 0.5f, "Spawn Point");
        }

      
    }

}
#endif