using UnityEngine;

public class Bow : WeaponBase
{
    public override void Attack()
    {
        print("Attacking");
    }

    public override void Charge()
    {
        print("Charging");
    }
}
