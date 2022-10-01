using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundEffects : MonoBehaviour
{
    public void PlayMenuSelectSound()
    {
        AudioManager.Instance.PlaySound("MenuSelectSound");
    }
    public void PlayMenuHoverSound()
    {
        AudioManager.Instance.PlaySound("MenuHoverSound");
    }
}
