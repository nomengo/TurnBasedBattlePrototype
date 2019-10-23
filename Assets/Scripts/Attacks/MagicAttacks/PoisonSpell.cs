using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSpell : BaseAttack
{
   public PoisonSpell()
    {
        attackName = "Poison1";
        attackDescription = "Poison, well I am not used to it";
        attackDamage = 5f;
        attackCost = 5f;
    }
}
