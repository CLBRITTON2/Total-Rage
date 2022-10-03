using UnityEngine;

public class DecalDestroyer : MonoBehaviour 
{
	private void OnEnable()
	{
        // Can manipulate gun max effective range by lowering disable time
		switch (gameObject.tag)
		{
			case "Bullet Impact Flesh":
                Invoke("Disable", 0.2f);
				break;

            case "Muzzle Flash":
                Invoke("Disable", 0.2f);
                break;

            case "Upgraded Muzzle Flash":
                Invoke("Disable", 0.2f);
                break;

            case "Bullet Impact Ground":
                Invoke("Disable", 0.6f);
                break;

            case "Pistol Bullet":
                Invoke("Disable", 2f);
                break;

            case "Assault Rifle Bullet":
                Invoke("Disable", 1.5f);
                break;

            case "Upgraded AR Bullet":
                Invoke("Disable", 1.5f);
                break;

            case "Sniper Rifle Bullet":
                Invoke("Disable", 1.5f);
                break;

            case "Rocket Launcher Bullet":
                Invoke("Disable", 3f);
                break;

            case "Ammocan":
                Invoke("Destroy", 15f);
                break;

            case "First Aid Kit":
                Invoke("Destroy", 15f);
                break;

            default:
                Invoke("Disable", 2f);
                break;
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
    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
