using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using demifiend100exp;

[assembly: MelonInfo(typeof(Demi100EXP), "Demifiend Gets 100% Experience", "1.0.0", "Valk")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace demifiend100exp;

public class Demi100EXP : MelonMod
{
    private static uint demifiendExp;

    [HarmonyPatch(typeof(nbResultProcess), nameof(nbResultProcess.nbResultLoad))]
    private class Patch
    {
        public static void Prefix()
        {
            // Registers Demi-Fiend's exp after exp attribution at the end of a battle
            demifiendExp = dds3GlobalWork.DDS3_GBWK.unitwork[0].exp;
        }
    }

    [HarmonyPatch(typeof(nbResultProcess), nameof(nbResultProcess.nbResultShutdown))]
    private class Patch2
    {
        public static void Postfix()
        {
            // The Demi-Fiend is always in stock slot 0.
            // Give only slot 0 the full 100% EXP that he would normally
            // miss when removed from the active party.
            var demifiend = dds3GlobalWork.DDS3_GBWK.unitwork[0];

            if (demifiendExp == demifiend.exp)
            {
                datCalc.datAddExp(demifiend, (int)nbResultProcess.AllExp); // Get 100% EXP
            }
        }
    }
}

