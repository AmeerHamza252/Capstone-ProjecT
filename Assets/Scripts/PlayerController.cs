using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Characters
{
    public Camera cam;

    [SerializeField] float speed = 5f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] LayerMask aimLayerMask;

    //[SerializeField] float dashSpeed;
    //[SerializeField] float smashRadius = 2.5f;
    [SerializeField] float ultiamteRadius = 10f;
    [SerializeField] float force = 700f;

   
    [SerializeField] float attackRate = 2f;
    [SerializeField] float nextAttackTime = 0f;

    [SerializeField] int manaValue = 40;
    [SerializeField] int maxMana;
    [SerializeField] int currentMana;

    [SerializeField] bool hasKey;

    [HideInInspector]
    public Vector3 movement;

    public ManaBar manabar;
    private Rigidbody rb;


    private void Start()
    {
        // hiding cursor on the screen
        //Cursor.lockState = CursorLockMode.Locked;
        // Player Stats
        InstantiateEntity(100, 10, 2);
        
        //Player Magic Bar 
        maxMana = manaValue;
        currentMana = maxMana;
        manabar.SetMaxMana(maxMana);
       
        rb = GetComponent<Rigidbody>();
    }
   
    // Update is called once per frame
    void Update()
    {   
        if (!GameManager.instance.isGameActive) return;
        MoveInputFromKeyboard();
        PlayerMovement();
        PlayerAttack();
        PlayerAbilities();
    }

    void PlayerAttack()
    {
        // To avoid spamming attack
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButton(0))
            {
                //Light Attack
                Attack("Attack", 0, 0.25f, attackPoint, attackRange, enemyLayers, this.damage, 1, "Sword Swing", "Enemy Impact", "Enemy Scream");
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetMouseButton(1))
            {
                //Heavy Attack
                Attack("Heavy Attack", 1, 0.4f, attackPoint, attackRange, enemyLayers, this.damage * this.damageMultipliyer, 1, "Sword Swing", "Enemy Impact", "Enemy Scream");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

    }

    public void MoveInputFromKeyboard()
    {

        //Input from Keyboard
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Vector to store input
        movement = new Vector3(horizontal, 0f, vertical);


    }

    public void PlayerMovement()
    {   // Player Aim
        AimTowardMouse();
        

        // Input To Move Cahracter
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
            
        }
        else
        {
      
            Moving(speed);
            
        }
    }
  

    private void AimTowardMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * 150 * Time.deltaTime;
        transform.Rotate(Vector3.up, mouseX);
        // Ray Casted from Camera which act as aim refrence point where the player sees

        //Ray ray = cam.ScreenPointToRay(Input.mousePosition) ;
        //if(Physics.Raycast(ray, out var hitinfo, Mathf.Infinity, aimLayerMask))
        //{
        //    var direction = hitinfo.point - transform.position ;
        //    direction.y = 0f;
        //    direction.Normalize();
        //    transform.forward = direction;
        //}
    }


    public void Moving(float speed)
    {
       
        //Moving
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= speed * Time.deltaTime;
            transform.Translate(movement, Space.Self);
            transform.TransformDirection(movement);

        }

        

        //Animating
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);

        animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        
    }

    void Running()
    {
       
        Moving(runSpeed);
       
        animator.SetTrigger("isRunning");
        
    }

    void PlayerAbilities()
    {
        if (currentMana > 0)
        {
            //if (Input.GetKeyDown(KeyCode.Q))
            //{

            //    StartCoroutine(SmashAbility());
            //}
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    StartCoroutine(DashAbility());
            //}
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(UltimateAbility());
            }
        }
    }

    //IEnumerator SmashAbility()
    //{
    //    // Magic
    //    currentMana -= 10;
    //    manabar.SetMana(currentMana);

    //    //Play sound
    //    AudioManager.instance.Play("Smash");

    //    //Play animation
    //    animator.SetTrigger("smashing");
       
    //    //Play particle effect
    //    particleEffects[3].Play();

        
    //    // Detecting player in certain radius around player
    //    Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, smashRadius);

    //    //Damage them
    //    foreach (Collider enemy in nearbyEnemies)
    //    {
    //        Rigidbody rb = enemy.GetComponent<Rigidbody>();
    //        if(rb!= null)
    //        {
    //            rb.AddExplosionForce(force, transform.position, smashRadius);
    //        }

    //        Enemy EnemyToBeDamaged = enemy.GetComponent<Enemy>();
    //        if (EnemyToBeDamaged != null)
    //        {
    //            EnemyToBeDamaged.GetDamageThorughAbility(50, 1, enemy.transform.position + new Vector3(0, 1.5f, 0), "Enemy Impact", "Enemy Scream");
    //        }
    //    }

    //    yield return new WaitForSeconds(1);
    //    particleEffects[3].Stop();

    //}

    //IEnumerator DashAbility()
    //{   //Player magic
    //    currentMana -= 10;
    //    manabar.SetMana(currentMana);

    //    //Audio
    //    AudioManager.instance.Play("Dash");

    //    //Particle effect
    //    particleEffects[4].Play();
      
    //    Moving(dashSpeed);

    //    yield return new WaitForSeconds(1);
    //    particleEffects[4].Stop();

    //}

    IEnumerator UltimateAbility()
    {
        //Player magic
        currentMana -= 10;
        manabar.SetMana(currentMana);

        //Audio
        AudioManager.instance.Play("Smash");

        //Animation
        animator.SetTrigger("ultimate");
       
        //Particle effect
        particleEffects[5].Play();


        // Detecting player in certain radius around player
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, ultiamteRadius);

        //Damage them
        foreach (Collider enemy in nearbyEnemies)
        {
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce( Vector3.forward * force);
            }

            Enemy EnemyToBeDamaged = enemy.GetComponent<Enemy>();
            if (EnemyToBeDamaged != null)
            {
                EnemyToBeDamaged.GetDamageThorughAbility(50, 1, enemy.transform.position + new Vector3(0, 1.5f, 0), "Enemy Impact", "Enemy Scream");
            }
        }

        yield return new WaitForSeconds(1);
        particleEffects[5].Stop();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //Player interaction with KEY to end game
        if (collision.gameObject.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(GameManager.instance.keyPrefab);
            GameManager.instance.keyPic.SetActive(true);
            GameManager.instance.gameFinishPanel.SetActive(true);
        }
       
    }

    //Display the radius around player
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, smashRadius);
        Gizmos.DrawWireSphere(transform.position, ultiamteRadius);
    }

  
    


}
