using DivineSkies.Modules;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _startGame;
    [SerializeField] private Transform _atmo;
    [SerializeField] private float _atmoRotationSpeed;

    private void Awake()
    {
        _startGame.onClick.AddListener(StartGame);
    }

    private void Update()
    {
        _atmo.Rotate(Vector3.up, _atmoRotationSpeed * Time.deltaTime);
    }

    private void StartGame()
    {
        ModuleController.LoadScene(SceneNames.GameScene);
    }
}
