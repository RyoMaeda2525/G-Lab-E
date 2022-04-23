using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CourseOutRecover))]
public class EditorForCourseOutRecover : Editor
{
    /// <summary>true : 説明文表示部は折りたたまれている</summary>
    bool _ExplainFolder = true;

    public override void OnInspectorGUI()
    {
        //CourseOutRecover component = target as CourseOutRecover;
        _ExplainFolder = EditorGUILayout.Foldout(_ExplainFolder, "使い方");

        if (_ExplainFolder)
        {
            EditorGUILayout.HelpBox(
                "TriggerEventコンポーネントと組み合わせ、コースから外れたオブジェクトをコース上に復帰させるコンポーネント\n" +
                "コースアウトとみなされる場所に TriggerEvent に紐づく Collider を配置し\n" +
                "TriggerEvent で このコンポーネントの DoRecoverメソッドを呼び出させることで\n" +
                "このコンポーネントの RecoverPoint に復帰させる機能を提供する\n\n" +
                "DoBlackoutMethod と ReturnFromBlackoutMethod は、復帰の際に暗転処理を入れたい場合に利用するオプション\n" +
                "DoBlackoutMethod : 暗転させる時に実行するメソッドをアサイン（暗転アニメーションなど）\n" +
                "ReturnFromBlackoutMethod : 暗転から明転させる時に実行するメソッドをアサイン"
                , MessageType.Info, true);
        }

        base.OnInspectorGUI();
    }
}
