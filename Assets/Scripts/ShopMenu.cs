using UnityEngine;

public class ShopMenu : MonoBehaviour
{
   public GameObject panel;
    public bool isPanelOpen = false;

    public GameManager gameManager;
    
    public void OpenMenu()
    {
        isPanelOpen = true;
        panel.SetActive(true);
        gameManager.PauseGame();
    }
    public void CloseMenu()
    {
        isPanelOpen = false;
        panel.SetActive(false);
        gameManager.ResumeGame();
    }

    public void BuyItem()
    {
        
    }
}
