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
    public LayerMask enemy;
    public LayerMask ground;


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
        readyToShoot = false;
        audioSource.PlayOneShot(shootSound);

        bulletsShot = Mathf.Min(bulletsPerTap, bulletsLeft);

        Ray cameraRay = Cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        Vector3 targetPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit camHit, range, enemy)) 
        {
            targetPoint = camHit.point;

        }
        else
        {
            targetPoint = Cam.transform.position + Cam.transform.forward * range;

        }

        ShootBullet(targetPoint);
    }

    private void ShootBullet(Vector3 targetPoint)
    {
        Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);

        int traceMask = enemy | ground;

        for (int i = 0; i < bulletsShot; i++)
        {
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            Vector3 baseDir = (targetPoint - attackPoint.position).normalized;
            Vector3 spreadOffset = attackPoint.TransformDirection(new Vector3(x, y, 0));
            Vector3 direction = (baseDir + spreadOffset).normalized;

            Debug.DrawRay(attackPoint.position, direction * range, Color.red, 5f);

            if (Physics.Raycast(attackPoint.position, direction, out RaycastHit hit, range, traceMask))
            {
                int hitLayer = hit.collider.gameObject.layer;

                if ((enemy.value & (1 << hitLayer)) != 0)
                {
                    if (hit.collider.TryGetComponent<EnemyHealth>(out var eH))
                    {
                        eH.TakeDamage(damage);
                        AudioSource.PlayClipAtPoint(hitSound, hit.point);
                    }
                    if (hit.collider.TryGetComponent<Destructible>(out var d))
                    {
                        d.TakeDamage(damage);
                        AudioSource.PlayClipAtPoint(hitSound, hit.point);
                    }
                }
                else if ((ground.value & (1 << hitLayer)) != 0)
                {
                    Quaternion rot = Quaternion.LookRotation(hit.normal);
                    var hole = Instantiate(bulletHole, hit.point, rot);
                    Destroy(hole, 10f);
                }
            }
        }

        bulletsLeft -= bulletsShot;
        Invoke(nameof(ResetShot), timeBetweenShooting);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        playerMovement.animator.SetBool("isReloading", true);
        audioSource.PlayOneShot(reloadSound);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
        playerMovement.animator.SetBool("isReloading", false);
    }



}
