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

        for (int i = 0; i < bulletsPerTap; i++)
        {
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            Vector3 direction = Cam.transform.forward + Cam.transform.TransformDirection(new Vector3(x, y, 0));

            if (Physics.Raycast(Cam.transform.position, direction, out RaycastHit hit, range))
            {
                GameObject hitObject = hit.collider.gameObject;
                int hitLayer = hitObject.layer;

                if ((enemy.value & (1 << hitLayer)) != 0)
                {
                    EnemyHealth enemyHealth = hitObject.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage);
                        AudioSource.PlayClipAtPoint(hitSound, hit.point);
                    }

                    Destructible destructible = hitObject.GetComponent<Destructible>();
                    if (destructible != null)
                    {
                        destructible.TakeDamage(damage);
                        AudioSource.PlayClipAtPoint(hitSound, hit.point);
                    }
                }

                if ((ground.value & (1 << hitLayer)) != 0)
                {
                    Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                    GameObject hole = Instantiate(bulletHole, hit.point, hitRotation);
                    Destroy(hole, 10f);
                }
            }
        }

        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft -= bulletsPerTap;
        Invoke("ResetShot", timeBetweenShooting);
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
