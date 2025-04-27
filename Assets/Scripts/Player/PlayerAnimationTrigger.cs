using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger(){
        player.AnimationTrigger();
    }
    private void AttackTrigger(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRidius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                
                if (_target != null)
                    player.stats.DoDamage(_target);
                
                // 修改前
                // hit.GetComponent<Enemy>().Damage();

                Inventory.Instance?.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }
    
    private void ThrowSword()
    {
        SkillManager.Instance.sword.CreateSword();
    }
    
}
