using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelManager : MonoBehaviour {
    [SerializeField] GameObject[] levels;

    GameManager gm;
    int actualLevel = 1;
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

        if (levels[actualLevel] != null) {
            levels[actualLevel].SetActive(true);
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


        if (actualLevel == 1)
            player.transform.position = doorsLvl1[UnityEngine.Random.Range(0, doorsLvl1.Count)].transform.position;
        else if (actualLevel == 2)
            player.transform.position = doorsLvl2[UnityEngine.Random.Range(0, doorsLvl2.Count)].transform.position;
        else if (actualLevel == 3)
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
                    if (actualLevel != 3 || (actualLevel == 3 && bossDead))
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
            if (actualLevel == 1) {
                for (int i = 0; i < doorsLvl1.Count; i++)
                    if (doorsLvl1[i] != null)
                        doorsLvl1[i].SetActive(true);
            }
            else if (actualLevel == 2) {
                for (int i = 0; i < doorsLvl2.Count; i++)
                    if (doorsLvl2[i] != null)
                        doorsLvl2[i].SetActive(true);
            }
            else if (actualLevel == 3) {
                for (int i = 0; i < doorsLvl3.Count; i++)
                    if (doorsLvl3[i] != null)
                        doorsLvl3[i].SetActive(true);
            }
        }
    }
    public void ChangeLevel() {
        //Debug.Log("ALFAJOR");
        actualLevel++;
        if (actualLevel > 3)
            actualLevel = 1;



        StartCoroutine(Change());


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

        if (levels[actualLevel] != null)
            levels[actualLevel].SetActive(true);

        if (actualLevel == 1) {
            for (int i = 0; i < doorsLvl1.Count; i++)
                if (doorsLvl1[i] != null)
                    doorsLvl1[i].SetActive(false);
        }
        else if (actualLevel == 2) {
            for (int i = 0; i < doorsLvl2.Count; i++)
                if (doorsLvl2[i] != null)
                    doorsLvl2[i].SetActive(false);
        }
        else if (actualLevel == 3) {
            for (int i = 0; i < doorsLvl3.Count; i++)
                if (doorsLvl3[i] != null)
                    doorsLvl3[i].SetActive(false);
        }

        if (actualLevel == 1)
            player.transform.position = doorsLvl1[UnityEngine.Random.Range(0, doorsLvl1.Count)].transform.position;
        else if (actualLevel == 2)
            player.transform.position = doorsLvl2[UnityEngine.Random.Range(0, doorsLvl2.Count)].transform.position;
        else if (actualLevel == 3)
            player.transform.position = doorsLvl3[UnityEngine.Random.Range(0, doorsLvl3.Count)].transform.position;


        paths.Scan();

        if (ChangedLevel != null)
            ChangedLevel(this);

        if (actualLevel == 3) {
            SpawnBoss();
        }

        StopCoroutine(Change());
    }

    void SpawnBoss() {
        Boss b = Instantiate(boss, new Vector3(0, 0, 0), Quaternion.identity, levels[actualLevel].transform);
        b.gameObject.SetActive(true);
    }

    public int GetCurrentLevel() {
        return actualLevel;
    }

    public GameObject[] GetLevels() {
        return levels;
    }
}
