using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
    }
    public void NewGame()
    {
        SceneManager.LoadScene("House");
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quit");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
    }

    public void Settings()
    {
        Debug.Log("Settings called, OptionsPanel.Instance: " + OptionsPanel.Instance);

        OptionsPanel.Instance.Open();
    }
}
