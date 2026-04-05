using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace DaThinky.Scenes;

public partial class ThoughtBubble : Control
{
	public override void _Ready()
	{
		Log.LogMessage(LogLevel.Info, LogType.Generic, "here?");
		
		var font = GD.Load<Font>("res://fonts/kreon_bold.ttf");
		if (font == null) return;

		Log.LogMessage(LogLevel.Info, LogType.Generic, "here2?");
		ApplyFont(this, font);
	}

	private static void ApplyFont(Node node, Font font)
	{
		Log.LogMessage(LogLevel.Info, LogType.Generic, "AHHHHHHHHHHHHHHHHHHHHHHHHHHHHh");
		
		if (node is Button button)
			button.AddThemeFontOverride("font", font);
		else if (node is LineEdit lineEdit)
			lineEdit.AddThemeFontOverride("font", font);

		foreach (var child in node.GetChildren())
			ApplyFont(child, font);
	}
}
