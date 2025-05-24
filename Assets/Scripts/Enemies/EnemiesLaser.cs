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
    private float fireTimer;
    private bool canShoot = true;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
    }

    public void TryShoot()
    {
        if (canShoot)
        {
            fireTimer = 0f;
            StartCoroutine(ShootLaser());
        }
    }

    IEnumerator ShootLaser()
    {
        canShoot = false;

        Vector3 origin = laserOrigin.position;
        Vector3 direction = (player.position - origin).normalized;

        laserLine.SetPosition(0, origin);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, gunRange))
        {
            laserLine.SetPosition(1, hit.point);

            // damage xd
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
