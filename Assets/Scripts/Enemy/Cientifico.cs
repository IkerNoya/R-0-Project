using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Cientifico : Enemy
{
    // Start is called before the first frame update
    public static event Action<Enemy, int> OnDetectedPlayer;
    [SerializeField] public float distancePlayerInRange;
    [SerializeField] public float distanceInAttackRange;

    [SerializeField] protected float damageFakazo;
    [SerializeField] private GameObject fakaCollider;
    [SerializeField] private float delayFakazo = 0.1f;
    [SerializeField] private float delayFkazoEnable = 0.05f;
    [SerializeField] bool enableIdleStatePlayerOutRange;
    [SerializeField] Faka faka;
    private bool called;
    private float auxDelayFakazo;
    private float auxDelayFakazoEnable;

    [SerializeField] Animator animator;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip soundHit;

    public enum EstadosGuardia
    {
        Idle,
        Perseguir,
        Atacar,
        Morir,
        Count
    }
    //HAGO UN ENUM DE Eventos
    public enum EventosGuardia
    {
        Quieto,
        EnRangoDePersecucion,
        EnRangoDeAtaque,
        FueraDeRangoDePersecucion,
        FueraDeRangoDeAtaque,
        SinVida,
        Count
    }

    protected override void Awake()
    {
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

    protected override void Start()
    {
        base.Start();
        called = false;
        auxDelayFakazo = delayFakazo;
        auxDelayFakazoEnable = delayFkazoEnable;
        fakaCollider.SetActive(false);
        faka.SetDamage(damageFakazo);
        ResetTimers();
        StopAIPathDestination();
    }
    private void OnEnable()
    {
        Cientifico.OnDetectedPlayer += LisentCallAlies;
        SecurityGuard.OnDetectedPlayer += LisentCallAlies;
    }
    private void OnDisable()
    {
        Cientifico.OnDetectedPlayer -= LisentCallAlies;
        SecurityGuard.OnDetectedPlayer -= LisentCallAlies;
    }
    // Update is called once per frame
    protected override void Update()
    {
        //Debug.Log((EstadosGuardia)fsm.GetCurrentState());
        //if (currentTarget == null) return;
       
        base.Update();
        switch (fsm.GetCurrentState())
        {
            case (int)EstadosGuardia.Idle:
                StopAIPathDestination();
                StartIdle();
                callAlies = true;
                break;
            case (int)EstadosGuardia.Perseguir:
                StartAIPathDestination();
                StartRun();
                if (callAlies && enableCallAlies)
                {
                    if (OnDetectedPlayer != null)
                        OnDetectedPlayer(this, (int)EstadosGuardia.Perseguir);
                    callAlies = false;
                }
                break;
            case (int)EstadosGuardia.Atacar:
                StartAttack();
                if (currentTarget != null)
                {
                    //Debug.Log("ATAQUE");
                    callAlies = true;
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
    }


    void StartIdle() {
        animator.SetBool("Moving", false);
        animator.SetBool("Idle", true);
        animator.SetBool("Attacking", false);
    }
    void StartRun() {
        animator.SetBool("Moving", true);
        animator.SetBool("Idle", false);
        animator.SetBool("Attacking", false);
    }
    void StartAttack() {
        animator.SetBool("Moving", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Attacking", true);
    }

    public void CheckPlayerInRangePerseguir()
    {
        Vector3 currentDistance = Vector3.zero;
        if (currentTarget != null)
        {
           
            currentDistance = transform.position - currentTarget.position;
            //Debug.Log(currentDistance.magnitude);
            if ((currentDistance.magnitude <= distancePlayerInRange && currentDistance.magnitude > distanceInAttackRange || called) 
                && fsm.GetCurrentState() != (int)EstadosGuardia.Atacar)
            {
                fsm.SendEvent((int)EventosGuardia.EnRangoDePersecucion);
                //Debug.Log("ENTRE");
            }
            else if (currentDistance.magnitude > distancePlayerInRange && enableIdleStatePlayerOutRange)
            {
                fsm.SendEvent((int)EventosGuardia.FueraDeRangoDePersecucion);
            }
        }
    }
    public void CheckPlayerInRangeAttack()
    {
        Vector3 currentDistance = Vector3.zero;
        if (currentTarget != null)
        {
            currentDistance = transform.position - currentTarget.position;
            //Debug.Log(currentDistance.magnitude);
            if (currentDistance.magnitude <= distanceInAttackRange)
            {
                fsm.SendEvent((int)EventosGuardia.EnRangoDeAtaque);
            }
            else
            {
                fsm.SendEvent((int)EventosGuardia.FueraDeRangoDeAtaque);
            }
        }
    }
    public void ResetTimers()
    {
        delayFakazo = 0;
        delayFkazoEnable = auxDelayFakazoEnable;
        fakaCollider.SetActive(true);
    }
    protected override void Attack()
    {
        Debug.Log("XD");
        if (delayFakazo > 0 && !fakaCollider.activeSelf) {
            delayFakazo = delayFakazo - Time.deltaTime;

        }
        else if (delayFakazo <= 0 && !fakaCollider.activeSelf) {
            delayFkazoEnable = auxDelayFakazoEnable;
            fakaCollider.SetActive(true);
            if (!source.isPlaying)
                source.PlayOneShot(soundHit);
        }

        if (delayFkazoEnable > 0 && fakaCollider.activeSelf)
        {
            delayFkazoEnable = delayFkazoEnable - Time.deltaTime;
        }
        else if (delayFkazoEnable <= 0 && fakaCollider.activeSelf)
        {
            delayFakazo = auxDelayFakazo;
            fakaCollider.SetActive(false);
        }
    }
    public void LisentCallAlies(Enemy e, int state)
    {
        if (e == null || e == this) return;

        fsm.SendEvent(state);
        called = true;
    }
}
