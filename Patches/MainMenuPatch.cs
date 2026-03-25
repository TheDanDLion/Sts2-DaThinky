using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace DaThinky.Patches;

[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public class MainMenuPatch
{
    [HarmonyPostfix]
    static void AddThoughtBubble(NMainMenu __instance)
    {
        Log.LogMessage(LogLevel.Info, LogType.Generic, "HELLO??");
		
        // Add the thought bubble scene. This is the calculator.
        PackedScene thoughtBubbleScene = ResourceLoader.Load<PackedScene>("res://Scenes/ThoughtBubble.tscn");
        TextureRect bubble = thoughtBubbleScene.Instantiate<TextureRect>();
        bubble.AnchorLeft = 0.3F;
        bubble.AnchorTop = 0.15F;
        // bubble.Visible = false;
        __instance.AddChild(bubble);
		
        // Add the button used to toggle the visibility of the thought bubble.
        PackedScene buttonScene = ResourceLoader.Load<PackedScene>("res://Scenes/ToggleButton.tscn");
        Button button = buttonScene.Instantiate<Button>();
        // button.AnchorLeft = 0.05F;
        // button.AnchorTop = 0.65F;
        __instance.AddChild(button);
		
        // Whenever the button is pressed, toggle the visibility of the bubble
		
		
        // button.Pressed += () => bubble.SetVisible(!bubble.Visible);
    }
}