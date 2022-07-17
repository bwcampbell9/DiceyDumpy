using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime;
    public int magazineSize, bulletsPerTap, damage, range;
    public bool allowButtonHold, isHitScan;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing :D
    public bool allowInvoke = true;

    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    private void Start()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
        //Set ammo display, if it exists :D
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        
        if (!reloading && bulletsLeft <= 0) Reload();
    }
    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse1);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            TryShoot();
        }
    }

    public void TryShoot()
    {
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            //Find the exact hit position using a raycast
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
            RaycastHit hit;

            //check if ray hits something
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75); //Just a point far away from the player

            //Calculate direction from attackPoint to targetPoint
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            Shoot(directionWithoutSpread);
        }
    }

    public void TryShoot(Vector3 direction)
    {
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot(direction);
        }
    }

    private void Shoot(Vector3 direction)
    {
        
        readyToShoot = false;

        //if more than one bulletsPerTap make sure to repeat shoot function
        while (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            //Calculate spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calculate new direction with spread
            Vector3 directionWithSpread = direction + new Vector3(x, y, 0); //Just add spread to last direction
            if (isHitScan)
            {
                if (Physics.Raycast(transform.position, directionWithSpread, out rayHit, range, whatIsEnemy))
                {
                    if (rayHit.collider.CompareTag("Enemy"))
                        rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
                    Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
                    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
                }
            }
            else
            {

                Debug.Log("pew");
                //Instantiate bullet/projectile
                GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
                //Rotate bullet to shoot direction
                currentBullet.transform.forward = directionWithSpread.normalized;

                //Add forces to bullet
                currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
                currentBullet.GetComponent<Rigidbody>().AddForce(transform.up * upwardForce, ForceMode.Impulse);
            }

            //Instantiate muzzle flash, if you have one
            if (muzzleFlash != null)
                Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

            bulletsLeft--;
            bulletsShot++;

            //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;

                //Add recoil to player (should only be called once)
                if (playerRb)
                {
                    playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
                }
            }
        }
    }
    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished()
    {
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
