using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour 
{
	private void OnEnable()
	{
		Invoke("Disable", 2f);
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
