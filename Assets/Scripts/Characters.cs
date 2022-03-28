using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int damage;
    public int damageMultipliyer;

    public Animator animator;
    public Healthbar healthbar;

    public ParticleSystem[] particleEffects;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    

    // Characters stats initilization
    public void InstantiateEntity(int maxHealth, int damage, int damageMultiplier)
    {
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
        this.damage = damage;
        this.damageMultipliyer = damageMultiplier;
        healthbar.SetMaxHealth(this.maxHealth);
    }

    //Play particle effects for characters
    public IEnumerator ParticleEffects(int effectsNum, float activationTime)
    {
        yield return new WaitForSeconds(activationTime);
        particleEffects[effectsNum].Play();

    }

    public void Attack(string animationName, int particleEffectNumber, float particleActivationTime, Transform attackPoint, float attackRange, LayerMask enemyLayers, int damage, int bloodParticleEffect, string attackSoundName, string hitSoundName, string screamSoundName)
    {
        //Play particle effect
        StartCoroutine(ParticleEffects(particleEffectNumber, particleActivationTime));

        // Play an attack animation
        animator.SetTrigger(animationName);

        //Play audio
        AudioManager.instance.Play(attackSoundName);


        //Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Characters>().GetDamage(damage, bloodParticleEffect, enemy.transform.position+ new Vector3(0,1.5f,0), hitSoundName, screamSoundName);
        }
    }


    public void GetDamage(int damage, int bloodParticleEfffect , Vector3 transform, string hitSoundName, string screamSoundName)
    {
        //Animation
        animator.SetTrigger("Hurt");
        
        //Audio
        AudioManager.instance.Play(hitSoundName);
        AudioManager.instance.Play(screamSoundName);
       
        //Particle effect
        Instantiate(particleEffects[bloodParticleEfffect], transform , Quaternion.identity).Play();
        
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
       
    }


    public void Die()
    {
        animator.SetBool("isDead", true);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
        Destroy(this.gameObject, 2);
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

   
}
