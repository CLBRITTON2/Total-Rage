using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed = 120f;
    public Rigidbody BulletRigidBody;
    public int ProjectileDamageOutput;

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

    }
    private void FixedUpdate()
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
    private void BulletFlightPath()
    {
        BulletRigidBody.velocity = transform.forward * Speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Bullet gets disabled if it hits an object
        Disable();
    }
}
