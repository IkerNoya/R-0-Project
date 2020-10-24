using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ProceduralGeneratorLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public enum TypeGenerationLevel
    {
        ForScene,
        ForPrefab,
    }
    public TypeGenerationLevel typeGenerationLevel = TypeGenerationLevel.ForPrefab;
    [SerializeField] private List<string> nameLevels;
    [SerializeField] private List<GameObject> prefabLevels;
    private GameObject currentPrefabLevel = null;
    private string currentNameLevel = "None";
    private const int countTried = 20;
    private int currentTried = 0;

    public int GenerateLevel()
    {
        int index = -1;
        switch (typeGenerationLevel)
        {
            case TypeGenerationLevel.ForPrefab:
                for (int i = 0; i < prefabLevels.Count; i++)
                {
                    if (prefabLevels[i].activeSelf)
                    {
                        currentPrefabLevel = prefabLevels[i];
                        i = prefabLevels.Count;
                    }
                }
                do
                {
                    index = UnityEngine.Random.Range(0, prefabLevels.Count);
                    currentTried++;

                    if (currentTried >= 20)
                    {
                        for (int i = 0; i < prefabLevels.Count; i++)
                        {
                            if (currentPrefabLevel != prefabLevels[index])
                            {
                                index = i;
                                i = prefabLevels.Count;
                            }
                        }
                        break;
                    }
                    

                } while (prefabLevels[index] == currentPrefabLevel);
                break;
            case TypeGenerationLevel.ForScene:
                currentNameLevel = SceneManager.GetActiveScene().name;
                do
                {
                    index = UnityEngine.Random.Range(0, nameLevels.Count);
                    currentTried++;

                    if (currentTried >= 20)
                    {
                        for (int i = 0; i < nameLevels.Count; i++)
                        {
                            if (currentNameLevel != nameLevels[index])
                            {
                                index = i;
                                i = prefabLevels.Count;
                            }
                        }
                        break;
                    }

                } while (nameLevels[index] == currentNameLevel);
                SceneManager.LoadScene(nameLevels[index]);
                break;
        }
        return index;
    }
}
