using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    private AudioManager audiomanager;
    WeaponHolster Holster;

    [SerializeField]
    private StatusIndicator statusIndicator;

    public GameObject damageEffect;
    public GameObject deathParticles;

    void Awake()
    {
        audiomanager = AudioManager.instance;
        Holster = FindObjectOfType<WeaponHolster>();
    }
   
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Holster.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Holster.Reload();
        }
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        Destroy(Instantiate(damageEffect.gameObject, hitPoint + hitDirection, Quaternion.FromToRotation(Vector3.up, hitDirection)) as GameObject, 10f);
        if (damage >= health)
        {
            Destroy(Instantiate(deathParticles.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, 10f);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
        if (statusIndicator != null)
            statusIndicator.SetHealth(health);
    }
}
