using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace DaThinky.Scenes;

public partial class ThoughtBubble : Control
{
	// Settings to control the fade
	private float _fade1 = 0.16F;
	private float _fade2 = 0.32F;
	private float _fade3 = 0.55F;
	
	private Control _dot1 = null!;
	private Control _dot2 = null!;
	private Control _thoughtBubble = null!;
	private Control _calculator = null!;

	public bool JustSolved;
	
	public override void _Ready()
	{
		var font = GD.Load<Font>("res://fonts/kreon_bold.ttf");
		if (font == null) return;
		
		ApplyFont(this, font);
		
		_dot1 = GetNode<Control>("%Dot1");
		_dot2 = GetNode<Control>("%Dot2");
		_thoughtBubble = GetNode<Control>("%ThoughtBubble");
		_calculator = GetNode<Control>("%Calculator");
		
		// Hide bubble so it does not appear initially. We set modularity so we can do a lil fade
		_dot1.Modulate = Colors.Transparent;
		_dot2.Modulate = Colors.Transparent;
		_thoughtBubble.Modulate = Colors.Transparent;
		_calculator.Modulate = Colors.Transparent;
	}

	public void ShowBubble()
	{
		var tweenDot1 = CreateTween();
		tweenDot1.TweenProperty(_dot1, "modulate", Colors.White, _fade1).SetTrans(Tween.TransitionType.Sine);

		var tweenDot2 = CreateTween();
		tweenDot2.TweenProperty(_dot2, "modulate", Colors.White, _fade2).SetTrans(Tween.TransitionType.Sine);

		var tweenThought = CreateTween();
		tweenThought.TweenProperty(_thoughtBubble, "modulate", Colors.White, _fade3).SetTrans(Tween.TransitionType.Sine);
		
		var tweenCalc = CreateTween();
		tweenCalc.TweenProperty(_calculator, "modulate", Colors.White, _fade3).SetTrans(Tween.TransitionType.Sine);
	}

	public void HideBubble()
	{
		var tweenDot1 = CreateTween();
		tweenDot1.TweenProperty(_dot1, "modulate", Colors.Transparent, _fade3).SetTrans(Tween.TransitionType.Sine);

		var tweenDot2 = CreateTween();
		tweenDot2.TweenProperty(_dot2, "modulate", Colors.Transparent, _fade2).SetTrans(Tween.TransitionType.Sine);

		var tweenThought = CreateTween();
		tweenThought.TweenProperty(_thoughtBubble, "modulate", Colors.Transparent, _fade1).SetTrans(Tween.TransitionType.Sine);
		
		var tweenCalc = CreateTween();
		tweenCalc.TweenProperty(_calculator, "modulate", Colors.Transparent, _fade1).SetTrans(Tween.TransitionType.Sine);
	}
	
	private static void ApplyFont(Node node, Font font)
	{
		if (node is Button button)
			button.AddThemeFontOverride("font", font);
		else if (node is LineEdit lineEdit)
			lineEdit.AddThemeFontOverride("font", font);

		foreach (var child in node.GetChildren())
			ApplyFont(child, font);
	}
}
