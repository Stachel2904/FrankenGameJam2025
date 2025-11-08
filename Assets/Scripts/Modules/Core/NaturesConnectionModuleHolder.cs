using DivineSkies.Modules.Core;
using DivineSkies.Modules.Game.Card;
using DivineSkies.Modules.Popups.Internal;
using DivineSkies.Modules.ResourceManagement;
using System;

public class NaturesConnectionModuleHolder : ModuleHolder<SceneNames>
{
    protected override Type[] ConstantModules => new[] {
        typeof(PopupController),
        typeof(ResourceController) };

    protected override Type[] GetSceneModuleTypes(SceneNames scene)
    {
        if(scene == SceneNames.GameScene)
        {
            return new[] {
                typeof(CardGameController)
            };
        }

        return Array.Empty<Type>();
    }
}