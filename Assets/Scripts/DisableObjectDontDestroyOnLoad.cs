using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectDontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        MyDontDestroyOnLoad go = FindObjectOfType<MyDontDestroyOnLoad>();
        if (go != null)
            go.gameObject.SetActive(false);
    }
}
