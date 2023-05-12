using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private InputActionReference shootControl;
    private bool canShoot = true;
    public bool hasGun = false;
    public GameObject bullet;
    public GameObject gun;
    public Transform muzzle;
    public float bulletSpeed;

    private void OnEnable()
    {
        shootControl.action.Enable();
    }

    private void OnDisable()
    {
        shootControl.action.Disable();
    }

    private void Start()
    {
        gun.SetActive(false);
    } 

    private void Update()
    {
        if(hasGun)
        {
            gun.SetActive(true);
        }

        if(hasGun && canShoot && shootControl.action.WasPressedThisFrame())
        {
            ShootGun();
        }
    }

    private void ShootGun()
    {
        var bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation);
        bulletInstance.GetComponent<Rigidbody>().velocity = muzzle.transform.forward * bulletSpeed;
        canShoot = false;
        Destroy(bulletInstance, 1.0f);
        canShoot = true;

        // Debug.Log("Shoot!");
    }
}
