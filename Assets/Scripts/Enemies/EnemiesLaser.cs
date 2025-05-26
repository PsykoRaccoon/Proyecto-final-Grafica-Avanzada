using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class EnemyLaser : MonoBehaviour
{
    public Transform laserOrigin;
    public Transform player;
    public float gunRange;
    public float fireRate;
    public float laserDuration;

    private LineRenderer laserLine;
    private bool canShoot = true;

    public float damage;

    public AudioClip laserSound;
    private AudioSource audioSource;

    private int playerLayerMask;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Crear máscara para solo la capa "Player"
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    public void TryShoot()
    {
        if (canShoot)
        {
            StartCoroutine(ShootLaser());
        }
    }

    IEnumerator ShootLaser()
    {
        if (player == null)
        {
            Debug.LogWarning("EnemyLaser no tiene referencia al Player. Aborta disparo.");
            yield break;
        }

        canShoot = false;

        audioSource.PlayOneShot(laserSound);

        Vector3 origin = laserOrigin.position;
        Vector3 direction = (player.position - origin).normalized;

        Debug.DrawRay(origin, direction * gunRange, Color.red, 1f);

        laserLine.SetPosition(0, origin);

        // Raycast solo para la capa Player
        if (Physics.Raycast(origin, direction, out RaycastHit hit, gunRange, playerLayerMask))
        {
            Debug.Log("Láser golpeó a: " + hit.collider.name);

            laserLine.SetPosition(1, hit.point);

            PlayerHealth playerHealth = hit.collider.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        else
        {
            laserLine.SetPosition(1, origin + direction * gunRange);
        }

        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
