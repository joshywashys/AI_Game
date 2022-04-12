using UnityEngine;

public class Bow : WeaponBase
{
	public GameObject projectile;

	public override void Attack(float charge)
	{
		base.Attack(charge);

		Instantiate(projectile, transform.position + transform.up, transform.rotation);
	}

	public override void Charge(float charge)
	{
		base.Charge(charge);
	}
}
