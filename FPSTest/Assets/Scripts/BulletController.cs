using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody bulletRigidBody;

    private void OnEnable()
    {
        Invoke("Hide", 1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidBody.velocity = transform.forward * speed;
    }
    void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
