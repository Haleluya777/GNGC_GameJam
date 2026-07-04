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

    void Awake()
    {
        Sequence sequence = DOTween.Sequence();
    }

    
}
