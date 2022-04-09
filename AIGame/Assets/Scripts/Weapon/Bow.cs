using UnityEngine;

public class Bow : WeaponBase
{
    protected override void Attack()
    {
        print("Attacking");
    }

    protected override void Charge()
    {
        print("Charging");
    }
}
