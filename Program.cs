using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

using static DaThinky.Patches.MainMenuPatch;

namespace DaThinky;

[ModInitializer(nameof(Initialize))]
public class Program
{
	private const string ModId = "Sts2-DaThinky";

	public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

	public static void Initialize()
	{
		LogWindow logWindow = new();
		((SceneTree)Engine.GetMainLoop()).Root.CallDeferred("add_child", logWindow);

		Harmony harmony = new(ModId);
		harmony.PatchAll();
	}
}
