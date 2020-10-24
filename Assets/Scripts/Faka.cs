using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faka : MonoBehaviour
{
    private float damage = 0;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.SetHP(player.GetHP() - damage);
            player.CheckDie();
        }
    }

    public void SetDamage(float _damage) { damage = _damage; }

    public float GetDamage() { return damage; }
}
