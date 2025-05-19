using UnityEditor;
using UnityEngine;

public class MyToolWindow : EditorWindow
{
    const string EditorPrefKey = "MyToolWindow_PropagationDataPath";

    public PropagationDataSO propagationDataSO;
    [MenuItem("Window/Propagation")]
    public static void ShowWindow()
    {
        var w = GetWindow<MyToolWindow>("Propagation");
        YourEditorRuntime.SetWindow(w);

    }

    private void OnEnable()
    {
        string path = EditorPrefs.GetString(EditorPrefKey, null);
        if (!string.IsNullOrEmpty(path))
        {
            propagationDataSO = AssetDatabase.LoadAssetAtPath<PropagationDataSO>(path);
        }
    }
    void OnDisable()
    {
        // Pencere kapanýrken seçilen referansýn yolunu kaydet
        if (propagationDataSO != null)
        {
            string path = AssetDatabase.GetAssetPath(propagationDataSO);
            EditorPrefs.SetString(EditorPrefKey, path);
            Debug.Log(EditorPrefKey + path);
        }
        else
        {
            EditorPrefs.DeleteKey(EditorPrefKey);

        }
    }
    void OnGUI()
    {
        GUILayout.Label("Instancing Aracý", EditorStyles.boldLabel);


        bool isButtonClicked = GUILayout.Button("Preview Mode", GUILayout.Width(100));
        if (isButtonClicked)
        {
            YourEditorRuntime.SetWindow(this);
        }
        propagationDataSO = (PropagationDataSO)EditorGUILayout.ObjectField(
            "Propagation Data",
            propagationDataSO,
            typeof(PropagationDataSO),
            allowSceneObjects: true
        );

    }
}