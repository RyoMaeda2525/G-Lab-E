using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerEvent))]
public class EditorForTriggerEvent : Editor
{
    /// <summary>true : 説明文表示部は折りたたまれている</summary>
    bool _ExplainFolder = true;

    public override void OnInspectorGUI()
    {
        //TriggerEvent component = target as TriggerEvent;
        _ExplainFolder = EditorGUILayout.Foldout(_ExplainFolder, "使い方");

        if (_ExplainFolder)
        {
            EditorGUILayout.HelpBox(
                "TagNameTriggerTarget に登録したタグのオブジェクトがこのオブジェクトの Collider に接触すると\n" +
                "登録したメソッドを実行する機能を提供するコンポーネント\n\n" +
                "実行タイミング別に以下の3つのいずれかにメソッドをアサインする\n" +
                "EnterToRunEvent : Colliderに触れた直後に1度だけ実行する 離れてからまた接触すると1度だけ実行する\n" +
                "StayToRunEvent : Colliderに触れている間は何度も実行する\n" +
                "ExitToRunEvent : Colliderから離れた直後に1度だけ実行する また接触してから離れると1度だけ実行する\n\n" +
                "使う際には Collider の IsKinematic を true にすること\n" +
                "Timeline を流すためのトリガー、コースアウトしたときにプレイヤーを戻すなどの活用を想定"
                , MessageType.Info, true);
        }

        base.OnInspectorGUI();
    }
}
