using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    [Header("Weapon Stats")]
    public float fireRate = 0;
    public int damage = 10;
    public int bulletSpeed;
    public int magasinSize;
    public float reloadTime = .3f;
    public AmmoType ammoType;

    [Header("What to Hit")]
    public LayerMask whatToHit;

    [Header("Effects")]
    public Transform MuzzleFlashPrefab;
    public string weaponShootSound = "DefaultShot";

    public float camShakeAmt = 0.1f;
    public float camShakeLength = 0.1f;


    float timeToFire = 0;

    public Bullet BulletPrefab;
    public Transform[] firePoint;

    public int bulletRemaining;
    Animator animator;

    //Caching
    AudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        bulletRemaining = magasinSize;
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        /*
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null)
        {
            Debug.LogError("No camera shake script found on GM object");
        }
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audio manager referenced in Weapon");
        }
        */
    }

    // Update is called once per frame

    public void Shoot()
    {
        if (Time.time > timeToFire && bulletRemaining>0)
        {
            for (int i = 0; i < firePoint.Length; i++)
            {
                timeToFire = Time.time + 1 / fireRate;
                Bullet newBullet = (Bullet)Instantiate(BulletPrefab, firePoint[i].position, firePoint[i].rotation);
                //muzzleFlash.Activate();
                newBullet.damage = damage;
                newBullet.SetSpeed(bulletSpeed);
            }
            bulletRemaining--;
            animator.SetTrigger("Fire");
        }
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
    }

    void Effects()
    {
        //shake the camera
        //camShake.Shake(camShakeAmt,camShakeLength);

        //Play sound 
        audioManager.PlaySound(weaponShootSound);
    }
}
