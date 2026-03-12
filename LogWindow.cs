using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace DaThinky;

public partial class LogWindow : Window
{
	private RichTextLabel _label = null!;

	public override void _Ready()
	{
		Title = "Log Monitor";
		Size = new Vector2I(800, 400);
		InitialPosition = WindowInitialPosition.Absolute;
		Position = DisplayServer.WindowGetPosition() + new Vector2I(DisplayServer.WindowGetSize().X, 0);

		FontFile font = ResourceLoader.Load<FontFile>("res://Resources/Fonts/JetBrains/JetBrainsMono-Regular.ttf");

		_label = new RichTextLabel
		{
			BbcodeEnabled = true,
			ScrollFollowing = true,
		};
		_label.AddThemeFontOverride("normal_font", font);
		_label.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		AddChild(_label);

		Log.LogCallback += OnLog;
	}

	public override void _ExitTree()
	{
		Log.LogCallback -= OnLog;
	}

	private void OnLog(LogLevel level, string message, int frame)
	{
		string color = level switch
		{
			LogLevel.Error    => "red",
			LogLevel.Warn     => "yellow",
			LogLevel.Debug    => "cyan",
			LogLevel.VeryDebug => "gray",
			_                 => "white",
		};

		_label.AppendText($"[color={color}][{level}] {message}[/color]\n");
	}
}
