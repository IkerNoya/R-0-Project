using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
public class GenerateCorps : MonoBehaviour
{
    // Start is called before the first frame update
    class ParentClass
    {
        public GameObject parentObject;
        public string nameLevel;
    }

    [System.Serializable]
    class ObjectInstanciate
    {
        public GameObject objectOnDieCharacter;
        public bool recycled;
    }
    [SerializeField] private Character character;
    [SerializeField] private ObjectInstanciate objectBloodCharacter;
    [SerializeField] private ObjectInstanciate objectCorpCharacter;
    public static event Action<GenerateCorps, GameObject, bool> OnCorpGenerateBlood;
    public static event Action<GenerateCorps, GameObject, bool> OnCorpGenerateCorpse;
    private ParentClass parentClasses;
    [SerializeField] GameObject[] parents;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int inLevel;

    private void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        parentClasses = new ParentClass();
        SettingParent();
        if (levelManager != null) {
            inLevel = levelManager.GetCurrentLevel();
            parents = levelManager.GetLevels();
        }
    }
    void Update()
    {
        CheckGenerateCorp();

        //ZONA DE TESTEO//
        //if (Input.GetKeyDown(KeyCode.Keypad0))
        //{
        //    if (character == null) return;
        //        character.SetHP(0);
        //}
        //-------------//
    }
    public void SettingParent() {
        if(levelManager!=null)
        if (parents[levelManager.GetCurrentLevel()] != null) {
            parentClasses.parentObject = parents[levelManager.GetCurrentLevel()];
            parentClasses.nameLevel = "Level" + levelManager.GetCurrentLevel();
        }
    }
    public void CheckGenerateCorp()
    {
        if (character == null) return;

        if (character.GetHP() <= 0)
            GenerateCorp();
    }
    public void GenerateCorp()
    {
        SettingParent();
        //Debug.Log(levelManager.GetCurrentLevel());

        GameObject go = null;
        go = Instantiate(objectBloodCharacter.objectOnDieCharacter, transform.position, Quaternion.identity, parentClasses.parentObject.transform);
        if (OnCorpGenerateBlood != null)
        {
            OnCorpGenerateBlood(this, go, objectBloodCharacter.recycled);
        }
        go = Instantiate(objectCorpCharacter.objectOnDieCharacter, transform.position, Quaternion.identity, parentClasses.parentObject.transform);
        if (OnCorpGenerateCorpse != null)
        {
            //Debug.Log(objectCorpCharacter.recycled);
            OnCorpGenerateCorpse(this, go, objectCorpCharacter.recycled);
        }
        //ZONA DE TESTEO//
        //character.SetHP(100);
        Destroy(character.gameObject);
    }
}