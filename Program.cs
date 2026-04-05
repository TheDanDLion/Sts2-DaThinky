using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace DaThinky;

[ModInitializer(nameof(Initialize))]
public class Program
{
	private const string ModId = "Sts2-DaThinky";
	public const string CalculatorHotkey = "dathinky_calculator";

	public static void Initialize()
	{
		// This line is required in order to load the default font within this assembly
		ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());

		if (!InputMap.HasAction(CalculatorHotkey))
		{
			InputMap.AddAction(CalculatorHotkey);
			var key = new InputEventKey();
			key.Keycode = Key.C;
			InputMap.ActionAddEvent(CalculatorHotkey, key);
		}

		Log.LogMessage(LogLevel.Debug, LogType.Generic, "Initializing DaThinky harmony patches");
		Harmony harmony = new(ModId);
		harmony.PatchAll();
	}
}
