using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitButtonSelected()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
