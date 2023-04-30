#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Resets scriptable objects once out of playmode.
/// </summary>
/// Notes:
/// 

[InitializeOnLoad]
public static class ScriptableObjectProtector
{
    private static bool _playing = false;

    static ScriptableObjectProtector()
    {
        EditorApplication.update += OnUpdate;
    }

    private static void OnUpdate()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode && !_playing)
        {
            foreach (var scriptableObject in FindScriptableObjectsByType())
            {
                EditorUtility.SetDirty(scriptableObject);
                Undo.RegisterCompleteObjectUndo(scriptableObject, scriptableObject.name);
            }
            _playing = true;
        }

        if (!EditorApplication.isPlayingOrWillChangePlaymode && _playing)
        {
            //Debug.Log("Revering ScriptableObjects");
            _playing = false;
            Undo.PerformUndo();
        }
    }

    private static List<ScriptableObject> FindScriptableObjectsByType()
    {
        var assets = new List<ScriptableObject>();
        var guids = AssetDatabase.FindAssets($"t:{typeof(ScriptableObject)}");
        foreach (var t in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(t);
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }

        return assets;
    }
}
#endif