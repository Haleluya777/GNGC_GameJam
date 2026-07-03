using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModule : ScriptableObject
{
    [SerializeField] private SkillNodeGraph nodeGraph;
    [SerializeField] private float coolDown;
}
