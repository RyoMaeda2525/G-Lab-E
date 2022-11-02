using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EchoMaterialSwitcher))]
public class EditorForEchoMaterialSwitcher : Editor
{
    /// <summary>true : 説明文表示部は折りたたまれている</summary>
    bool _ExplainFolder = true;

    public override void OnInspectorGUI()
    {
        //EchoMaterialSwitcher component = target as EchoMaterialSwitcher;
        _ExplainFolder = EditorGUILayout.Foldout(_ExplainFolder, "使い方");

        if (_ExplainFolder)
        {
            EditorGUILayout.HelpBox(
                "MeshRenderer を持ったオブジェクトにアタッチすることで、エコーロケーションの表現を促すコンポーネント\n" +
                "MaterialEcho にエコーロケーション表現をする Material をアサインする\n" +
                "あとは EcholocationController でエコーロケーションを起動すると Material が入れ替わるようになる"
                , MessageType.Info, true);
        }

        base.OnInspectorGUI();
    }
}
