using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;

    public float speed, rotationSpeed; 
    public float defaultAttackCooldown = 5f;
    public float defaultRandomAttackCooldown = 0.25f;
    public float defaultAttackRange = 4f;
    
    float distance;
    float attackCooldown;
    bool attackColliderActive = false;

    Animator animator;
    EnemyStateController EnemyStateController;
    AnimationClip attackAnimClip;
    AnimationEvent evt1;
    AnimationEvent evt2;
    AnimationEvent evt3;
    AnimationEvent evt4;
    AnimationEvent evt5;
    AnimationEvent evt6;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        EnemyStateController = GetComponent<EnemyStateController>();
        // attackAnimClip = animator.runtimeAnimatorController.animationClips[2];

        // evt1 = new AnimationEvent();
        // evt1.time = 1.33f;
        // evt1.functionName = "AttackStart";
        // evt2 = new AnimationEvent();
        // evt2.time = 1.8f;
        // evt2.functionName = "AttackEnd";
        // evt3 = new AnimationEvent();
        // evt3.time = 2.50f;
        // evt3.functionName = "AttackStart";
        // evt4 = new AnimationEvent();
        // evt4.time = 2.90f;
        // evt4.functionName = "AttackEnd";
        // evt5 = new AnimationEvent();
        // evt5.time = 4.00f;
        // evt5.functionName = "AttackStart";
        // evt6 = new AnimationEvent();
        // evt6.time = 4.33f;
        // evt6.functionName = "AttackEnd";

        // attackAnimClip.AddEvent(evt1);
        // attackAnimClip.AddEvent(evt2);
        // attackAnimClip.AddEvent(evt3);
        // attackAnimClip.AddEvent(evt4);
        // attackAnimClip.AddEvent(evt5);
        // attackAnimClip.AddEvent(evt6);
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        animator.SetFloat("distanceToPlayer", distance);
        attackCooldown -= Time.deltaTime;
    }

    public void Fighting()
    {
        agent.speed = speed;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) // attack animation is playing
        {
            agent.isStopped = true;
            if (!attackColliderActive)
            {
                transform.rotation = RotateTowardsPlayer();
            }
        }
        else if (distance > defaultAttackRange) // move to player
        {
            agent.isStopped = false;
            agent.SetDestination(EnemyStateController.lastSeenPlayerPosition);
        }
        else if (attackCooldown > 0) // attack is on cooldown
        {

        }
        else // start attack
        {
            agent.isStopped = true;
            animator.SetTrigger("attack1");
            attackCooldown = defaultAttackCooldown * Random.Range(1 - defaultRandomAttackCooldown, 1 + defaultRandomAttackCooldown);
            agent.ResetPath();
        }
    }

    Quaternion RotateTowardsPlayer()
    {
        Vector3 direction = EnemyStateController.lastSeenPlayerPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        return Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    // void AttackStart()
    // {
    //     attackColliderActive = true;
    //     weaponCollider.enabled = true;
    // }

    // void AttackEnd()
    // {
    //     attackColliderActive = false;
    //     weaponCollider.enabled = false;
    // }
}
