using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : PickableItem
{
    public AmmoType ammoType;
    public int nbOfAmmo = 1;
    public GameObject graphics;

    void Start()
    {
        OnArea+=_AddAmmo;
        switch (ammoType)
        {
            case AmmoType.LIGHT_AMMO:
                itemInfos.text = "Light Ammo ";
                break;
            case AmmoType.HEAVY_AMMO:
                itemInfos.text = "Heavy Ammo ";
                break;
            case AmmoType.BUCKSHOT:
                itemInfos.text = "BuckShot ";
                break;
        }
        itemInfos.text+= "x"+nbOfAmmo;
    }

    IEnumerator AddAmmo()
    {
        isPickedUp = true;
        Destroy(graphics);

        //player.GetComponent<WeaponHolster>().ammos[ammoType] += nbOfAmmo;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void _AddAmmo()
    {
        if(graphics!=null)
        {
            StartCoroutine(AddAmmo());
        }
        
    }

}
