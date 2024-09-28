using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Button _startButton;
    private Button _quitButton;
    
    void Awake()
    {
        _startButton = GameObject.Find("PlayButton").GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
    }

    private void Start()
    {
        _startButton.onClick.AddListener(StartGame);
        _quitButton.onClick.AddListener(QuitGame);
    }
    
    private static void StartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    
    private static void QuitGame()
    {
        Application.Quit();
    }
}
