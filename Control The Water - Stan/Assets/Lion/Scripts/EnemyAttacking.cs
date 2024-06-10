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
    public float defaultObserveRangeMin = 20;
    public float defaultObserveRangeMax = 30;
    public float defaultNextAttackChance = 50;
    
    float distance;
    float attackCooldown;
    bool attackColliderActive = false;
    string fightState = "close in";
    Vector3 targetPosition;

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
        if (fightState == "close in")
        {
            agent.isStopped = false;
            animator.SetInteger("movingState", 1);
            if (distance < defaultObserveRangeMax)
            {
                fightState = "observe";
            }
            else
            {
                agent.SetDestination(PositionAtDistanceFromPlayer(defaultObserveRangeMax));
            }
        }
        else if (fightState == "observe")
        {
            agent.isStopped = false;
            RotateTowardsPlayer();
            animator.SetInteger("movingState", 0);
            if (distance < defaultObserveRangeMin)
            {
                fightState = "attack";
            }
            else if (distance > defaultObserveRangeMax)
            {
                fightState = "close in";
                EnemyStateController.LoseInterest();
            }
        }
        else if (fightState == "attack")
        {
            if (attackCooldown <= 0 && distance <= defaultAttackRange)
            {
                agent.isStopped = true;
                animator.SetInteger("movingState", 1);
                animator.SetTrigger("attack1");
                attackCooldown = defaultAttackCooldown * Random.Range(1 - defaultRandomAttackCooldown, 1 + defaultRandomAttackCooldown);
                agent.ResetPath();
                if (defaultNextAttackChance < Random.Range(0, 100))
                {
                    fightState = "back up";
                }
            }
            if (attackCooldown > 0)
            {
                agent.isStopped = true;
                animator.SetInteger("movingState", -1);
            }
            else if (distance > defaultAttackRange)
            {
                agent.isStopped = false;
                animator.SetInteger("movingState", 1);
                agent.SetDestination(player.transform.position); 
            }
            else
            {
                agent.isStopped = true;
                animator.SetInteger("movingState", 0);
            }
        }
        else if (fightState == "back up")
        {
            animator.SetInteger("movingState", -1);
            if (distance > defaultObserveRangeMin)
            {
                fightState = "observe";
            }
            else
            {
                agent.SetDestination(PositionAtDistanceFromPlayer(defaultObserveRangeMin + 1));
            }
        }


        agent.speed = speed;
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) // attack animation is playing
        // {
        //     agent.isStopped = true;
        //     if (!attackColliderActive)
        //     {
        //         RotateTowardsPlayer();
        //     }
        // }
        // else if (distance > defaultAttackRange) // move to player
        // {
        //     agent.isStopped = false;
        //     agent.SetDestination(EnemyStateController.lastSeenPlayerPosition);
        // }
        // else if (attackCooldown > 0) // attack is on cooldown
        // {

        // }
        // else // start attack
        // {
        //     agent.isStopped = true;
        //     animator.SetTrigger("attack1");
        //     attackCooldown = defaultAttackCooldown * Random.Range(1 - defaultRandomAttackCooldown, 1 + defaultRandomAttackCooldown);
        //     agent.ResetPath();
        // }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    Vector3 PositionAtDistanceFromPlayer(float x)
    {
        Vector3 A = player.transform.position;
        Vector3 B = transform.position;
        targetPosition = x * Vector3.Normalize(B - A) + A;
        return targetPosition;
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
