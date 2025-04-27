using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Ice fire effect", menuName = "Data/Item effect/Ice fire")]
public class IceFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player = PlayerManager.Instance.player;

        bool thirdAttack = player.primaryAttack.comboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceFire = Instantiate(iceFirePrefab, _respondPosition.position, player.transform.rotation);
            newIceFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir,0);
        }

    }
    
    
}
