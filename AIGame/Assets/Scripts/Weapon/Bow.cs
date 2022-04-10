using UnityEngine;

public class Bow : WeaponBase
{
	public override void Attack(float charge)
	{
		base.Attack(charge);

		print("Attacking");
	}

	public override void Charge(float charge)
	{
		base.Charge(charge);
	}
}
