using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevelData : MonoBehaviour
{
    void Start()
    {
        LevelData ld = FindObjectOfType<LevelData>();
        for (int i = 0; i < ld.gameObjectLevels.Length; i++)
        {
            ld.gameObjectLevels[i].gameObject.SetActive(false);
        }
        ld.gameObjectLevels[0].gameObject.SetActive(true);
    }
}
