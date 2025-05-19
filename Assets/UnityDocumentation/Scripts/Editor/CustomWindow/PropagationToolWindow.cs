using Codice.Client.Common.GameUI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class PropagationToolWindow : EditorWindow
{
    #region Variables
    const string PropagationSO_PrefKey = "EditorPref_PropagationSOKey";
    const string BrushSize_PrefKey = "EditorPref_BrushSizeKey";
    const string Density_PrefKey = "EditorPref_DensityKey";
    const string MeshIndex_PrefKey = "EditorPref_MeshIndexKey";
    

    public PropagationDataSO propagationDataSO;
    public float brushSize;
    public int density;
    public int selectedMeshIndex = 0;

    #endregion

    #region Opening Window
    [MenuItem("Tool/Propagation")]
    public static void ShowWindow()
    {
        var w = GetWindow<PropagationToolWindow>("Propagation");
       
        EditorPropagationHandler.SetWindow(w);
    }
    #endregion

    #region GUI Methods
    void GUI_Label()
    {
        GUILayout.Label("Instancing Aracı", EditorStyles.boldLabel);
       
    }
    void GUI_UpdateIcon()
    {
        Texture2D myIcon = EditorGUIUtility.Load("Assets/Editor/Icons/PropagationIcon.png") as Texture2D;
        titleContent = new GUIContent("Propagation", myIcon);
    }
    void GUI_MeshListAndPicker()
    {
       
        SerializedObject so = new SerializedObject(propagationDataSO);
        SerializedProperty meshList = so.FindProperty("meshInfos");
        EditorGUILayout.PropertyField(meshList, true);
        so.ApplyModifiedProperties();
         string[] meshNames = new string[meshList.arraySize];
        for (int i = 0; i < meshList.arraySize; i++)
        {
            meshNames[i] = propagationDataSO.meshInfos[i].meshName;
        }

        int cellWidth = 200;
        int columns = Mathf.Max(1, (int)(position.width / cellWidth));


        GUILayout.BeginVertical(); // Ana dikey layout

        // Ortalanmış görsel ve etiket
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(); // Bu, görsel ve metni dikeyde sıralamak için
        Texture2D myIcon = AssetPreview.GetAssetPreview(propagationDataSO.meshInfos[selectedMeshIndex].mesh);
        GUILayout.Label(myIcon, GUILayout.Width(150), GUILayout.Height(150));

        if (propagationDataSO.meshInfos.Count > 2)
        {
            GUILayout.Label("Selected Mesh: " + propagationDataSO.meshInfos[selectedMeshIndex].meshName,
                            EditorStyles.centeredGreyMiniLabel); // Ortalanmış stil
        }
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        // Altına seçim gridini koy
        selectedMeshIndex = GUILayout.SelectionGrid(selectedMeshIndex, meshNames, columns);

        GUILayout.EndVertical();

    }

    void GUI_BrushAdjusments()
    {
        brushSize = EditorGUILayout.Slider("Brush Size", brushSize, 0.1f, 10f);
        density = EditorGUILayout.IntSlider("Density", density, 1, 100);
    }
    void GUI_PropagationDataSO()
    {
        GUILayout.Label("Propagation Data", EditorStyles.boldLabel);
        propagationDataSO = (PropagationDataSO)EditorGUILayout.ObjectField(
          "Propagation Data",
          propagationDataSO,
          typeof(PropagationDataSO),
          allowSceneObjects: true
      );
    }
    void OnGUI()
    {
        #region Update Window Icon
        GUI_UpdateIcon();
        #endregion

        #region Window Title
        // Başlık
        GUI_Label();
           EditorGUILayout.Space(10);
        #endregion

        #region Propagation Data Input
        // ► Data SO Seçici
        EditorGUILayout.BeginVertical("box");
           
            GUI_PropagationDataSO();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);
        #endregion

        #region Mesh List and Picker
        // ► Mesh Listesi ve Seçimi
        if (propagationDataSO != null)
            {
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Mesh Seçimi", EditorStyles.boldLabel);
                GUI_MeshListAndPicker();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);
            #endregion

        #region Brush Settings
            // ► Brush Ayarları
            EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Brush Ayarları", EditorStyles.boldLabel);
                GUI_BrushAdjusments();
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("Lütfen bir Propagation Data SO dosyası atayın.", MessageType.Warning);
            }

            EditorGUILayout.Space(10);
        #endregion

        #region Preview Button
        // ► Preview Button
        EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool isButtonClicked = GUILayout.Button("Preview Mode", GUILayout.Width(150), GUILayout.Height(30));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (isButtonClicked)
            {
                EditorPropagationHandler.SetWindow(this);
            }
        #endregion
    }

    #endregion

    #region EditorPrefs
    private void OnEnable()
    {
        string path = EditorPrefs.GetString(PropagationSO_PrefKey, null);
        if (!string.IsNullOrEmpty(path))
        {
            propagationDataSO = AssetDatabase.LoadAssetAtPath<PropagationDataSO>(path);
        }
       brushSize = EditorPrefs.GetFloat(BrushSize_PrefKey, 1f);
        selectedMeshIndex = EditorPrefs.GetInt(MeshIndex_PrefKey, 0);
        density = EditorPrefs.GetInt(Density_PrefKey, 1);
    }
    void OnDisable()
    {
        // Pencere kapanırken seçilen referansın yolunu kaydet
        if (propagationDataSO != null)
        {
            string path = AssetDatabase.GetAssetPath(propagationDataSO);
            EditorPrefs.SetString(PropagationSO_PrefKey, path);
            Debug.Log(PropagationSO_PrefKey + path);
        }
        else
        {
            EditorPrefs.DeleteKey(PropagationSO_PrefKey);

        }
       EditorPrefs.SetFloat(BrushSize_PrefKey, brushSize);
        EditorPrefs.SetInt(MeshIndex_PrefKey, selectedMeshIndex);
        EditorPrefs.SetInt(Density_PrefKey, density);
    }
    #endregion
}