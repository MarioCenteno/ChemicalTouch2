using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float firingSpeed;
    public Transform bulletSpawn;
    public Transform barrel;
    public GameObject bullet;

    // Start is called before the first frame update


    public void Fire()
    {
        GameObject spawnBullet = Instantiate(bullet, bulletSpawn.position, barrel.rotation);
        spawnBullet.GetComponent<Rigidbody>().velocity = firingSpeed * barrel.forward;
        Destroy(spawnBullet, 2);
    }
}
