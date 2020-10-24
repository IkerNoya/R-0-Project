using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    // Start is called before the first frame update
    public static LevelData instaceLevelData;

    public GameObject[] gameObjectLevels;

    private void Awake()
    {
        if (instaceLevelData == null)
        {
            instaceLevelData = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
