using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Weapon")]
public class WeaponStatsSO : ScriptableObject
{
    public float damage;
    public float chargeTime;
    public float desiredRange;
}