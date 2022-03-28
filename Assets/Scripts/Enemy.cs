using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Characters
{
  public void GetDamageThorughAbility(int damage, int bloodParticleEfffect, Vector3 transform, string hitSoundName, string screamSoundName)
    {

        animator.SetTrigger("Hurt");
        AudioManager.instance.Play(hitSoundName);
        AudioManager.instance.Play(screamSoundName);

        Instantiate(particleEffects[bloodParticleEfffect], transform, Quaternion.identity).Play();

        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
