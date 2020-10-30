using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTraps : MonoBehaviour
{
    // Start is called before the first frame update
    class LevelsTraps
    {
        public GameObject TrapGameObject;
        public int LevelTramp;
    }
    
    private LevelManager levelManager;
    [SerializeField] private List<LevelsTraps> tramps;
    [SerializeField] private float rangeGenerationX;
    [SerializeField] private float rangeGenerationY;
    [SerializeField] private int addMinCurrentGenerationForLevelPass = 1;
    [SerializeField] private int addMaxCurrentGenerationForLevelPass = 2;
    [SerializeField] private int minCurrentGeneration = 0;
    [SerializeField] private int maxCurrentGeneration = 0;
    [SerializeField] private int limitGeneration;
    private int currentLevelTramps = 1;


    public void AddCurrenGeneration()
    {
        if (minCurrentGeneration < limitGeneration)
        {
            minCurrentGeneration = minCurrentGeneration + addMinCurrentGenerationForLevelPass;
        }
        else
        {
            minCurrentGeneration = limitGeneration;
        }

        if (maxCurrentGeneration < limitGeneration)
        {
            maxCurrentGeneration = maxCurrentGeneration + addMaxCurrentGenerationForLevelPass;
        }
        else
        {
            maxCurrentGeneration = limitGeneration;
        }
    }
    public void GenerationTramps()
    {
        int countGenerationTramps = Random.Range(minCurrentGeneration, maxCurrentGeneration);
        bool enableIndex = false;
        int indexTramp = 0;
        do
        {
            indexTramp = Random.Range(0, tramps.Count);
            if (currentLevelTramps >= tramps[indexTramp].LevelTramp)
            {
                enableIndex = true;
            }

        } while (!enableIndex);
        for (int i = 0; i < countGenerationTramps; i++)
        {
            float x = Random.Range(-rangeGenerationX, rangeGenerationX);
            float y = Random.Range(-rangeGenerationY, rangeGenerationY);

            Vector3 position = new Vector3(x, y, 0);

            Instantiate(tramps[indexTramp].TrapGameObject, position, Quaternion.identity);
        }
    }

}
