using System;
using HarmonyLib;

namespace Spectre.Features.EffectRemover;

internal static partial class EffectRemover
{
    internal static void ToggleEffectRemoverPatches(bool effectRemoverOn)
    {
        if (effectRemoverOn)
        {
            PatchManager.ApplyPatch(typeof(Patch_LevelDataDecode));
            PatchManager.ApplyPatch(typeof(Patch_EditorLoadGameScene));
            if (Options.EffectRemoverEnableSave)
                PatchManager.ApplyPatch(typeof(Patch_SaveLevelEditorAction));
        }
        else
        {
            PatchManager.UnpatchPatch(typeof(Patch_LevelDataDecode));
            PatchManager.UnpatchPatch(typeof(Patch_EditorLoadGameScene));
            PatchManager.UnpatchPatch(typeof(Patch_SaveLevelEditorAction));
        }
    }

    internal static void ToggleEditorSavePatch(bool enable)
    {
        if (enable)
            PatchManager.ApplyPatch(typeof(Patch_SaveLevelEditorAction));
        else
            PatchManager.UnpatchPatch(typeof(Patch_SaveLevelEditorAction));
    }
}
