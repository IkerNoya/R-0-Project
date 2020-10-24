using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float stamina;
    [SerializeField] protected float damageAmmount;
    [SerializeField] protected float speed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region SETTERS/GETTERS
    public void SetHP(float value)
    {
        hp = value;
    }
    public float GetHP()
    {
        return hp;
    }
    public void SetStamina(float value)
    {
        stamina = value;
    }
    public float GetStamina()
    {
        return stamina;
    }
    public void Setdamage(float value)
    {
        damageAmmount = value;
    }
    public float GetDamage()
    {
        return damageAmmount;
    }

    #endregion
}
