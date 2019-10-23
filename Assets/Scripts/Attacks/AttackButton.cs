using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public BaseAttack magicAttackToPerform;
    
    public void CastMagicAttacks()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input4(magicAttackToPerform);
    }
}
