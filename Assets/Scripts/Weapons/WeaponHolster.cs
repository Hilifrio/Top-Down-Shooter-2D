using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    public Transform weaponSpawn;
    public List<Weapon> weapons = new List<Weapon>();
    public Dictionary<AmmoType, int> ammos = new Dictionary<AmmoType, int>();   //Nombre de type de balles en réserve
    List<int> ammoInWeaponsMag = new List<int>();                               //Nombre de balles dans chaque armes du holster

    bool isReloading = false;
    public Weapon equippedGun { get; protected set;}
    public int selectedWeapon = 0;

    bool isSwitching;

    void Start()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            ammoInWeaponsMag.Add(weapons[i].magasinSize);
            if (!ammos.ContainsKey(weapons[i].ammoType))
            {
                ammos.Add(weapons[i].ammoType, weapons[i].magasinSize * 10);
            }
        }
           StartCoroutine(SelectWeapon());
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if(!isSwitching)
        {
            ammoInWeaponsMag[selectedWeapon] = equippedGun.bulletRemaining;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                selectedWeapon = 0;

            if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count >= 2)
                selectedWeapon = 1;

            if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Count >= 3)
                selectedWeapon = 2;

            if (Input.GetKeyDown(KeyCode.Alpha4) && weapons.Count >= 4)
                selectedWeapon = 3;

            if (Input.GetKeyDown(KeyCode.Alpha5) && weapons.Count >= 5)
                selectedWeapon = 4;
        }
        
        if(previousSelectedWeapon != selectedWeapon)
        {
            StartCoroutine(SelectWeapon());
        }
    }

    IEnumerator SelectWeapon()
    {
        isSwitching = true;
        int i = 0;
        foreach(Weapon weapon in weapons)
        {
            if(i == selectedWeapon)
            {
                if (equippedGun != null)
                {
                    Destroy(equippedGun.gameObject);
                }
                equippedGun = weapon;
                equippedGun = Instantiate(weapon) as Weapon;
                equippedGun.transform.parent = weaponSpawn;

                equippedGun.transform.localPosition = Vector3.zero;
                equippedGun.transform.localRotation = Quaternion.identity;

                equippedGun.bulletRemaining = ammoInWeaponsMag[i];
            }
            else
            {
                //weapon.gameObject.SetActive(false);
            }
            i++;
        }
        yield return new WaitForSeconds(.3f);
        isSwitching = false;
    }

    public void Reload()
    {
        if (equippedGun != null && !isSwitching && equippedGun.bulletRemaining < equippedGun.magasinSize && !isReloading)
        {
            if (ammos[equippedGun.ammoType] == 0)
                Debug.LogWarning("No ammo of this type " + equippedGun.ammoType + " left");
            else
                equippedGun.Reload();
                StartCoroutine(ProcessReload());
        }
    }

    IEnumerator ProcessReload()
    {
        isReloading = true;
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Weapon/" + weaponName + "Reload", transform.position);
        //weaponController.rigController.SetTrigger("Reloading");

        yield return new WaitForSeconds(equippedGun.reloadTime);

        if (equippedGun.magasinSize - equippedGun.bulletRemaining <= ammos[equippedGun.ammoType])
        {
            ammos[equippedGun.ammoType] -= equippedGun.magasinSize - equippedGun.bulletRemaining;
            equippedGun.bulletRemaining = equippedGun.magasinSize;
        }
        else
        {
            equippedGun.bulletRemaining += ammos[equippedGun.ammoType];
            ammos[equippedGun.ammoType] = 0;
        }
        isReloading = false;
    }

    public void Shoot()
    {
        if (equippedGun != null && !isSwitching && !isReloading)
        {
            equippedGun.Shoot();
        }
        if (equippedGun.bulletRemaining == 0)
            Reload();
    }
}
