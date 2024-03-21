using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    // references
    [SerializeField] Transform gunBarrelEnd;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Animator anim;

    // stats
    [SerializeField] int maxAmmo;
    [SerializeField] float timeBetweenShots = 0.1f;

    // private variables
    int ammo;
    float elapsed = 0;

    //Event Handler
    [SerializeField] UnityEvent<int,int> AmmoChange;

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
        AmmoChange?.Invoke(maxAmmo,ammo);
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
    }

    public bool AttemptFire()
    {
        if (ammo <= 0)
        {
            return false;
        }

        if(elapsed < timeBetweenShots)
        {
            return false;
        }


        Debug.Log("Bang");
        Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;
        AmmoChange?.Invoke(maxAmmo, ammo);

        return true;
    }

    public void AddAmmo(int amount)
    {
        if (amount >= maxAmmo-ammo)
        {
            ammo = maxAmmo;
        }
        else
        {
            ammo += amount;
        }
        AmmoChange?.Invoke(maxAmmo, ammo);
    }
}
