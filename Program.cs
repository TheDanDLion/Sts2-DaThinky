using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace Sts2_Template;

[ModInitializer(nameof(Initialize))]
public class Program
{
    private const string ModId = "Sts2-Template";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}