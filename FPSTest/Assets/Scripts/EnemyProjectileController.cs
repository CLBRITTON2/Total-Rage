using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public Rigidbody enemyProjectileRigidbody;
    public float upForce, forwardForce;

    private void OnEnable()
    {
        ThrowGrenade();
        // Projectile auto disable after 2 seconds
        Invoke("Disable", 2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
        enemyProjectileRigidbody.velocity = Vector3.zero;
        enemyProjectileRigidbody.angularVelocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
