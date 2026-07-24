using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace Spectre.Features.Other;

/// <summary>
/// 用 Transpiler 删除 DetailedResults.GenerateResults 中 "maximumUsedKeys" 的
/// > 10 条件判断，使其始终显示。
/// Patch 通过 PatchManager 动态挂载/卸载，受 Options.AlwaysShowMaxUsedKeys 控制。
/// </summary>
[HarmonyPatch(typeof(DetailedResults), "GenerateResults")]
internal static class Patch_AlwaysShowMaxUsedKeys
{
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = instructions.ToList();

        // 游戏 IL: ldc.i4.s 10 → ble.s/ble
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].opcode != OpCodes.Ldc_I4_S)
                continue;
            if (!(list[i].operand is sbyte sb) || sb != 10)
                continue;
            if (i + 1 >= list.Count)
                continue;

            var next = list[i + 1].opcode;
            if (next == OpCodes.Ble_S || next == OpCodes.Ble)
            {
                Debug.Log("[Patch_AlwaysShowMaxUsedKeys] Patched: ldc.i4.s 10 → ble/ble.s");
                list[i] = new CodeInstruction(OpCodes.Pop);
                list[i + 1] = new CodeInstruction(OpCodes.Nop);
                break;
            }
        }

        return list;
    }
}