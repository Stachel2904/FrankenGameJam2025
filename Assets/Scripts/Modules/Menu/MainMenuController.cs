using DivineSkies.Modules;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _startGame;

    private void Awake()
    {
        _startGame.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        ModuleController.LoadScene(SceneNames.GameScene);
    }
}
