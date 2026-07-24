using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Spectre.Features.Other;

/// <summary>
/// 允许用户从特殊按键列表中移除部分按键，使其不被"输入忽略"。
/// 用户允许的按键仍然会计入 mainPressCount / maximumUsedKeys。
/// </summary>

[HarmonyPatch(typeof(RDInputType_Keyboard), "CountSpecialInput")]
internal static class Patch_KeyboardSpecialKeys
{
    [HarmonyPostfix]
    internal static void Postfix(List<KeyCode> __result)
    {
        if (Options.AllowedKeyboardSpecialKeys.Count == 0)
            return;
        __result.RemoveAll(k => Options.AllowedKeyboardSpecialKeys.Contains(k.ToString()));
    }
}

[HarmonyPatch(typeof(RDInputType_AsyncKeyboard), "GetSpecialInput")]
internal static class Patch_AsyncSpecialKeys
{
    [HarmonyPostfix]
    internal static void Postfix(List<AsyncKeyCode> __result)
    {
        if (Options.AllowedAsyncSpecialKeys.Count == 0)
            return;
        __result.RemoveAll(k => Options.AllowedAsyncSpecialKeys.Contains(k.label.ToString()));
    }
}