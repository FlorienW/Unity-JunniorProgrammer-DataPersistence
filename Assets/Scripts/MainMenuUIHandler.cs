using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    //* If Start button pressed loads the game scene.
    public void StartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    
    //* If Quit button pressed if game was on the unity editor it stops the playmode else quits from the application.
    public void QuitButtonClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
    
    //* If InputAreaOnValueChanged sets the current player name.
    public void InputAreaOnValueChanged(string value)
    {
        GameManager.instance.SetPlayerName(value);
    }
}
