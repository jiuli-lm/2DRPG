using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLAnimationTrigger : MonoBehaviour
{
    private Enemy_KL enemy => GetComponentInParent<Enemy_KL>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRidius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage();
            }
        }
    }
    
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
    
}
