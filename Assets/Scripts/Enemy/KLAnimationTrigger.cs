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
}
