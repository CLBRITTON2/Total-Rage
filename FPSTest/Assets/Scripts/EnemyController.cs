using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        // Enemy disabled if it is hit by bullet
        if (other.gameObject.tag == "Bullet")
        {
            Disable();
        }
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
