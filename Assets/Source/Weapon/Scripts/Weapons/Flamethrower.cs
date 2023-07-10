using System.Collections;
using UnityEngine;

public class Flamethrower : FiringWeapon, IHandWeapon, IRewardWeapon
{
    [SerializeField] private Transform _aim;
    [SerializeField] private ParticleSystem _burningTarget;
    [SerializeField] private Light _light;

    public Transform Aim => _aim;

    public override void Accept(IWeaponVisitor weaponVisitor, float damage)
    {
        weaponVisitor.Visit(this, damage);
    }

    public void PutInHand(bool state)
    {
        PlayerAnimationsController.StopReloadAnimation();
        gameObject.SetActive(state);
        PlayerAnimationsController.PlayWeaponSwitch(WeaponType.Minigun);
    }

    public void BurnTarget(Transform target)
    {
        ParticleSystem burningEffect = Instantiate(_burningTarget, target.position, Quaternion.LookRotation(Vector3.up));
        burningEffect.transform.SetParent(target);

        Light light = Instantiate(_light, target.position, Quaternion.identity, burningEffect.transform);
        InitLight(light);
    }

    public void Reload() => CurrentBulletsCount = ClipCapacity;

    private void InitLight(Light light)
    {
        light.intensity = 3;
        light.range = 3;
        light.enabled = true;

        StartCoroutine(LightFlickering());

        IEnumerator LightFlickering()
        {
            while (light != null)
            {
                light.intensity = 2 + Mathf.PingPong(Time.time, 3);
                yield return null;
            }
        }
    }
}
