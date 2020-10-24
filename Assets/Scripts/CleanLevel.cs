using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CleanLevel : MonoBehaviour
{
    public static event Action<CleanLevel> OnClearLevel;
    [System.Serializable]
    public class ObjectsClean
    {
        public GameObject objectOnDieCharacter;
        public bool recycled;
    }
    // Start is called before the first frame update
    public List<ObjectsClean> objectsClean;
    void Start()
    {
        objectsClean = new List<ObjectsClean>();
    }
    private void OnEnable()
    {
        GenerateCorps.OnCorpGenerateBlood += AddListClean;
        GenerateCorps.OnCorpGenerateCorpse += AddListClean;
        SettingDestroyObjects();
    }
    public void AddListClean(GenerateCorps gc, GameObject go, bool recycled)
    {
        ObjectsClean objectClean = new ObjectsClean();
        objectClean.objectOnDieCharacter = go;
        objectClean.recycled = recycled;
        objectsClean.Add(objectClean);

        //Debug.Log(objectsClean.Count);
        if (OnClearLevel != null)
            OnClearLevel(this);

    }
    private void OnDisable()
    {
        GenerateCorps.OnCorpGenerateBlood -= AddListClean;
        GenerateCorps.OnCorpGenerateCorpse -= AddListClean;
        SettingDestroyObjects();
    }
    public void SettingDestroyObjects()
    {
        if (objectsClean == null) return;
        if (objectsClean.Count <= 0) return;

        for (int i = 0; i < objectsClean.Count; i++)
        {
            //Debug.Log("DESTRUI AL " + i);
            //Debug.Log(objectsClean[i].recycled);
            if (objectsClean != null)
            {
                if (!objectsClean[i].recycled)
                {
                    Destroy(objectsClean[i].objectOnDieCharacter);
                    //Debug.Log("DESTRUI AL " +i);
                }
            }
        }
        objectsClean.Clear();
    }
}
