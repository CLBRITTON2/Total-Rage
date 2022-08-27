using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    Rigidbody enemyProjectileRigidbody;
    public float upForce, forwardForce;

    private void OnEnable()
    {
        // Projectile auto disable after 2 seconds
        Invoke("Disable", 2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyProjectileRigidbody = GetComponent<Rigidbody>();

        ThrowGrenade();
    }
    private void ThrowGrenade()
    {
        enemyProjectileRigidbody.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
        enemyProjectileRigidbody.AddForce(transform.up * upForce, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {

    }
    void Disable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
