using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("GunStats")]
    public int damage, magSize, bulletsPerTap;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera Cam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask enemy;


    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip hitSound;

    private AudioSource audioSource;

    [Header("Graphics")]
    public GameObject muzzleFlash, bulletHole;
    public TextMeshProUGUI magText;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();

        bulletsLeft = magSize;
        readyToShoot = true;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MyInput();
        magText.SetText(bulletsLeft + "/" + magSize);
    }

    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && playerMovement.isAiming)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = Cam.transform.forward + new Vector3(x, y, 0);

        readyToShoot = false;
        audioSource.PlayOneShot(shootSound);

        if (Physics.Raycast(Cam.transform.position, direction, out rayHit, range, enemy))
        {
            Quaternion hitRotation = Quaternion.LookRotation(rayHit.normal);
            GameObject hole = Instantiate(bulletHole, rayHit.point, hitRotation);
            AudioSource.PlayClipAtPoint(hitSound, rayHit.point);
            Destroy(hole, 10f);

            /*PlayerHealth targetHealth = rayHit.collider.GetComponent<PlayerHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }*/
        }

        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
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
        audioSource.PlayOneShot(reloadSound);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
    }


}
