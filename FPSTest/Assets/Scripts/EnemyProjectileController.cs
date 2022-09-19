using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public Rigidbody EnemyProjectileRigidbody;
    public float UpForce, ForwardForce;
    public int DamageAmount = 2;

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
        EnemyProjectileRigidbody.AddForce(transform.forward * ForwardForce, ForceMode.Impulse);
        EnemyProjectileRigidbody.AddForce(transform.up * UpForce, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void Disable()
    {
        EnemyProjectileRigidbody.velocity = Vector3.zero;
        EnemyProjectileRigidbody.angularVelocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthSystem>().PlayerTakeDamage(DamageAmount);
        }
    }
}
