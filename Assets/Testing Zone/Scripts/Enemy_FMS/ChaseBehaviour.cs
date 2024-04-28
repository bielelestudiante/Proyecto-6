using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChaseBehaviour : BaseBehaviour
{
    public float ChaseSpeed = 4;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Verificar si el enemigo est� muerto
        EnemyHealthSystem enemyHealth = animator.GetComponent<EnemyHealthSystem>();
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            // Detener el comportamiento de persecuci�n si el enemigo est� muerto
            return;
        }

        bool isChasing = CheckPlayer(animator.transform);
        animator.SetBool("IsChasing", isChasing);
        bool isPlayerClose = CheckPlayer2(animator.transform);
        animator.SetBool("IsPlayerClose", isPlayerClose);

        Move(animator.transform);
    }

    private void Move(Transform mySelf)
    {
        Vector3 PlayerPos = new Vector3(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z);

        mySelf.transform.LookAt(PlayerPos);

        mySelf.Translate(Vector3.forward * ChaseSpeed * Time.deltaTime, Space.Self);
    }
}
