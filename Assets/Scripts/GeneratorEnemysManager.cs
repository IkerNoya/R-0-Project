using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GeneratorEnemysManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int countRoundsAddRandomGenerate = 1;
    [SerializeField] int rounds;
    public int minAddedRandomGenerate = 1;
    public int maxAddedRandomGenerate = 2;
    public int minRandomGenerateEnemys = 1;
    public int maxRandomGenerateEnemys = 2;
    public GeneradorEnemys[] generadorEnemys;
    public OnEnableLevel enableLevelObject;

    bool upgrade = true;

    private void Start()
    {
        for (int i = 0; i < generadorEnemys.Length; i++)
        {
            generadorEnemys[i].numberCurrentLevel = enableLevelObject.numberLevel;
        }
    }
    private void OnEnable()
    {
        upgrade = true;
        //Enable();
    }
    private void OnDisable()
    {
        upgrade = true;
        Enable();
    }
    public void AddMaxRandomGenerate()
    {
        //Debug.Log("ENTRE");
        int resultRandom = UnityEngine.Random.Range(minAddedRandomGenerate, maxAddedRandomGenerate+1);
        //Debug.Log(resultRandom);
        minRandomGenerateEnemys = maxRandomGenerateEnemys;
        maxRandomGenerateEnemys = maxRandomGenerateEnemys + resultRandom;

        for (int i = 0; i < generadorEnemys.Length; i++)
        {
            generadorEnemys[i].maxEnemysGenerates = UnityEngine.Random.Range(minRandomGenerateEnemys, maxAddedRandomGenerate + 1);
        }
    }
    public void Enable()
    { 
        for (int i = 0; i<generadorEnemys.Length; i++)
        {
            generadorEnemys[i].gameObject.SetActive(true);
        }

        if (upgrade)
        {
            AddMaxRandomGenerate();
            upgrade = false;
        }
    }
}
