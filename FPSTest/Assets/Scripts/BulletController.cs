using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody bulletRigidBody;

    private void OnEnable()
    {
        // Bullet auto disable after 2 seconds
        Invoke("Disable", 2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BulletFlightPath();
    }
    void Disable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Bullet gets disabled if it hits an object
        Disable();
    }
    private void BulletFlightPath()
    {
        bulletRigidBody.velocity = transform.forward * speed;
    }
}