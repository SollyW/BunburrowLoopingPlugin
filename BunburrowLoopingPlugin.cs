using BepInEx;
using HarmonyLib;
using Levels;
using Misc;

namespace BunburrowLoopingPlugin;

[BepInPlugin("au.sollyw.bunburrowlooping", "Bunburrow Looping", "1.0.0")]
public class BunburrowLoopingPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        Harmony.CreateAndPatchAll(typeof(BunburrowLoopingPlugin));
    }

    [HarmonyPatch(typeof(LevelsList))]
    [HarmonyPatch("AdjacentBunburrows", MethodType.Getter)]
    [HarmonyPostfix]
    public static void AdjacentBunburrows(ref DirectionsListOf<LevelsList> __result)
    {
        DirectionsListOf<LevelsList> copy = new DirectionsListOf<LevelsList>(
            __result[Direction.Left],
            __result[Direction.Up],
            __result[Direction.Right],
            __result[Direction.Down]);
        __result = copy;
        FillVoids(copy, Direction.Left);
        FillVoids(copy, Direction.Up);
        FillVoids(copy, Direction.Right);
        FillVoids(copy, Direction.Down);

        static void FillVoids(DirectionsListOf<LevelsList> levelsLists, Direction direction) {
            LevelsList levelList = levelsLists[direction];
            if (levelList == null) {
                Direction opposite = direction.GetOpposite();
                levelsLists.SetPart(direction,
                    levelsLists[opposite]?.AdjacentBunburrows[opposite]);
                UnityEngine.Debug.Log($"Replaced {direction} with {levelsLists[direction]}");
            }
        }
    }
}
