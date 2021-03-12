using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum VanguardState { IDLE, RUN, JUMP, PUNCH }

public class VanguardBehaviour : MonoBehaviour
{
    [Header("Line of Sight")]
    public bool HasLOS;
    public GameObject player;

    private NavMeshAgent nav;
    private Animator animator;

    [Header("Attack")]
    public float attackDistance;
    public PlayerBehaviour playerBehaviour;
    public float damageDelay = 1.0f;
    public bool isAttacking = false;
    public float punchForce = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //var distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (HasLOS) {
            nav.SetDestination(player.transform.position);
        }

        if (HasLOS && Vector3.Distance(transform.position, player.transform.position) < attackDistance) { 
            //maybe an attack?
            animator.SetInteger("AnimState", (int)VanguardState.PUNCH);
            transform.LookAt(transform.position - player.transform.forward);
            
            if (isAttacking == false) {
                isAttacking = true;
                StartCoroutine(DoPunchDamage());
            }
            if (nav.isOnOffMeshLink) {
                animator.SetInteger("AnimState", (int)VanguardState.JUMP);
            }
        } else if (HasLOS) { 
            animator.SetInteger("AnimState", (int)VanguardState.RUN);
            isAttacking = false;
        } else {
            animator.SetInteger("AnimState", (int)VanguardState.IDLE);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            HasLOS = true;
            player = other.transform.gameObject;
        }
    }

    IEnumerator DoPunchDamage() {
        yield return new WaitForSeconds(1);
        //playerBehaviour.TakeDamage(20);
        isAttacking = false;
    }
}
