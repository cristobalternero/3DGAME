using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProyectileGun : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;
    public float shootForce;

    [Header("Gun Stats")]
    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magazineSize, bulletsPertap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletShoot;

    [Header("Bools")]
    private bool shooting, readyToShoot, reloading;

    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;

    [Header("Grafics")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem shellCasing;
    
    [Header("Audio")]
    AudioSource gunAudioSource;
    public AudioClip audioShoot;
    public AudioClip reloadGun;

    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        gunAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        muzzleFlash.Stop();
        shellCasing.Stop();
    }

    void Update()
    {
        MyInput();
        
        //Setting ammo display
        if (CanvasReferences.Instance?.ammunitionDisplay != null)
        {
            CanvasReferences.Instance.ammunitionDisplay.SetText(bulletsLeft / bulletsPertap + " / " + magazineSize / bulletsPertap);
        }
    }

    private void MyInput()
    {
        //Checking if allowed to hold down button and take corresponding input
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletShoot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Finding the exact hit position using a raycast
        //VieportPointtoRay(0.5,0.5,0 = middle of the screen)
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //Checking if ray hits something
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75); //just a point far way from the player
        }

        // Calculating direction from attackpoint to targetPoint
        // Direction of A --> B = B.position - A.position
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //just add spread to last direction

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);


        StartCoroutine(WeaponEffects());
        ////Instantiate muzzle flash
        //if (muzzleFlash != null)
        //{
        //    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //}

        bulletsLeft--;
        bulletShoot++;

        //Invoke resetShot function
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more tran one bulletsPerTap make sure to repeat shoot function
        if (bulletShoot < bulletsPertap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        gunAudioSource.PlayOneShot(audioShoot);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        gunAudioSource.PlayOneShot(reloadGun);
        Invoke("ReloadFinish", reloadTime);
    }

    private void ReloadFinish()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    IEnumerator WeaponEffects()
    {
        muzzleFlash.Play();
        shellCasing.Play();
        yield return new WaitForEndOfFrame();
        muzzleFlash.Stop();
        shellCasing.Stop();
    }
}
