using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerMovement : MonoBehaviour {

    #region variables
    private Animator anim;
    private Rigidbody rb;
    private GameObject[] WeaponTrail;
    public static bool playerdeath;
    public static float health = 100f;
    public static float max = 100f;
    public static bool facingRight;
    public bool isCrouching, isDead, wasGrounded, invincibleEscape, invincible, isShadowing = false, isGrounded, isAttacking = false, shadowPower = false, emitDashCooldown = false, isDashing = false, canDash = true, doubleJump = true, candoublejump = false;
    private float airAttackSpeed, jumpForce = 10f, dashForce = 800f, superDashForce = 310f, dashCooldown = 1f, shadowCooldown = 4f, nextShadow = 0f, nextDash = 0f, PlayerSpeed = 5.5f;
    private bool canClick, _leftTrig, _rightTrig;
    public int noOfClicks, weaponType, weaponSelected, lastWeapon;
    public float checkRbVelocityY, RightTriggerAxis, LeftTriggerAxis, move, powerupReceiver = 0;
    private static bool PowerUp_SingleSlash, PowerUp_ShadowDash;
    public AnimatorOverrideController OverridingController;
    public AnimationClip shadowDashClip;
    public AnimationClip normalDashClip;
    public AnimationClip singleSwordIdleClip;
    public AnimationClip greatSwordIdleClip;
    public AnimationClip dualSwordIdleClip;
    public AnimationClip singleSwordJumpAttackClip;
    public AnimationClip greatSwordJumpAttackClip;
    public AnimationClip dualSwordJumpAttackClip;
    public GameObject DashCD;
    public GameObject NormalAfterImage;
    public GameObject ShadowAfterImage;
    public GameObject RangedSlashEmitter;
    public GameObject RangedDarkSlash;
    public SimpleHealthBar healthBar;
    public AudioSource audioS;
    public AudioSource aura;
    public AudioClip audio_normaldash;
    public AudioClip audio_shadowdash;
    public AudioClip audio_singlesword;
    public AudioClip audio_dualsword;
    public AudioClip audio_greatsword;
    public AudioClip audio_landing;
    public CapsuleCollider feetcap;
    private GameMaster gm;
    public Renderer PlayerRenderer;
    #endregion

    private List<Collider> m_collisions = new List<Collider>();

    private void Awake() {
        WeaponTrail = GameObject.FindGameObjectsWithTag("Weapon Trail");
        health = 100f;
        max = 100f;
        aura.Pause();
        //For testing
        //PowerUp_ShadowDash = true;
        //PowerUp_SingleSlash = true;
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
        facingRight = true;
        noOfClicks = 0;
        canClick = true;
        lastWeapon = weaponType;
        anim.runtimeAnimatorController = OverridingController;
        weaponType = 0;
        weaponSelected = GameObject.Find("Weapon Holder").GetComponent<WeaponSwitching>().selectedWeapon;
        Invoke("DeactivateTrail", 0.4f);
        wasGrounded = isGrounded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { isGrounded = false; }
    }

    void Update() {
        isDead = playerdeath;
        if (!playerdeath)
        {
            if (!PauseMenu.GameIsPaused)
            {
                anim.SetBool("grounded", isGrounded);
                JumpingAndLanding();
                Dashing();
                RightTriggerAxis = Input.GetAxis("Dash");
                LeftTriggerAxis = Input.GetAxis("ShadowForm");
                RightTrigger();
                LeftTrigger();
                weaponSelected = GameObject.Find("Weapon Holder").GetComponent<WeaponSwitching>().selectedWeapon;
                WeaponAnimation();
                anim.SetFloat("AirAttackSpeed", airAttackSpeed);
                if (health > max)
                    health = max;
                //PowerUp Receiver
                if(powerupReceiver == 1)
                {
                    PowerUp_ShadowDash = true;
                }
                if (powerupReceiver == 2)
                {
                    PowerUp_SingleSlash = true;
                }
                //Character power moves
                if (Input.GetKeyDown(KeyCode.Q) && PowerUp_ShadowDash || _leftTrig && PowerUp_ShadowDash)
                {
                    if (Time.time > nextShadow)
                        shadowPower = !shadowPower;
                }
                if (Time.time < nextShadow)
                    shadowPower = false;
                if (shadowPower)
                {
                    OverridingController["Dash"] = shadowDashClip;
                    aura.UnPause();
                }            
                if (!shadowPower)
                {
                    OverridingController["Dash"] = normalDashClip;
                    aura.Pause();
                }
                    

                checkRbVelocityY = rb.velocity.y;
                if (rb.velocity.y > 17)
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                if(wasGrounded != isGrounded)
                {
                    audioS.PlayOneShot(audio_landing);
                    wasGrounded = isGrounded;
                }

                //Normal combo starter
                if (!isCrouching && isGrounded)
                {
                    if (shadowPower && weaponType != 0)
                    {
                        if (Input.GetButtonDown("Fire1"))
                            ComboStarter();
                    }     
                    else if(shadowPower && weaponType == 0)
                    {
                        if(PowerUp_SingleSlash && !anim.GetCurrentAnimatorStateInfo(0).IsName("RangedSlash"))
                            if (Input.GetButtonDown("Fire1"))
                                StartCoroutine(RangedSlash());
                        if (!PowerUp_SingleSlash)
                            if (Input.GetButtonDown("Fire1"))
                                ComboStarter();
                    }
                    else if (!shadowPower)
                    {
                        if (Input.GetButtonDown("Fire1"))
                            ComboStarter();
                    }         
                }
                if(!isGrounded)
                {
                    if (shadowPower && weaponType == 0 && PowerUp_SingleSlash)
                    {
                        if (Input.GetButtonDown("Fire1"))
                            StartCoroutine(RangedSlash());
                    }
                }

                if (invincible)
                    move = 0;

                //Vertical input and crouch attack
                float misc = Input.GetAxis("Vertical");
                if (misc < 0 && isGrounded)
                {
                    anim.SetBool("crouching", true);
                    rb.velocity = new Vector3(0, 0, 0);
                    PlayerSpeed = 0f;
                    isCrouching = true;
                }
                else if (misc >= 0 && isGrounded)
                {
                    anim.SetBool("crouching", false);
                    isCrouching = false;
                    PlayerSpeed = 5.5f;
                }
                //Crouch Attack
                if (Input.GetButtonDown("Fire1") && isCrouching)
                {
                    anim.SetTrigger("crouchattack");
                    isAttacking = true;
                }
                else if (!Input.GetButtonDown("Fire1") && isCrouching)
                    isAttacking = false;
            }
        }
        else if (playerdeath)
            rb.velocity = new Vector3(0, 0, 0);
    }

    void FixedUpdate() {
        if (!playerdeath)
        {
            if (!PauseMenu.GameIsPaused)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 1") || anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 2") || anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 3") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch Slash") || anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack2") || anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack3") || anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack2") || anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack3") || anim.GetCurrentAnimatorStateInfo(0).IsName("RangedSlash") || anim.GetCurrentAnimatorStateInfo(0).IsName("SwitchWeapons") || anim.GetCurrentAnimatorStateInfo(0).IsName("DualJumpAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("GreatSwordJumpAttack"))
                    isAttacking = true;
                else
                    isAttacking = false;
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && noOfClicks > 4 || anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && noOfClicks > 4)
                    noOfClicks = 0;
                //movement

                if (isAttacking && isGrounded)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    ActivateTrail();
                    return;
                }
                else if (isAttacking && !isGrounded)
                {
                    ActivateTrail();
                    return;
                }
                if (!playerdeath || !isAttacking)
                {
                    if (WeaponTrail != null)
                        DeactivateTrail();
                    if(!invincible || !isCrouching)
                        move = Input.GetAxis("Horizontal");
                    

                    if (!isCrouching || !isDashing)
                    {
                        if (!invincible)
                            rb.velocity = new Vector3(move * PlayerSpeed, rb.velocity.y, 0);
                        else if(invincible || isCrouching)
                            rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    }
                       
                    anim.SetFloat("Speed", Mathf.Abs(move));
                    if (!isDashing)
                    {
                        if (move > 0 && !facingRight)
                            FlipRight();
                        else if (move < 0 && facingRight)
                            FlipLeft();
                    }
                }
            }
        }
        if (playerdeath)
            return;
    }

    void CrouchSoundAtk()
    {
        if (weaponType == 0)
            audioS.PlayOneShot(audio_singlesword);
        if (weaponType == 1)
            audioS.PlayOneShot(audio_dualsword);
        if (weaponType == 2)
            audioS.PlayOneShot(audio_greatsword);
    }

    void FlipLeft() {
        facingRight = !facingRight;
        transform.eulerAngles = new Vector3(0, 270, 0);
    }
    void FlipRight() {
        facingRight = !facingRight;
        transform.eulerAngles = new Vector3(0, 90, 0);
    }
    void JumpingAndLanding() {
        //jump and doublejump
        if (Input.GetButtonDown("Jump") && !isAttacking)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (doubleJump == true)
                    candoublejump = true;
            }
            else
            {
                if (candoublejump)
                {
                    anim.Play("Jump");
                    rb.velocity = new Vector3(0, 1, 0);
                    rb.AddForce(Vector3.up * (jumpForce / 1.5f), ForceMode.Impulse);
                    candoublejump = false;
                }
            }
        }
        if (!isGrounded && Input.GetButtonDown("Fire1") && !shadowPower && PowerUp_SingleSlash || !isGrounded && Input.GetButtonDown("Fire1") && !PowerUp_SingleSlash)
        {
            isAttacking = true;
            anim.SetBool("AttackAir", isAttacking);
            Invoke("AirAttackCooldown", 0.2f);
        }
    }
    void Dashing() {
        //grounddashing and airdashing
        if (Time.time > nextDash)
            if (isGrounded)
                canDash = true;
        if (isGrounded)
        {
            if (shadowPower && !PowerUp_ShadowDash || !shadowPower)
                if (Input.GetButtonDown("Fire3") && canDash || _rightTrig && canDash)
                    StartCoroutine(DashMove());
            if (shadowPower && PowerUp_ShadowDash)
                if (Input.GetButtonDown("Fire3") && canDash || _rightTrig && canDash)
                    StartCoroutine(SuperDashMove());
        }
        else if (!isGrounded)
        {
            if (shadowPower && !PowerUp_ShadowDash || !shadowPower)
                if (Input.GetButtonDown("Fire3") && canDash || _rightTrig && canDash)
                    StartCoroutine(DashMove());
            if (shadowPower && PowerUp_ShadowDash)
                if (Input.GetButtonDown("Fire3") && canDash || _rightTrig && canDash)
                    StartCoroutine(SuperDashMove());
        }
    }

    IEnumerator DashMove()
    {
        if (facingRight)
            dashForce = 850;
        if (!facingRight)
            dashForce = -850;

        rb.velocity = new Vector3(0, 0, 0);
        isDashing = true;
        audioS.PlayOneShot(audio_normaldash);
        rb.AddForce(dashForce, 0, 0);
        anim.Play("Dash");
        canDash = false;
        nextDash = Time.time + dashCooldown;
        NormalAfterImage.SetActive(true);
        yield return 0;
        rb.AddForce(dashForce, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.AddForce(dashForce, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.AddForce(dashForce, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.AddForce(dashForce, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.AddForce(dashForce, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.AddForce(dashForce, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        NormalAfterImage.SetActive(false);
        isDashing = false;
    }
    IEnumerator SuperDashMove()
    {
        if (facingRight)
            superDashForce = 400;
        if (!facingRight)
            superDashForce = -400;
        feetcap.enabled = false;
        rb.velocity = new Vector3(0, 0, 0);
        audioS.PlayOneShot(audio_shadowdash);
        isDashing = true;
        isShadowing = true;
        invincible = true;
        ShadowAfterImage.SetActive(true);
        SlowTime2f();
        rb.AddForce(Vector3.right * superDashForce, ForceMode.Impulse);
        anim.Play("Dash");
        canDash = false;
        emitDashCooldown = true;
        nextShadow = Time.time + shadowCooldown;
        nextDash = Time.time + dashCooldown;
        SlowTime2f();
        yield return new WaitForSeconds(0.1f);
        ShadowAfterImage.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        SlowTime4f();
        yield return new WaitForSeconds(0.1f);
        SlowTime6f();
        yield return new WaitForSeconds(0.1f);
        SlowTime8f();
        yield return new WaitForSeconds(0.05f);
        TimeNormal();
        isDashing = false;
        isShadowing = false;
        invincible = false;
        feetcap.enabled = true;
    }

    void AirAttackCooldown()
    {
        isAttacking = false;
        anim.SetBool("AttackAir", isAttacking);
    }
    void ComboStarter()
    {
        if (canClick)
            noOfClicks++;
        if (noOfClicks == 1)
        {
            anim.SetInteger("AttackGround", noOfClicks);
            anim.SetInteger("WeaponType", weaponType);
        }
    }
    public void ComboCheck()
    {
        canClick = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 1") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle  
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 1") && noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo          
            anim.SetInteger("AttackGround", 2);
            canClick = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 2") && noOfClicks == 2)
        {  //If the second animation is still playing and only 2 clicks have happened, return to idle          
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 2") && noOfClicks < 3)
        {  //Failproof return to idle       
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 2") && noOfClicks >= 3)
        {  //If the second animation is still playing and at least 3 clicks have happened, continue the combo         
            anim.SetInteger("AttackGround", 3);
            canClick = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 3"))
        { //Since this is the third and last animation, return to idle                  
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1HAttack 3") && noOfClicks >= 3)
        { //Failproof return to idle               
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else
        {
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
    }
    public void ComboCheckDual()
    {
        canClick = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack1") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle  
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack1") && noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo          
            anim.SetInteger("AttackGround", 2);
            canClick = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack2") && noOfClicks == 2)
        {  //If the second animation is still playing and only 2 clicks have happened, return to idle          
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack2") && noOfClicks < 3)
        {  //Failproof return to idle       
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack2") && noOfClicks >= 3)
        {  //If the second animation is still playing and at least 3 clicks have happened, continue the combo         
            anim.SetInteger("AttackGround", 3);
            canClick = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack3"))
        { //Since this is the third and last animation, return to idle                  
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("DualAttack3") && noOfClicks >= 3)
        { //Failproof return to idle               
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else
        {
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
    }
    public void ComboCheckGreat()
    {
        canClick = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack1") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle  
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack1") && noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo          
            anim.SetInteger("AttackGround", 2);
            canClick = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack2") && noOfClicks == 2)
        {  //If the second animation is still playing and only 2 clicks have happened, return to idle          
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack2") && noOfClicks < 3)
        {  //Failproof return to idle       
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack2") && noOfClicks >= 3)
        {  //If the second animation is still playing and at least 3 clicks have happened, continue the combo         
            anim.SetInteger("AttackGround", 3);
            canClick = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack3"))
        { //Since this is the third and last animation, return to idle                  
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("GreatAttack3") && noOfClicks >= 3)
        { //Failproof return to idle               
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else
        {
            anim.SetInteger("AttackGround", 0);
            canClick = true;
            noOfClicks = 0;
        }
    }

    public void ActivateTrail() {
        foreach (GameObject weapon in WeaponTrail)
            weapon.GetComponent<XftWeapon.XWeaponTrail>().Activate(); }
    public void DeactivateTrail() {
        foreach (GameObject weapon in WeaponTrail)
            weapon.GetComponent<XftWeapon.XWeaponTrail>().Deactivate(); }
    public void SlowTime2f() { Time.timeScale = 0.2f; }
    public void SlowTime4f() { Time.timeScale = 0.4f; }
    public void SlowTime6f() { Time.timeScale = 0.6f; }
    public void SlowTime8f() { Time.timeScale = 0.8f; }
    public void TimeNormal() { Time.timeScale = 1f; }

    IEnumerator SmallMoveRoutine()
    {
        if (facingRight)
        {
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        if (!facingRight)
        {
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        yield return 0;
    }
    IEnumerator MediumMoveRoutine()
    {
        if (facingRight)
        {
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        if (!facingRight)
        {
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }

        yield return 0;
    }
    IEnumerator BigMoveRoutine()
    {
        if (facingRight)
        {
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(100, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        if (!facingRight)
        {
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(-100, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        yield return 0;
    }

    private bool isLeftBeingPressed = false;
    private bool isRightBeingPressed = false;
    private void LeftTrigger()
    {
        if (LeftTriggerAxis > 0)
        {
            if (!isLeftBeingPressed)
                _leftTrig = true;
            else if (isLeftBeingPressed)
                _leftTrig = false;
            isLeftBeingPressed = true;

        }
        else if (LeftTriggerAxis == 0)
        {
            isLeftBeingPressed = false;
        }
    }
    private void RightTrigger()
    {
        if (RightTriggerAxis > 0)
        {
            if (!isRightBeingPressed)
                _rightTrig = true;
            else if (isRightBeingPressed)
                _rightTrig = false;
            isRightBeingPressed = true;
        }
        else if (RightTriggerAxis == 0)
        {
            isRightBeingPressed = false;
        }
    }
    private void WeaponAnimation()
    {
        if (lastWeapon != weaponType)
        {
            anim.Play("SwitchWeapons");
            lastWeapon = weaponType;
        }
        if (weaponType == 0)
        {
            OverridingController["Idle"] = singleSwordIdleClip;
            OverridingController["JumpSlash"] = singleSwordJumpAttackClip;
            airAttackSpeed = 1.0f;
        }
        if (weaponType == 1)
        {
            OverridingController["Idle"] = dualSwordIdleClip;
            OverridingController["JumpSlash"] = dualSwordJumpAttackClip;
            airAttackSpeed = 1.5f;
        }
        if (weaponType == 2)
        {
            OverridingController["Idle"] = greatSwordIdleClip;
            OverridingController["JumpSlash"] = greatSwordJumpAttackClip;
            airAttackSpeed = 2.5f;
        }
    }

    IEnumerator RangedSlash()
    {
        GameObject Temporary_Slash_Handler;
        Temporary_Slash_Handler = Instantiate(RangedDarkSlash, RangedSlashEmitter.transform.position, RangedSlashEmitter.transform.rotation);
        Temporary_Slash_Handler.transform.Rotate(Vector3.forward * 90);
        anim.Play("RangedSlash");
        isAttacking = true;
        isShadowing = true;
        nextShadow = Time.time + shadowCooldown;
        emitDashCooldown = true;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        yield return 0;
        rb.velocity = new Vector3(0, 0, 0);
        isAttacking = false;
        isShadowing = false;
        DashCD.GetComponent<DashCooldownEffect>().Disable();
        Destroy(Temporary_Slash_Handler, 1f);
        yield return new WaitForSeconds(0.1f);
    }

    public void RemoveHealth(float amount)
    {
        if (!invincibleEscape)
        {
            health -= amount;
            healthBar.UpdateBar(health, max);
            if (!playerdeath)
                StartCoroutine(HurtFrames());
            if (health <= 0)
                Death();
        }
    }
    public void GreaterKnockback(float amount)
    {
        if (!invincible)
        {
            health -= amount;
            healthBar.UpdateBar(health, max);
            if (!playerdeath)
                StartCoroutine(GreatKnockback());
            if (health <= 0)
                Death();
        }
    }

    IEnumerator HurtFrames()
    {
        float knockback = 0;
        invincible = true;
        invincibleEscape = true;
        anim.Play("Hurt");
        if (facingRight)
            knockback = -300;
        if (!facingRight)
            knockback = 300;
        if (health > 0)
        {
            rb.AddForce(knockback, 0, 0);
            move = 0;
            for(int i = 0; i < 18; i++)
            {
                yield return 0;
                rb.AddForce(knockback, 0, 0);
            }         
            yield return new WaitForSeconds(0.4f);
            invincible = false;
            for(int p = 0; p < 30; p++)
            {
                PlayerRenderer.enabled = !PlayerRenderer.enabled;
                yield return new WaitForSeconds(0.05f);
                
            }
            PlayerRenderer.enabled = true;
            invincibleEscape = false;
        }
    }
    IEnumerator GreatKnockback()
    {
        float knockback = 0;
        invincible = true;
        anim.Play("Hurt");
        if (facingRight)
            knockback = -800;
        if (!facingRight)
            knockback = 800;
        if (health > 0)
        {
            rb.AddForce(knockback, 0, 0);
            move = 0;
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;
            rb.AddForce(knockback, 0, 0);
            yield return 0;

            yield return new WaitForSeconds(0.5f);
            invincible = false;
        }
    }


    public void Death()
    {
        playerdeath = true;
        rb.velocity = new Vector3(0, 0, 0);
        anim.Play("Death");
    }
}
