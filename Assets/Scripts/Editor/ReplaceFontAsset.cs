using UnityEditor;
using UnityEngine;
using TMPro;

public class ReplaceFontAsset : EditorWindow
{
    [MenuItem("Tools/Replace TMP Font")]
    static void ReplaceFont()
    {
        TMP_FontAsset newFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/Fonts/NotoSans_Multi.asset");

        foreach (var tmp in Resources.FindObjectsOfTypeAll<TMP_Text>())
        {
            tmp.font = newFont;
            EditorUtility.SetDirty(tmp);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Font ersetzt!");
    }
}