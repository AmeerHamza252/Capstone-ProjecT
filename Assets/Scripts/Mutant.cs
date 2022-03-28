using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mutant : Enemy
{
    [SerializeField] float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;

    [SerializeField] float attackRate = 2f;
    float nextAttackTime = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        InstantiateEntity(30, 10, 2);

        target = PlayerReference.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            // Run
            animator.SetBool("canRun", true);
            //AudioManager.instance.Play("Enemy Running");
        }
        else
        {
            animator.SetBool("canRun", false);
        }

        if (distance <= agent.stoppingDistance)
        {
            if (Time.time >= nextAttackTime)
            {
               
                    //Attacks
                    Attack("canAttack", 0, 0.4f, attackPoint, attackRange, enemyLayers, this.damage, 2, "Enemy Attack", "Player Impact", "Player Scream");
                    nextAttackTime = Time.time + 1f / attackRate;
                
               
            }
            //Face The Target
            FaceTarget();
        }
        else
        {
            StopAllCoroutines();
        }
    }

    void FaceTarget()
    {
        // Direction to target
        Vector3 direction = (target.position - transform.position).normalized;

        //Rotation to point to Target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Updating Rotation 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
