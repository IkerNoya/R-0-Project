using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserSphere : MonoBehaviour {
    [SerializeField] float damage;
    [SerializeField] float speed;
    Boss boss;
    Vector3 direction;
    private void Start() {
        Destroy(this.gameObject, 1.5f);
        StartCoroutine(LateStart());
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        direction = boss.target.position - transform.position;
    }
    private void Update() {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (!collision.gameObject.GetComponent<PlayerController>().GetDash()) {
                collision.gameObject.GetComponent<PlayerController>().ReceiveDamage(damage);
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Walls")) {
            Destroy(gameObject);
        }
    }

    IEnumerator LateStart() {
        yield return new WaitForEndOfFrame();
        StopCoroutine(LateStart());
    }

}
