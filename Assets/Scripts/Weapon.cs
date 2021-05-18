using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bullestsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    //graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI text;


    // Start is called before the first frame update
    void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        //settext
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        //Shoot
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bullestsPerTap;
            Shoot();
        }

    }


    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //Raycast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                //rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
            }
        }


        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        if (muzzleFlash)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }



        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
  

}
