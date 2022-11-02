using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    /// <summary>Timelineを再生させるコンポーネント</summary>
    PlayableDirector _TimelineRunner = default;

    [SerializeField, Tooltip("Scene開始直後に再生するもの")]
    ActivalContents _ForIntroduction = new ActivalContents();

    [SerializeField, Tooltip("Scene中間に再生するもの(先頭から順番に再生)")]
    ActivalContents[] _ForInserts = default;

    [SerializeField, Tooltip("Scene終了直前に再生するもの")]
    ActivalContents _ForEnding = new ActivalContents();

    [SerializeField, Tooltip("true : Scene終了向けTimelineが再生された")]
    static bool _IsSceneEndTimelineFinished = false;

    /// <summary>true : Scene終了向けTimelineが再生された</summary>
    public static bool IsSceneEndTimelineFinished { get => _IsSceneEndTimelineFinished; }


    // Start is called before the first frame update
    void Start()
    {
        _TimelineRunner = GetComponent<PlayableDirector>();

        _TimelineRunner.playableAsset = _ForIntroduction._UseTimeline;
        _TimelineRunner.played += _ForIntroduction.OnTimelinePlayed;
        _TimelineRunner.stopped += _ForIntroduction.OnTimelineStopped;
        _TimelineRunner.Play();

        _IsSceneEndTimelineFinished = false;
    }

    public void RunEventTimeline(int index = 0)
    {
        if(_ForInserts != null && _ForInserts.Length > index)
        {
            _TimelineRunner.playableAsset = _ForInserts[index]._UseTimeline;
            _TimelineRunner.played += _ForInserts[index].OnTimelinePlayed;
            _TimelineRunner.stopped += _ForInserts[index].OnTimelineStopped;
            _TimelineRunner.Play();
        }
    }

    public void RunSceneEndTimeline()
    {
        _TimelineRunner.playableAsset = _ForEnding._UseTimeline;
        _TimelineRunner.played += _ForEnding.OnTimelinePlayed;
        _TimelineRunner.stopped += _ForEnding.OnTimelineStopped;
        _TimelineRunner.stopped += pd => _IsSceneEndTimelineFinished = true;
        _TimelineRunner.Play();
    }


    /// <summary>TimelineとTimeline実行・終了時にActive・非Activeにするオブジェクトをそれぞれ登録するオブジェクトを格納するクラス</summary>
    [Serializable]
    class ActivalContents
    {
        [SerializeField, Tooltip("該当Timeline")]
        public PlayableAsset _UseTimeline;

        [SerializeField, Tooltip("Timeline開始時に実行するメソッドをアサイン")]
        public UnityEvent _ForStartAction;

        [SerializeField, Tooltip("Timeline終了時に実行するメソッドをアサイン")]
        public UnityEvent _ForEndAction;

        /// <summary>PlayableDirectorのplayedに登録するAction</summary>
        public void OnTimelinePlayed(PlayableDirector pd)
        {
            _ForStartAction.Invoke();
        }

        /// <summary>PlayableDirectorのstoppedに登録するAction</summary>
        public void OnTimelineStopped(PlayableDirector pd)
        {
            _ForEndAction.Invoke();

            //Actionから除外
            pd.played -= OnTimelinePlayed;
            pd.stopped -= OnTimelineStopped;
        }
    }
}
