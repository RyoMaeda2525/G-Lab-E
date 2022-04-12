using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimelineManager))]
public class EditorForTimelineManager : Editor
{
    /// <summary>true : 説明文表示部は折りたたまれている</summary>
    bool _ExplainFolder = true;

    public override void OnInspectorGUI()
    {
        //TimelineManager component = target as TimelineManager;
        _ExplainFolder = EditorGUILayout.Foldout(_ExplainFolder, "使い方");

        if (_ExplainFolder)
        {
            EditorGUILayout.HelpBox(
                "各 Timeline の実行とそれに伴うメソッドの実行を促すコンポーネント\n\n" +
                "For Introduction : シーン開始直後、ゲームプレイ前に流す Timeline が対象\n" +
                "For Insert : シーンおよびゲームプレイ中の合間に流す Timeline が対象\n" +
                "For Ending : シーン終了直前、他のシーンに移動する前に流す Timeline が対象\n\n" +
                "Use Timeline には、流す Timeline をアサインする\n" +
                "For Start Action には、Timeline を流す直前に実行したいメソッドをアサインする（SetActive(false)など）\n" +
                "For End Action には、Timeline を流した直後に実行したいメソッドをアサインする（SetActive(true)など）\n\n" +
                "For Insert は size の数だけ上記の Timeline 設定をアサインできる\n" +
                "   Timeline を流す際は RunEventTimelineメソッドを TriggerEventコンポーネントなどから呼び出して使用する\n" +
                "   RunEventTimelineメソッドの引数は For Insert の Element番号\n" +
                "For Ending の Timeline の呼び出しは RunSceneEndTimelineメソッドを TriggerEvent などで呼び出す\n" +
                "   For End Action の最後に何かしらの Scene遷移メソッドをアサインすること"
                , MessageType.Info, true);
        }

        base.OnInspectorGUI();
    }
}
