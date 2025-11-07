using DivineSkies.Modules;
using DivineSkies.Modules.Core;
using UnityEngine;

public class NaturesConnectionBootstrap : BootstrapBase<SceneNames, NaturesConnectionModuleHolder>
{
    [SerializeField] private GameObject _keepAlive;

    protected override void OnStarted()
    {
        base.OnStarted();
        DontDestroyOnLoad(_keepAlive);
        ModuleController.LoadScene(SceneNames.MainMenuScene);
    }
}