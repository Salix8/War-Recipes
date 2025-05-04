using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;       // Prefab de la bala
    public Transform firePoint;           // Punto de disparo
    public float bulletSpeed = 20f;
    public AudioClip shootSound;
    public ParticleSystem muzzleExplosion;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Aseguramos que el muzzle esté desactivado al inicio
        if (muzzleExplosion)
        {
            muzzleExplosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instanciar bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        // Reproducir sonido
        if (shootSound && audioSource)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Activar explosión de partículas
        if (muzzleExplosion)
        {
            muzzleExplosion.Play();
        }
    }
}
