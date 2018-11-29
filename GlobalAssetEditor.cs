using System;
using UnityEngine;
using UnityEditor;

public class GlobalAssetEditor : EditorWindow
{

    public static bool EditCrunchCompression
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool("Edit Crunch Compression", false);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool("Edit Crunch Compression", value);
#endif
        }
    }

    public static bool CrunchCompression
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool("Crunch Compression", false);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool("Crunch Compression", value);
#endif
        }
    }

    public static bool EditMaxTexture
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool("Edit Maximum Texture Size", false);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool("Edit Maximum Texture Size", value);
#endif
        }
    }



    public static int MaxTextureSize
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetInt("Maximum Texture Size", 3);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetInt("Maximum Texture Size", value);
#endif
        }
    }

    public static bool EditMipmaps
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool("Edit Mipmaps", false);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool("Edit Mipmaps", value);
#endif
        }
    }

    public static bool Mipmaps
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool("Mipmaps", false);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool("Mipmaps", value);
#endif
        }
    }


    [MenuItem("Tools/Global Asset Editor")]
    public static void ShowWindow()
    {
        GetWindow<GlobalAssetEditor>(false, "Global Asset Editor", true);
        MaxTextureSize = 2048;

    }

    private static void EditTextures(bool editCrunchCompression, bool crunchCompression, 
            bool editMaxTexture , int maxTextureSize, bool editMipmaps, bool Mipmaps)
    {
        // Find all assets of Texture Type
        string[] allTextureAssets = AssetDatabase.FindAssets("t:Texture", null);

        //Tracks Progress, current item worked on
        int count = 0;


        foreach (string textureAsset in allTextureAssets)
        {
            count++;

            string assetPath = AssetDatabase.GUIDToAssetPath(textureAsset);
           
            try
            {
                TextureImporter tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (tImporter == null)
                {
                    //This is not a texture move to next item
                    Debug.LogWarning("Skipping Item: " + assetPath + " not a Texture");
                    continue;
                    
                }

                //Skip Texture Cube
                if (tImporter.textureShape == TextureImporterShape.TextureCube)
                {
                    Debug.LogWarning("Skipping Item: " + assetPath + " Texture shape is Cube");
                    continue;
                }
                
         
                if (editMipmaps)
                {
                    tImporter.mipmapEnabled = Mipmaps;
                }

                if (editMaxTexture)
                {
                    tImporter.maxTextureSize = maxTextureSize;
                }

                if (editCrunchCompression)
                {
                    tImporter.crunchedCompression = crunchCompression;
                }

                AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

                Debug.Log("Done Texture: " + assetPath + " " + count + "/" +allTextureAssets.Length);
               
            }
            catch (Exception e)
            {
                Debug.LogError(assetPath + " " + e.Message + " ");
                throw;
            }

        }
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Textures");
        EditCrunchCompression = EditorGUILayout.Toggle("Edit Crunch Compression", EditCrunchCompression);

        if (EditCrunchCompression)
        {
            CrunchCompression = EditorGUILayout.Toggle("Crunch Compression", CrunchCompression);
        }

        EditMaxTexture = EditorGUILayout.Toggle("Edit Maximum Texture Size", EditMaxTexture);

        if (EditMaxTexture)
        {
            MaxTextureSize = EditorGUILayout.IntField("Maximum Texture Size", MaxTextureSize);
        }

        EditMipmaps = EditorGUILayout.Toggle("Edit Mipmaps", EditMipmaps);

        if (EditMipmaps)
        {
            Mipmaps = EditorGUILayout.Toggle("Mipmaps", Mipmaps);
        }

        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("Edit All Textures"))
        {
            if (EditorUtility.DisplayDialog("Edit All Textures?",
                "Edit all Textures with selected options? This is NOT reversible and can take a long time depending on the project size!", "Proceed", "Cancel"))
            {
                //Start Editing All Textures
                EditTextures(EditCrunchCompression, CrunchCompression,
                    EditMaxTexture, MaxTextureSize, EditMipmaps, Mipmaps);
            }



            
        }

    }

}