using DivineSkies.Modules.Core;
using System;

public class NaturesConnectionModuleHolder : ModuleHolder<SceneNames>
{
    protected override Type[] ConstantModules => Array.Empty<Type>();

    protected override Type[] GetSceneModuleTypes(SceneNames scene)
    {
        return Array.Empty<Type>();
    }
}