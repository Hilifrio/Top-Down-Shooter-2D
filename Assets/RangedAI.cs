using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Weapon weapon;
    public float timeBetweenShots = 0;
    private EnemyStateAI stateAI;
    public bool isReloading = false;
    void Start()
    {
        stateAI = GetComponent<EnemyStateAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateAI.isInRange && !isReloading)
            weapon.Shoot();
        if (weapon.bulletRemaining == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
            

    }

    IEnumerator Reload()
    {
        isReloading = true;
        weapon.Reload();
        yield return new WaitForSeconds(weapon.reloadTime);
        weapon.bulletRemaining = weapon.magasinSize;
        isReloading = false;
    }
}
