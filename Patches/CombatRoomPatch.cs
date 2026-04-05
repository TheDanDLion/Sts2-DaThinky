using DaThinky.Scenes;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace DaThinky.Patches;

[HarmonyPatch(typeof(NCombatRoom), nameof(NCombatRoom._Ready))]
public static class CombatRoomPatch
{
    private static bool _bubbleVisible;
    private static ThoughtBubble? _bubble;
    private static ToggleButton? _button;

    [HarmonyPostfix]
    static void AddThoughtBubble(NCombatRoom __instance)
    {
        _bubbleVisible = false;
        
        var thoughtBubbleScene = ResourceLoader.Load<PackedScene>("res://Scenes/ThoughtBubble.tscn");
        _bubble = thoughtBubbleScene.Instantiate<ThoughtBubble>();
        __instance.AddChild(_bubble);

        // Defer so the full scene tree (including NTopBar) is ready before we search it
        if (_button == null || !_button.IsValid())
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
        
        _button = new ToggleButton();
        _button.TextureNormal = ResourceLoader.Load<Texture2D>("res://Resources/Images/calculator.png");
        _button.IgnoreTextureSize = true;
        _button.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;

        if (mapButton is Control mapControl)
            _button.CustomMinimumSize = mapControl.Size * 0.75f;
        _button.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;

        var wrapper = new MarginContainer();
        wrapper.AddThemeConstantOverride("margin_right", 12);
        wrapper.AddChild(_button);

        var mapParent = mapButton.GetParent();
        mapParent.AddChild(wrapper);
        mapParent.MoveChild(wrapper, mapButton.GetIndex());

        _button.Pressed += ToggleBubbleVisibility;
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
