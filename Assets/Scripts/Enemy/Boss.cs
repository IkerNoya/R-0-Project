using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : MonoBehaviour {
    [SerializeField] float hp;
    [SerializeField] float speed;
    bool attacking = false;
    [SerializeField] PlayerController player;
    [SerializeField] float timeStopped;

    [SerializeField] Vector3 posToAttack;

    [SerializeField] float damageLaser;
    [SerializeField] float damageHit;
    [SerializeField] BossLaserSphere laserSphere;
    [SerializeField] int maxLaserSpheresToShoot;

    public Transform target;
    float timer;

    [SerializeField] GameObject[] doorsToUse;
    [SerializeField] Enemy[] enemies;
    [SerializeField] SecurityGuard sg;
    [SerializeField] Cientifico cfc;
    [SerializeField] int enemiesToCreate;
    [SerializeField] Transform enemyParent;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip soundLaser;
    [SerializeField] AudioClip soundDash;
    [SerializeField] AudioClip soundDead;


    public delegate void BossDead();
    public static event BossDead DeadBoss;

    void Start() {
        player = FindObjectOfType<PlayerController>();
        target = player.transform;
        enemyParent = GameObject.FindGameObjectWithTag("EnemiesParent").transform;
        doorsToUse = GameObject.FindGameObjectsWithTag("XDoor");
    }

    private void OnEnable() {
        StartCoroutine(LateStart());
    }
    void SpawnEnemies(float timeLimit, ref float time) {
        if (time >= timeLimit) {
            SecurityGuard e1 = Instantiate(sg, doorsToUse[0].transform.position, Quaternion.identity, enemyParent);
            e1.distancePlayerInRange = 1000;
            e1.fsm.SendEvent((int)SecurityGuard.EventosGuardia.EnRangoDeAtaque);

            SecurityGuard e2 = Instantiate(sg, doorsToUse[1].transform.position, Quaternion.identity, enemyParent);
            e1.distancePlayerInRange = 1000;
            e1.fsm.SendEvent((int)SecurityGuard.EventosGuardia.EnRangoDeAtaque);
            timer = 0;
        }
    }
    IEnumerator LateStart() {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(PrepareAttack());
        StopCoroutine(LateStart());
        yield return null;
    }

    private void OnDisable() {
        StopCoroutine(PrepareAttack());
        StopCoroutine(Attack());
    }

    private void Update() {
        if (player == null) {
            transform.Rotate(Vector3.forward * 180f * Time.deltaTime, Space.Self);
        }
        timer += Time.deltaTime;
        SpawnEnemies(10f, ref timer);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!attacking)
            return;

        if (collision.gameObject.CompareTag("Player")) {
            if (!player.GetDash()) {
                player.ReceiveDamage(damageHit);
                attacking = false;
                if (player == null) {
                    StopCoroutine(PrepareAttack());
                    StopCoroutine(Attack());
                }
            }
        }
    }
    IEnumerator PrepareAttack() {
        attacking = false;
        yield return new WaitForSeconds(timeStopped);
        StopCoroutine(PrepareAttack());
        if (player != null) {
            posToAttack = player.transform.position;
            StartCoroutine(Attack());
        }
        yield return null;
    }
    IEnumerator Attack() {
        attacking = true;

        int attackPosibilities = UnityEngine.Random.Range(0, 100);

        if (attackPosibilities <= 25) {
            source.PlayOneShot(soundDash);
            while (transform.position != posToAttack) {
                transform.position = Vector2.MoveTowards(transform.position, posToAttack, speed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
        else {
            float timeBetweenAttacks = 0.05f;
            for (int i = 0; i < maxLaserSpheresToShoot; i++) {
                source.PlayOneShot(soundLaser, 0.125f);
                BossLaserSphere bls = Instantiate(laserSphere, transform.position, Quaternion.identity);

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        //else {
        //    float timeToSpawn = 3f;
        //    int rand = Random.Range(0, 2);
        //    Debug.Log(rand);
        //    if (rand == 0) {
        //        SecurityGuard e1 = Instantiate(sg,doorsToUse[0].transform.position,Quaternion.identity,enemyParent);
        //        e1.distancePlayerInRange = 1000;
        //        e1.fsm.SendEvent((int)SecurityGuard.EventosGuardia.EnRangoDeAtaque);
        //    }
        //    else {
        //        Cientifico e1 = Instantiate(cfc,doorsToUse[0].transform.position,Quaternion.identity,enemyParent);
        //        e1.distanceInAttackRange = 1000;
        //        e1.fsm.SendEvent((int)Cientifico.EventosGuardia.EnRangoDeAtaque);
        //    }
        //    rand = Random.Range(0, 2);

        //    if (rand == 0) {
        //        SecurityGuard e1 = Instantiate(sg,doorsToUse[1].transform.position,Quaternion.identity,enemyParent);
        //        e1.distancePlayerInRange = 1000;
        //        e1.fsm.SendEvent((int)SecurityGuard.EventosGuardia.EnRangoDeAtaque);
        //    }
        //    else {
        //        Cientifico e1 = Instantiate(cfc,doorsToUse[1].transform.position,Quaternion.identity,enemyParent);
        //        e1.distanceInAttackRange = 1000;
        //        e1.fsm.SendEvent((int)Cientifico.EventosGuardia.EnRangoDeAtaque);
        //    }


        //    Debug.Log(rand);

        //    yield return new WaitForSeconds(10);
        //}

        StopCoroutine(Attack());
        if (player != null)
            StartCoroutine(PrepareAttack());

        yield return null;
    }
    public void ReceiveDamage(float d) {
        hp -= d;
        if (hp <= 0) {
            if (DeadBoss != null)
                DeadBoss();

            source.PlayOneShot(soundDead);
            Destroy(this);
            Destroy(this.gameObject, 0.1f);
        }
    }
}
