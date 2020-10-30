using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelManager : MonoBehaviour {
    public static LevelManager instanceLevelManager;
    [SerializeField] GameObject[] levels;
    [SerializeField] bool proceduralGeneration;
    [SerializeField] ProceduralGeneratorLevel proceduralGeneratorLevel;
    GameManager gm;
    int currentLevelBackground = 1;
    int indexLevel = 1;
    int currenLevel = 1;
    int countLevelsDone = 0;

    public static event Action<LevelManager> ChangedLevel;

    [SerializeField] AstarPath paths;

    [SerializeField] Boss boss;
    [SerializeField] PlayerController player;

    [SerializeField] List<GameObject> doorsLvl1;
    [SerializeField] List<GameObject> doorsLvl2;
    [SerializeField] List<GameObject> doorsLvl3;

    bool bossDead = false;

    private void OnEnable() {
        CleanLevel.OnClearLevel += CheckNextLevel;
        PlayerController.DoorEnter += ChangeLevel;
        Boss.DeadBoss += OpenDoors;
        Boss.DeadBoss += DeadBoss;
    }
    private void OnDisable() {
        Boss.DeadBoss -= DeadBoss;
        Boss.DeadBoss -= OpenDoors;
        CleanLevel.OnClearLevel -= CheckNextLevel;
        PlayerController.DoorEnter -= ChangeLevel;
    }
    private void Awake()
    {
        if (instanceLevelManager == null)
        {
            instanceLevelManager = this;
        }
        else if (instanceLevelManager != null)
        {
            if (instanceLevelManager != this)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Start() {
        player = FindObjectOfType<PlayerController>();
        gm = GameManager.instanceGM;
        //for (int i = 0; i < doors.Length; i++)
        //    if (doors[i] != null)
        //        doors[i].SetActive(false);
        //
        //int doorToOpen = Random.Range(0, doors.Length);
        //if (doors[doorToOpen] != null)
        //    doors[doorToOpen].SetActive(true);
        //

        for (int i = 0; i < levels.Length; i++)
            if (levels[i] != null)
                levels[i].SetActive(false);

        if (levels[currentLevelBackground] != null) {
            levels[currentLevelBackground].SetActive(true);
        }

        paths.Scan();

        for (int i = 0; i < doorsLvl1.Count; i++)
            if (doorsLvl1[i] != null)
                doorsLvl1[i].SetActive(false);
        for (int i = 0; i < doorsLvl2.Count; i++)
            if (doorsLvl2[i] != null)
                doorsLvl2[i].SetActive(false);

        for (int i = 0; i < doorsLvl3.Count; i++)
            if (doorsLvl3[i] != null)
                doorsLvl3[i].SetActive(false);


        if (currentLevelBackground == 1)
            player.transform.position = doorsLvl1[UnityEngine.Random.Range(0, doorsLvl1.Count)].transform.position;
        else if (currentLevelBackground == 2)
            player.transform.position = doorsLvl2[UnityEngine.Random.Range(0, doorsLvl2.Count)].transform.position;
        else if (currentLevelBackground == 3)
            player.transform.position = doorsLvl3[UnityEngine.Random.Range(0, doorsLvl3.Count)].transform.position;
    }


    public void CheckNextLevel() {
        if (gm != null) {
            if (gm.GetCurrentCountEnemy() <= 0 && gm.GetEnableCheckNextLevel()) {
                OpenDoors();
                gm.SetEnableCheckNextLevel(false);
            }
        }
    }

    public void CheckNextLevel(CleanLevel cleanLevel) {
        if (cleanLevel != null) {
            if (gm != null) {
                if (gm.GetCurrentCountEnemy() <= 0 && gm.GetEnableCheckNextLevel()) {
                    if (currentLevelBackground != 3 || (currentLevelBackground == 3 && bossDead))
                        OpenDoors();
                    gm.SetEnableCheckNextLevel(false);
                }
            }
        }
    }
    void DeadBoss() {
        bossDead = true;
    }
    void OpenDoors() {
        if (gm.GetCurrentCountEnemy() <= 0) {
            if (currentLevelBackground == 1) {
                for (int i = 0; i < doorsLvl1.Count; i++)
                    if (doorsLvl1[i] != null)
                        doorsLvl1[i].SetActive(true);
            }
            else if (currentLevelBackground == 2) {
                for (int i = 0; i < doorsLvl2.Count; i++)
                    if (doorsLvl2[i] != null)
                        doorsLvl2[i].SetActive(true);
            }
            else if (currentLevelBackground == 3) {
                for (int i = 0; i < doorsLvl3.Count; i++)
                    if (doorsLvl3[i] != null)
                        doorsLvl3[i].SetActive(true);
            }
        }
    }
    //ACA CAMBIO DE NIVEL (CHEKEAR ACA PARA HACER EL CAMBIO DE NIVEL PROSEDURAL)
    public void ChangeLevel() {

        currentLevelBackground++;
        if (currentLevelBackground > 3)
            currentLevelBackground = 1;
        StartCoroutine(Change());

        if (currenLevel < levels.Length)
        {
            if (proceduralGeneration)
            {
                indexLevel = proceduralGeneratorLevel.GenerateLevel() + 1;
                currentLevelBackground = indexLevel;
            }
            else
                indexLevel = currentLevelBackground;
        }
        else if (currenLevel >= levels.Length)
        {
            indexLevel = levels.Length - 1;
            if (currenLevel >= levels.Length + 1)
            {
                indexLevel = 1;
                currenLevel = 1;
            }
        }

        currenLevel++;
        countLevelsDone++;
       
        
        
        // int doorToOpen = Random.Range(0, doors.Length);
        // while (doors[doorToOpen] == door.gameObject)
        //     doorToOpen = Random.Range(0, doors.Length);
        //
        // if (doors[doorToOpen] != null)
        //     doors[doorToOpen].SetActive(true);

    }
    IEnumerator Change() {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        bossDead = false;
        for (int i = 0; i < levels.Length; i++)
            if (levels[i] != null)
                levels[i].SetActive(false);

        if (levels[indexLevel] != null)
            levels[indexLevel].SetActive(true);

        if (currentLevelBackground == 1) {
            for (int i = 0; i < doorsLvl1.Count; i++)
                if (doorsLvl1[i] != null)
                    doorsLvl1[i].SetActive(false);
        }
        else if (currentLevelBackground == 2) {
            for (int i = 0; i < doorsLvl2.Count; i++)
                if (doorsLvl2[i] != null)
                    doorsLvl2[i].SetActive(false);
        }
        else if (currentLevelBackground == 3) {
            for (int i = 0; i < doorsLvl3.Count; i++)
                if (doorsLvl3[i] != null)
                    doorsLvl3[i].SetActive(false);
        }

        if (currentLevelBackground == 1)
            player.transform.position = doorsLvl1[UnityEngine.Random.Range(0, doorsLvl1.Count)].transform.position;
        else if (currentLevelBackground == 2)
            player.transform.position = doorsLvl2[UnityEngine.Random.Range(0, doorsLvl2.Count)].transform.position;
        else if (currentLevelBackground == 3)
            player.transform.position = doorsLvl3[UnityEngine.Random.Range(0, doorsLvl3.Count)].transform.position;


        paths.Scan();

        if (ChangedLevel != null)
            ChangedLevel(this);

        if (currentLevelBackground == 3) {
            SpawnBoss();
        }

        StopCoroutine(Change());
    }

    void SpawnBoss() {
        Boss b = Instantiate(boss, new Vector3(0, 0, 0), Quaternion.identity, levels[currentLevelBackground].transform);
        b.gameObject.SetActive(true);
    }

    public int GetCurrentLevel() {
        return currentLevelBackground;
    }

    public GameObject[] GetLevels() {
        return levels;
    }
}
