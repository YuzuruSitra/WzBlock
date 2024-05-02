using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;

public class MagicAttacks_Projectile : MonoBehaviour
{
    private Vector3 projectileDir;
    public GameObject FX_Hit;

    ParticleSystem FX_Projectile;
    ParticleSystem FX_ProjectileBase;
    ParticleSystem FX_ProjectileTrail;

    AudioSource SFX_Projectile;

    SphereCollider projectileCol;

    float moveSpeed;

    private void Start()
    {
        FX_Projectile = gameObject.GetComponent<ParticleSystem>();
        FX_ProjectileBase = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        FX_ProjectileTrail = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();

        projectileCol = gameObject.GetComponent<SphereCollider>();
        SFX_Projectile = gameObject.GetComponent<AudioSource>();


        moveSpeed = 60f;

    }

    public void Setup(Vector3 projectileDir)
    {
        this.projectileDir = projectileDir;
    }

    private void Update()
    {
        transform.position += projectileDir * moveSpeed * Time.deltaTime;
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider col)
    {
        GameObject hitFX;
        hitFX = Instantiate(FX_Hit, col.transform.position, Quaternion.identity);
        Destroy(hitFX, 3f);


        FX_ProjectileBase.gameObject.SetActive(false);
        FX_ProjectileBase.Stop();
        FX_ProjectileTrail.Stop();
        FX_Projectile.Stop();

        SFX_Projectile.Stop();

        moveSpeed = 0f;
        projectileCol.enabled = false;

        Destroy(gameObject, 3f);
    }
}
