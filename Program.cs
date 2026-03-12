using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace Sts2_DaThinky;

[ModInitializer(nameof(Initialize))]
public class Program
{
	private const string ModId = "Sts2-DaThinky";

	public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

	public static void Initialize()
	{
		Harmony harmony = new(ModId);
		harmony.PatchAll();
	}
}

[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public class MainMenuPatch
{
	[HarmonyPostfix]
	static void AddThoughtBubble(NMainMenu __instance)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/ThoughtBubble.tscn");
		TextureRect bubble = scene.Instantiate<TextureRect>();
		__instance.AddChild(bubble);
	}
}
