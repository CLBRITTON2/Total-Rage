using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour 
{
	private void OnEnable()
	{
		// Bullet hole auto disable after 2 seconds
		Invoke("Disable", 1.5f);
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
