using UnityEngine;

public class ToggleBlackScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject blackScreen;
    private bool isBlack = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {   
            isBlack = !isBlack;
        }
    }
    private void FixedUpdate() {
        if (isBlack == true)
        {
            blackScreen.SetActive(true);
        } 
        else
        {
            blackScreen.SetActive(false);
        }
    }
}
