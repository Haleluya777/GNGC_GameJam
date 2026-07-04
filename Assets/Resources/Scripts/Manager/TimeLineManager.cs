using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using DG.Tweening;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{
    public List<TimelineAsset> timelines;
    public PlayableDirector director;

    public bool timeLinePlaying;
    void Awake()
    {
        Sequence sequence = DOTween.Sequence();
    }

    public void TimeLineStatus(bool status)
    {
        timeLinePlaying = status;
    }
}
