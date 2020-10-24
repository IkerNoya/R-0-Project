using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemys : MonoBehaviour
{
    // Start is called before the first frame update
    public enum TypeEnemysGenerates
    {
        GenerateCientifico,
        GenerateSecurityGuard,
        GenerateAll,
    }
    private enum TypeGeneration
    {
        OneGeneration,
        Gradual,
    }
    [SerializeField] float delayGenerate;
    [SerializeField] float auxDelayGenerate;
    [HideInInspector] public int numberCurrentLevel;
    [SerializeField] bool enableGradualGeneration = false;
    [SerializeField] int countGradualGeneration;
    public GameObject securityGuard_GO;
    public GameObject cientifico_GO;
    public GameObject parentEnemys;
    public TypeEnemysGenerates typeEnemysGenerates;
    private TypeGeneration typeGeneration;

    [SerializeField] float rangePositionX;
    [SerializeField] float rangePositionY;

    private bool start;
    private int countEnemysGenerates;
    [HideInInspector]
    public int maxEnemysGenerates = 1;
    GameManager instanceGM;

    private void Awake()
    {
        instanceGM = GameManager.instanceGM;
        gameObject.SetActive(true);
        countEnemysGenerates = 0;
    }

    private void OnEnable()
    {
        OnEnableLevel.onDisableLevel += Disable;
        start = true;
    }
    private void OnDisable()
    {
        OnEnableLevel.onDisableLevel -= Disable;
        start = false;
        typeGeneration = TypeGeneration.OneGeneration;
        countEnemysGenerates = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            CheckGenerateEnemy();
        }
    }

    public void Disable(OnEnableLevel onEnableLevel, int currentLevel)
    {
        if (onEnableLevel != null)
        {
            if (numberCurrentLevel == currentLevel)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void CheckGenerateEnemy()
    {
        int count = 0;
        if (enableGradualGeneration)
            count = maxEnemysGenerates / 2;
        else
            count = maxEnemysGenerates;

        switch (typeGeneration)
        {
            case TypeGeneration.OneGeneration:
                GenerateEnemy(count);
                if (enableGradualGeneration)
                {
                    typeGeneration = TypeGeneration.Gradual;
                }
                break;
            case TypeGeneration.Gradual:
                if (delayGenerate > 0)
                {
                    delayGenerate = delayGenerate - Time.deltaTime;
                }
                else
                {
                    delayGenerate = auxDelayGenerate;
                    GenerateEnemy(countGradualGeneration);
                }
                break;

        }
    }
    void GenerateEnemy(int count)
    {
        Vector3 relativePosition = Vector3.zero;
        float porcentageGenerationCientifico = 30;
        float porcentageGenerationSecurityGuard = 70;

        for (int i = 0; i < count; i++)
        {
            if (countEnemysGenerates < maxEnemysGenerates)
            {
                //Enemy e = null;
                //GameObject go = null;
                float x = Random.Range(-rangePositionX, rangePositionX);
                float y = Random.Range(-rangePositionY, rangePositionY);
                float optionGeneration = Random.Range(0, 100);
                relativePosition = new Vector3(x, y, 0);
                switch (typeEnemysGenerates)
                {
                    case TypeEnemysGenerates.GenerateAll:
                        if (optionGeneration <= porcentageGenerationCientifico && optionGeneration >= 0)
                        {
                            Instantiate(cientifico_GO, (transform.position + relativePosition), Quaternion.identity, parentEnemys.transform);
                        }
                        else if(optionGeneration > porcentageGenerationCientifico 
                            && optionGeneration <= porcentageGenerationSecurityGuard + porcentageGenerationCientifico)
                        {
                            Instantiate(securityGuard_GO, (transform.position + relativePosition), Quaternion.identity, parentEnemys.transform);
                        }
                        break;
                    case TypeEnemysGenerates.GenerateCientifico:
                        Instantiate(cientifico_GO, (transform.position + relativePosition), Quaternion.identity);
                        break;
                    case TypeEnemysGenerates.GenerateSecurityGuard:
                        Instantiate(securityGuard_GO, (transform.position + relativePosition), Quaternion.identity, parentEnemys.transform);
                        break;
                }
                countEnemysGenerates++;
            }
        }
        if (countEnemysGenerates >= maxEnemysGenerates)
        {
            start = false;
        }
    }
}
