using DaThinky.Scenes;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace DaThinky.Patches;

[HarmonyPatch(typeof(NCombatRoom), nameof(NCombatRoom._Ready))]
public static class CombatRoomPatch
{
    private static bool _bubbleVisible;
    private static ThoughtBubble? _bubble;
    
    [HarmonyPostfix]
    static void AddThoughtBubble(NCombatRoom __instance)
    {
        Log.LogMessage(LogLevel.Info, LogType.Generic, "Adding thought bubble to combat room.");

        // Add the thought bubble scene.
        var thoughtBubbleScene = ResourceLoader.Load<PackedScene>("res://Scenes/ThoughtBubble.tscn");
        _bubble = thoughtBubbleScene.Instantiate<ThoughtBubble>();
        __instance.AddChild(_bubble);
        Log.LogMessage(LogLevel.Info, LogType.Generic, "Added thought bubble to combat room.");

        // Add the button used to toggle the visibility of the thought bubble.
        var buttonScene = ResourceLoader.Load<PackedScene>("res://Scenes/ToggleButton.tscn");
        var button = buttonScene.Instantiate<TextureButton>();
        button.AnchorLeft   = 0.05F;
        button.AnchorRight  = 0.05F;
        button.AnchorTop    = 0.65F;
        button.AnchorBottom = 0.65F;
        button.Visible = true;
        // Whenever the button is pressed, toggle the visibility of the bubble.
        button.Pressed += ToggleBubbleVisibility;
        __instance.AddChild(button);
        Log.LogMessage(LogLevel.Info, LogType.Generic, "Added button to combat room.");
    }

    private static void ToggleBubbleVisibility()
    {
        _bubbleVisible = !_bubbleVisible;
        if (_bubbleVisible)
            _bubble?.ShowBubble();
        else
            _bubble?.HideBubble();
    }
}
