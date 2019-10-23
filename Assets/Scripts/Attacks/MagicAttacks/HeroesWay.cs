using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesWay : BaseAttack
{
   public HeroesWay()
    {
        attackName = "HeroesWay";
        attackDescription = "This spell only usable by honorable HEROES,(nobody knows where this spell comes from)";
        attackDamage = 75f;
        attackCost = 60f;
    }
}
