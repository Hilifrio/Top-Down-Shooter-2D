using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{


    public Transform deathParticles;
    public GameObject damageEffect;

    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;
    public int damage = 0;
    public float attackRange = 0;

    public string enemyDeathSound = "Explosion";

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    public event System.Action OnAttack;

    void Awake()
    {
        Debug.Log(health);
        if (statusIndicator != null)
        {
            statusIndicator.setMaxHealth(startingHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null)
        {
            Debug.Log("Damage Player");
            _player.TakeDamage(damage);
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
