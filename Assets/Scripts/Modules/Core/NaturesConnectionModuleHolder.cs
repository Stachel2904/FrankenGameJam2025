using DivineSkies.Modules.Core;
using DivineSkies.Modules.Game.Card;
using DivineSkies.Modules.ResourceManagement;
using System;

public class NaturesConnectionModuleHolder : ModuleHolder<SceneNames>
{
    protected override Type[] ConstantModules => Array.Empty<Type>();

    protected override Type[] GetSceneModuleTypes(SceneNames scene)
    {
        if(scene == SceneNames.GameScene)
        {
            return new[] {
                typeof(CardGameController),
                typeof(ResourceController)
            };
        }

        return Array.Empty<Type>();
    }
}