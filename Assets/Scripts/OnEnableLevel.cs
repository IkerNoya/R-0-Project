using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class OnEnableLevel : MonoBehaviour
{
    public static event Action<OnEnableLevel, int> onEnableLevel;
    public static event Action<OnEnableLevel, int> onDisableLevel;
    public int numberLevel;
    private void OnEnable()
    {
        if (onEnableLevel != null)
            onEnableLevel(this, numberLevel);
    }
    private void OnDisable()
    {
        if (onDisableLevel != null)
            onDisableLevel(this, numberLevel);
    }
}