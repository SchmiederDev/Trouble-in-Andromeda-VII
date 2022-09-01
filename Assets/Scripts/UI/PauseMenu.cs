using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject MenuScreen;
    public GameObject MessagePanel;

    public TMP_Text MessageText;

    private const string noSaveGameTxt = "Sorry, no save game found.";

    private bool gameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        MenuScreen.SetActive(true);
        MessagePanel.SetActive(false);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPauseStatus();
    }

    private void CheckPauseStatus()
    {
        gameIsPaused = Input.GetKeyDown(KeyCode.Escape);

        if (gameIsPaused)
        {
            MenuScreen.SetActive(!MenuScreen.activeSelf);

            if (MenuScreen.activeSelf)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }            

    }

    public void OnResumeButtonClick()
    {
        MenuScreen.SetActive(false);
        Time.timeScale = 1;
        EventSystem.current.SetSelectedGameObject(null);

        if (MessagePanel.activeSelf)
        {
            MessagePanel.SetActive(false);
            MessageText.text = null;
        }

    }

    public void OnLoadGameClick()
    {
        if(TheGame.theGameInst.LoadGame())
        {            
            MenuScreen.SetActive(false);
            Time.timeScale = 1;

            if(MessagePanel.activeSelf)
            {
                MessagePanel.SetActive(false);
                MessageText.text = null;
            }
        }

        else
        {
            MessagePanel.SetActive(true);
            MessageText.text = noSaveGameTxt;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnQuitButtonClick()
    {
        Debug.Log("Game was quit");
        EventSystem.current.SetSelectedGameObject(null);
        Application.Quit();        
    }

}
