using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SecurityGuard : Enemy {
    public static event Action<Enemy, int> OnDetectedPlayer;
    [SerializeField] protected float speedAttack;
    [SerializeField] public float distancePlayerInRange;
    [SerializeField] public float distanceInAttackRange;
    [SerializeField] protected bool generateWeaponRandom = true;
    public Weapons weapons;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip soundSmg;
    [SerializeField] AudioClip soundShotgun;
    [SerializeField] AudioClip soundRevolver;

    [SerializeField] Animator animator;

    public enum EstadosGuardia {
        Idle,
        Perseguir,
        Atacar,
        Morir,
        Count
    }
    //HAGO UN ENUM DE Eventos
    public enum EventosGuardia {
        Quieto,
        EnRangoDePersecucion,
        EnRangoDeAtaque,
        FueraDeRangoDePersecucion,
        FueraDeRangoDeAtaque,
        SinVida,
        Count
    }

    public EstadosGuardia test;

    private void OnEnable() {
        SecurityGuard.OnDetectedPlayer += LisentCallAlies;
        Cientifico.OnDetectedPlayer += LisentCallAlies;
    }
    private void OnDisable() {
        Cientifico.OnDetectedPlayer -= LisentCallAlies;
        SecurityGuard.OnDetectedPlayer -= LisentCallAlies;
    }
    protected override void Start() {
        base.Start();
        if (generateWeaponRandom) {
            weapons.type = (Weapons.WeaponType)UnityEngine.Random.Range(0, weapons.GetCountWeapons());
            //Debug.Log(weapons.type);
        }
    }
    protected override void Awake() {
        base.Awake();
        // Aca defino las relaciones de estado y le hago el new al objeto FSM
        fsm = new FSM((int)EstadosGuardia.Count, (int)EventosGuardia.Count, (int)EstadosGuardia.Idle);
        fsm.SetRelations((int)EstadosGuardia.Idle, (int)EstadosGuardia.Perseguir, (int)EventosGuardia.EnRangoDePersecucion);
        fsm.SetRelations((int)EstadosGuardia.Idle, (int)EstadosGuardia.Atacar, (int)EventosGuardia.EnRangoDeAtaque);
        fsm.SetRelations((int)EstadosGuardia.Perseguir, (int)EstadosGuardia.Atacar, (int)EventosGuardia.EnRangoDeAtaque);
        fsm.SetRelations((int)EstadosGuardia.Atacar, (int)EstadosGuardia.Perseguir, (int)EventosGuardia.FueraDeRangoDeAtaque);
        fsm.SetRelations((int)EstadosGuardia.Perseguir, (int)EstadosGuardia.Idle, (int)EventosGuardia.FueraDeRangoDePersecucion);

        fsm.SetRelations((int)EstadosGuardia.Idle, (int)EstadosGuardia.Morir, (int)EventosGuardia.SinVida);
        fsm.SetRelations((int)EstadosGuardia.Perseguir, (int)EstadosGuardia.Morir, (int)EventosGuardia.SinVida);
        fsm.SetRelations((int)EstadosGuardia.Atacar, (int)EstadosGuardia.Morir, (int)EventosGuardia.SinVida);

        fsm.SetRelations((int)EstadosGuardia.Perseguir, (int)EstadosGuardia.Idle, (int)EventosGuardia.Quieto);
        fsm.SetRelations((int)EstadosGuardia.Atacar, (int)EstadosGuardia.Idle, (int)EventosGuardia.Quieto);
    }
    // Update is called once per frame
    protected override void Update() {
        //if (currentTarget == null) return;

        base.Update();
        switch (fsm.GetCurrentState()) {
            case (int)EstadosGuardia.Idle:
                StopAIPathDestination();
                callAlies = true;
                if (weapons.type == Weapons.WeaponType.subMachineGun || weapons.type == Weapons.WeaponType.Shotgun)
                    StartIdleSMG();
                else
                    StartIdlePistol();

                break;
            case (int)EstadosGuardia.Perseguir:
                StartAIPathDestination();

                if (weapons.type == Weapons.WeaponType.subMachineGun || weapons.type == Weapons.WeaponType.Shotgun)
                    StartRunSMG ();
                else
                    StartRunPistol();

                if (callAlies && enableCallAlies) {
                    if (OnDetectedPlayer != null)
                        OnDetectedPlayer(this, (int)EstadosGuardia.Perseguir);
                    callAlies = false;
                }
                break;
            case (int)EstadosGuardia.Atacar:

                if (weapons.type == Weapons.WeaponType.subMachineGun || weapons.type == Weapons.WeaponType.Shotgun)
                    StartAttackSMG();
                else
                    StartAttackPistol();

                if (currentTarget != null) {
                    callAlies = true;
                    aiPath.maxSpeed = speedAttack;
                    Attack();
                    Vector2 dir = new Vector2(currentTarget.position.x - transform.position.x, currentTarget.position.y - transform.position.y);
                    transform.up = dir;
                }
                break;
            case (int)EstadosGuardia.Morir:
                callAlies = false;
                break;
        }

        CheckPlayerInRangePerseguir();
        CheckPlayerInRangeAttack();
        test = (EstadosGuardia)fsm.GetCurrentState();
    }

    void StartIdleSMG() {
        animator.SetBool("Attack_Pistol", false);
        animator.SetBool("Idle_Pistol", true);
        animator.SetBool("Run_Pistol", false);
    }
    void StartIdlePistol() {
        animator.SetBool("Attack_Pistol", false);
        animator.SetBool("Idle_Pistol", true);
        animator.SetBool("Run_Pistol", false);
    }
    void StartAttackSMG() {
        animator.SetBool("Attack_Pistol", true);
        animator.SetBool("Idle_Pistol", false);
        animator.SetBool("Run_Pistol", false);
    }
    void StartAttackPistol() {
        animator.SetBool("Attack_Pistol", true);
        animator.SetBool("Idle_Pistol", false);
        animator.SetBool("Run_Pistol", false);
    }
    void StartRunSMG() {
        animator.SetBool("Attack_Pistol", false);
        animator.SetBool("Idle_Pistol", false);
        animator.SetBool("Run_Pistol", true);
    }
    void StartRunPistol() {
        animator.SetBool("Attack_Pistol", false);
        animator.SetBool("Idle_Pistol", false);
        animator.SetBool("Run_Pistol", true);
    }










    public void CheckPlayerInRangePerseguir() {
        Vector3 currentDistance = Vector3.zero;
        if (currentTarget != null) {
            //Debug.Log("A BUSCAR UWU");
            currentDistance = transform.position - currentTarget.position;
            if (currentDistance.magnitude <= distancePlayerInRange && currentDistance.magnitude > distanceInAttackRange) {
                fsm.SendEvent((int)EventosGuardia.EnRangoDePersecucion);
            }
            /*else if (currentDistance.magnitude > distancePlayerInRange)
            {
                fsm.SendEvent((int)EventosGuardia.FueraDeRangoDePersecucion);
            }*/
        }
    }
    public void CheckPlayerInRangeAttack() {
        Vector3 currentDistance = Vector3.zero;
        if (currentTarget != null) {
            currentDistance = transform.position - currentTarget.position;
            if (currentDistance.magnitude <= distanceInAttackRange) {
                fsm.SendEvent((int)EventosGuardia.EnRangoDeAtaque);
            }
            else {
                fsm.SendEvent((int)EventosGuardia.FueraDeRangoDeAtaque);
            }
        }
    }
    public void LisentCallAlies(Enemy e, int state) {
        if (e == null || e == this) return;

        fsm.SendEvent(state);
    }
    protected override void Attack() {
        base.Attack();
        switch (weapons.type) {
            case Weapons.WeaponType.Revolver:
                if (weapons.GetCanShoot())
                    source.PlayOneShot(soundRevolver,0.225f);
                weapons.ShootRevolver(Vector3.zero);
                break;
            case Weapons.WeaponType.Shotgun:
                if (weapons.GetCanShoot())
                    source.PlayOneShot(soundShotgun,0.225f);
                weapons.ShootShotgun(Vector3.zero);
                break;
            case Weapons.WeaponType.subMachineGun:
                if (weapons.GetCanShoot())
                    source.PlayOneShot(soundSmg,0.125f);
                weapons.ShootSubmachineGun(Vector3.zero);
                break;
        }
    }
}
