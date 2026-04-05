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
        _bubbleVisible = false;

        var thoughtBubbleScene = ResourceLoader.Load<PackedScene>("res://Scenes/ThoughtBubble.tscn");
        _bubble = thoughtBubbleScene.Instantiate<ThoughtBubble>();
        __instance.AddChild(_bubble);

        // Defer so the full scene tree (including NTopBar) is ready before we search it
        Callable.From(() => AddButtonToTopBar(__instance)).CallDeferred();
    }

    private static void AddButtonToTopBar(NCombatRoom instance)
    {
        // Search the whole tree for the Map button by its unique node name
        var mapButton = instance.GetTree().Root.FindChild("Map", true, false);
        if (mapButton == null)
        {
            Log.LogMessage(LogLevel.Warn, LogType.Generic, "DaThinky: Could not find Map button — cannot place calculator button next to it.");
            return;
        }

        Log.LogMessage(LogLevel.Info, LogType.Generic, $"DaThinky: Found Map button under {mapButton.GetParent().Name}");

        var button = new ToggleButton();
        button.TextureNormal = ResourceLoader.Load<Texture2D>("res://Resources/Images/calculator.png");
        button.IgnoreTextureSize = true;
        button.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
        button.CustomMinimumSize = mapButton is Control mapControl
            ? mapControl.Size
            : new Vector2(53, 48);
        button.PivotOffset = button.CustomMinimumSize / 2f;

        var mapParent = mapButton.GetParent();
        mapParent.AddChild(button);
        mapParent.MoveChild(button, mapButton.GetIndex());

        button.Pressed += ToggleBubbleVisibility;
    }

    internal static void ToggleBubbleVisibility()
    {
        _bubbleVisible = !_bubbleVisible;
        if (_bubbleVisible)
            _bubble?.ShowBubble();
        else
            _bubble?.HideBubble();
    }
}
