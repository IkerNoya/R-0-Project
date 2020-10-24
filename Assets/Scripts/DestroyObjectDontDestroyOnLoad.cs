using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectDontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        MyDontDestroyOnLoad go = FindObjectOfType<MyDontDestroyOnLoad>();
        if (go != null)
            Destroy(go.gameObject);
    }
}
