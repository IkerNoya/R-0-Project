using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : Character {
    #region VARIABLES
    [SerializeField] float dashSpeed;
    [SerializeField] CameraShake screenShake;
    [SerializeField] GameObject hitCollider;
    [SerializeField] GameObject shotgun;
    [SerializeField] GameObject smg;
    [SerializeField] GameObject revolver;
    [SerializeField] float shakeMagnitude = 0.05f;
    [SerializeField] float shakeDuration = 0.2f;
    [SerializeField] string playerInputHorizontal;
    [SerializeField] string playerInputVertical;
    [SerializeField] GameObject upperWall;
    [SerializeField] GameObject lowerWall;
    [HideInInspector] public Vector3 lastMousePosition;
    public static event Action<PlayerController> OnDiePlayer;
    Vector3 mousePosition;
    Vector3 movement;
    SpriteRenderer spriteRenderer;
    SpriteRenderer shotgunSpriteRenderer;
    SpriteRenderer smgSpriteRenderer;
    SpriteRenderer revolverSpriteRenderer;
    SpriteRenderer upperWallRenderer;
    SpriteRenderer lowerWallRenderer;
    Weapons weapons;
    Rigidbody2D rb;

    [SerializeField] GameObject[] cannonPos;
    [SerializeField] AudioSource source;
    enum WeaponSelected {
        SMG, Shotgun, Revolver
    }
    WeaponSelected selection;
    bool aimingRight = false;
    bool aimingLeft = false;

    bool ActivateDash = false;
    bool canActivateDash = true;

    [SerializeField] PlayerAnims playerAnims;

    [SerializeField] AudioClip soundDash;
    [SerializeField] AudioClip soundShootSMG;
    [SerializeField] AudioClip soundShootRevolver;
    [SerializeField] AudioClip soundShootShotgun;
    [SerializeField] AudioClip soundDead;

    public delegate void EnterDoor();
    public static event EnterDoor DoorEnter;
    #endregion

    #region BASE_FUNCTIONS
    void Start() {
        ChangedLevel(null);
        LevelManager.ChangedLevel += ChangedLevel;
        weapons = GetComponent<Weapons>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        screenShake = FindObjectOfType<CameraShake>();
        rb = GetComponent<Rigidbody2D>();
        shotgunSpriteRenderer = shotgun.GetComponent<SpriteRenderer>();
        smgSpriteRenderer = smg.GetComponent<SpriteRenderer>();
        revolverSpriteRenderer = revolver.GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }
    private void OnDisable() {
        LevelManager.ChangedLevel -= ChangedLevel;
    }
    void Update() {
        if (!ActivateDash) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movement = new Vector2(Input.GetAxis(playerInputHorizontal), Input.GetAxis(playerInputVertical)) * speed;
            Vector2 dir = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            switch (selection) {
                case WeaponSelected.Shotgun:
                    shotgun.SetActive(true);
                    smg.SetActive(false);
                    revolver.SetActive(false);
                    if (mousePosition.x > transform.position.x) {
                        shotgunSpriteRenderer.flipX = false;

                        aimingRight = true;
                        aimingLeft = false;
                        if (cannonPos[0] != null)
                            if (!cannonPos[0].activeSelf)
                                cannonPos[0].SetActive(true);
                        if (cannonPos[1] != null)
                            if (cannonPos[1].activeSelf)
                                cannonPos[1].SetActive(false);

                        shotgun.transform.right = dir;
                    }
                    else if (mousePosition.x < transform.position.x) {
                        shotgunSpriteRenderer.flipX = true;

                        aimingRight = false;
                        aimingLeft = true;
                        if (cannonPos[0] != null)
                            if (cannonPos[0].activeSelf)
                                cannonPos[0].SetActive(false);
                        if (cannonPos[1] != null)
                            if (!cannonPos[1].activeSelf)
                                cannonPos[1].SetActive(true);

                        shotgun.transform.right = -dir;
                    }
                    break;
                case WeaponSelected.SMG:
                    shotgun.SetActive(false);
                    smg.SetActive(true);
                    revolver.SetActive(false);
                    if (mousePosition.x > transform.position.x) {
                        smgSpriteRenderer.flipX = false;

                        aimingRight = true;
                        aimingLeft = false;
                        if (cannonPos[2] != null)
                            if (!cannonPos[2].activeSelf)
                                cannonPos[2].SetActive(true);
                        if (cannonPos[3] != null)
                            if (cannonPos[3].activeSelf)
                                cannonPos[3].SetActive(false);

                        smg.transform.right = dir;
                    }
                    else if (mousePosition.x < transform.position.x) {
                        smgSpriteRenderer.flipX = true;

                        aimingRight = false;
                        aimingLeft = true;
                        if (cannonPos[2] != null)
                            if (cannonPos[2].activeSelf)
                                cannonPos[2].SetActive(false);
                        if (cannonPos[3] != null)
                            if (!cannonPos[3].activeSelf)
                                cannonPos[3].SetActive(true);

                        smg.transform.right = -dir;
                    }
                    break;
                case WeaponSelected.Revolver:
                    shotgun.SetActive(false);
                    smg.SetActive(false);
                    revolver.SetActive(true);
                    if (mousePosition.x > transform.position.x) {
                        revolverSpriteRenderer.flipX = false;

                        aimingRight = true;
                        aimingLeft = false;
                        if (cannonPos[4] != null)
                            if (!cannonPos[4].activeSelf)
                                cannonPos[4].SetActive(true);
                        if (cannonPos[5] != null)
                            if (cannonPos[5].activeSelf)
                                cannonPos[5].SetActive(false);

                        revolver.transform.right = dir;
                    }
                    else if (mousePosition.x < transform.position.x) {
                        revolverSpriteRenderer.flipX = true;

                        aimingRight = false;
                        aimingLeft = true;
                        if (cannonPos[4] != null)
                            if (cannonPos[4].activeSelf)
                                cannonPos[4].SetActive(false);
                        if (cannonPos[5] != null)
                            if (!cannonPos[5].activeSelf)
                                cannonPos[5].SetActive(true);

                        revolver.transform.right = -dir;
                    }
                    break;
            }
            if (shotgun.transform.position.y > transform.position.y || smg.transform.position.y > transform.position.y || revolver.transform.position.y > transform.position.y) {
                shotgunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
                smgSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
                revolverSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            }
            else if (shotgun.transform.position.y < transform.position.y || smg.transform.position.y < transform.position.y || revolver.transform.position.y < transform.position.y) {
                shotgunSpriteRenderer.sortingOrder = 1 + spriteRenderer.sortingOrder;
                smgSpriteRenderer.sortingOrder = 1 + spriteRenderer.sortingOrder;
                revolverSpriteRenderer.sortingOrder = 1 + spriteRenderer.sortingOrder;
            }
            if (upperWall != null && lowerWall != null) {
                if (lowerWall.transform.position.y < transform.position.y) lowerWallRenderer.sortingOrder = spriteRenderer.sortingOrder + 2;
                else lowerWallRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

                if (upperWall.transform.position.y < transform.position.y) upperWallRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
                else upperWallRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            }
            Inputs();
        }

    }
    private void FixedUpdate() {
        rb.velocity = new Vector2(movement.x, movement.y);
        if (ActivateDash) {
            StartCoroutine(Dash());
            StartCoroutine(DashCooldown());
        }
    }

    void ChangedLevel(LevelManager lm) {

        upperWall = null;
        lowerWall = null;

        upperWall = GameObject.FindGameObjectWithTag("WallUp");
        lowerWall = GameObject.FindGameObjectWithTag("WallDown");

        if (upperWall != null)
            upperWallRenderer = upperWall.GetComponent<SpriteRenderer>();
        if (lowerWall != null)
            lowerWallRenderer = lowerWall.GetComponent<SpriteRenderer>();
    }

    #endregion

    #region FUNCTIONS
    public void CheckDie() {
        if (GetHP() <= 0) {
            SetHP(1);
            source.PlayOneShot(soundDead);
            if (OnDiePlayer != null)
                OnDiePlayer(this);
            SetHP(0);
        }
    }

    void Inputs() {
        if (weapons != null) {
            switch (weapons.type) {
                case Weapons.WeaponType.subMachineGun:
                    if (Input.GetMouseButton(0) && !ActivateDash) {
                        if (weapons.GetCanShoot()) {
                            source.PlayOneShot(soundShootSMG,0.15f);
                            lastMousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f) + new Vector3((float)UnityEngine.Random.Range(-1.75f, 1.75f), (float)UnityEngine.Random.Range(-1.75f, 1.75f), 0f);

                            if (aimingRight) {
                                if (cannonPos[2] != null && cannonPos[2].activeSelf)
                                    weapons.ShootSubmachineGun(cannonPos[2].transform.position);
                            }
                            else if (aimingLeft) {
                                if (cannonPos[3] != null && cannonPos[3].activeSelf)
                                    weapons.ShootSubmachineGun(cannonPos[3].transform.position);
                            }

                            if (screenShake != null)
                                StartCoroutine(screenShake.Shake(weapons.GetShakeDuration(), weapons.GetShakeMagnitude(Weapons.WeaponType.subMachineGun)));
                        }

                    }
                    break;
                case Weapons.WeaponType.Shotgun:
                    if (Input.GetMouseButtonDown(0) && !ActivateDash) {

                        if (weapons.GetCanShoot()) {
                            source.PlayOneShot(soundShootShotgun);
                            lastMousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f);

                            if (aimingRight) {
                                if (cannonPos[0] != null && cannonPos[0].activeSelf)
                                    weapons.ShootShotgun(cannonPos[0].transform.position);
                            }
                            else if (aimingLeft) {
                                if (cannonPos[1] != null && cannonPos[1].activeSelf)
                                    weapons.ShootShotgun(cannonPos[1].transform.position);
                            }

                            if (screenShake != null)
                                StartCoroutine(screenShake.Shake(weapons.GetShakeDuration(), weapons.GetShakeMagnitude(Weapons.WeaponType.Shotgun)));
                        }

                    }
                    break;
                case Weapons.WeaponType.Revolver:
                    if (Input.GetMouseButton(0) && !ActivateDash) {

                        if (weapons.GetCanShoot()) {
                            source.PlayOneShot(soundShootRevolver);
                            lastMousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f) + new Vector3((float)UnityEngine.Random.Range(-0.5f, 0.5f), (float)UnityEngine.Random.Range(-0.5f, 0.5f), 0f);

                            if (aimingRight) {
                                if (cannonPos[4] != null && cannonPos[4].activeSelf)
                                    weapons.ShootRevolver(cannonPos[4].transform.position);
                            }
                            else if (aimingLeft) {
                                if (cannonPos[5] != null && cannonPos[5].activeSelf)
                                    weapons.ShootRevolver(cannonPos[5].transform.position);
                            }

                            if (screenShake != null)
                                StartCoroutine(screenShake.Shake(weapons.GetShakeDuration(), weapons.GetShakeMagnitude(Weapons.WeaponType.Shotgun)));
                        }

                    }
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            playerAnims.StartIdleAnim();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            playerAnims.StartIdleAnim();
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.A)) {
            playerAnims.StartAnimMoveSide();
        }


        if (Input.GetKeyDown(KeyCode.D)) {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.D)) {
            playerAnims.StartAnimMoveSide();
        }


        if (Input.GetMouseButton(1)) {
            if (hitCollider != null)
                StartCoroutine(StartCollider(hitCollider));
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            weapons.type = Weapons.WeaponType.subMachineGun;
            selection = WeaponSelected.SMG;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            weapons.type = Weapons.WeaponType.Shotgun;
            selection = WeaponSelected.Shotgun;
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            weapons.type = Weapons.WeaponType.Revolver;
            selection = WeaponSelected.Revolver;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canActivateDash && movement != Vector3.zero) {
            source.PlayOneShot(soundDash);

            playerAnims.StartDashAnim();
            ActivateDash = true;
            canActivateDash = false;
        }
    }

    public void ReceiveDamage(float d) {
        hp -= d;
        if (hp <= 0)
            source.PlayOneShot(soundDead);
    }

    public bool GetDash() {
        return ActivateDash;
    }

    #endregion

    #region COROUTINES
    IEnumerator StartCollider(GameObject collider) {
        collider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        collider.SetActive(false);
    }

    IEnumerator DashCooldown() {
        yield return new WaitForSeconds(2f);
        canActivateDash = true;
    }
    IEnumerator Dash() {
        rb.velocity = movement.normalized * dashSpeed;
        yield return new WaitForSeconds(0.3f);
        ActivateDash = false;
    }
    #endregion

    #region COLLISION
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Door")) {
            if (DoorEnter != null)
                DoorEnter();
        }
        if (collision.gameObject.CompareTag("Bullet") && !ActivateDash) {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null) {
                if (bullet.GetUser() != Bullet.User.Player) {
                    Debug.Log("OUCH");
                    SetHP(GetHP() - bullet.GetDamage());
                    Destroy(bullet.gameObject);
                }
            }
            if (collision.gameObject.CompareTag("Bullet") && ActivateDash) {
                Debug.Log("Esquivado");
            }
        }
    }
    #endregion
}
