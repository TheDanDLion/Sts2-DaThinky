using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace DaThinky.Scenes;

public partial class ToggleButton : TextureButton
{
	private const float HoverAngle = -Mathf.Pi / 15f;
	private const float HoverAnimDur = 0.5f;
	private const float UnhoverAnimDur = 1.0f;
	private const float PressDownDur = 0.25f;

	private CancellationTokenSource? _hoverToken;
	private CancellationTokenSource? _unhoverToken;
	private CancellationTokenSource? _pressToken;
	private Action? _hotkeyAction;

	public override void _Ready()
	{
		MouseEntered += OnHover;
		MouseExited += OnUnhover;
		ButtonDown += OnPress;
		ButtonUp += OnRelease;
		Resized += () => PivotOffset = Size / 2f;

		_hotkeyAction = () => EmitSignal(SignalName.Pressed);
		NHotkeyManager.Instance?.PushHotkeyPressedBinding(Program.CalculatorHotkey, _hotkeyAction);
	}

	public override void _ExitTree()
	{
		_hoverToken?.Cancel();
		_unhoverToken?.Cancel();
		_pressToken?.Cancel();
		if (_hotkeyAction != null)
			NHotkeyManager.Instance?.RemoveHotkeyPressedBinding(Program.CalculatorHotkey, _hotkeyAction);
	}

	private void UpdatePivot() => PivotOffset = Size / 2f;

	private void OnHover()
	{
		UpdatePivot();
		_unhoverToken?.Cancel();
		_hoverToken?.Cancel();
		Scale = Vector2.One * 1.1f;
		Modulate = new Color(1.1f, 1.1f, 1.1f);
		_hoverToken = new CancellationTokenSource();
		TaskHelper.RunSafely(AnimHover(_hoverToken));
	}

	private void OnUnhover()
	{
		UpdatePivot();
		_hoverToken?.Cancel();
		_pressToken?.Cancel();
		_unhoverToken?.Cancel();
		_unhoverToken = new CancellationTokenSource();
		TaskHelper.RunSafely(AnimUnhover(_unhoverToken));
	}

	private void OnPress()
	{
		UpdatePivot();
		_hoverToken?.Cancel();
		_pressToken?.Cancel();
		_pressToken = new CancellationTokenSource();
		TaskHelper.RunSafely(AnimPressDown(_pressToken));
	}

	private void OnRelease()
	{
		UpdatePivot();
		_pressToken?.Cancel();
		_hoverToken?.Cancel();
		_hoverToken = new CancellationTokenSource();
		TaskHelper.RunSafely(AnimHover(_hoverToken));
	}

	private async Task AnimHover(CancellationTokenSource cancelToken)
	{
		float timer = 0f;
		float startAngle = Rotation;
		for (; timer < HoverAnimDur; timer += (float)GetProcessDeltaTime())
		{
			if (cancelToken.IsCancellationRequested) return;
			Rotation = Mathf.LerpAngle(startAngle, HoverAngle, Ease.BackOut(timer / HoverAnimDur));
			if (!GodotObject.IsInstanceValid(this) || !IsInsideTree()) return;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		Rotation = HoverAngle;
	}

	private async Task AnimUnhover(CancellationTokenSource cancelToken)
	{
		float timer = 0f;
		float startAngle = Rotation;
		Vector2 startScale = Scale;
		float startV = Modulate.R;
		for (; timer < UnhoverAnimDur; timer += (float)GetProcessDeltaTime())
		{
			if (cancelToken.IsCancellationRequested) return;
			Rotation = Mathf.LerpAngle(startAngle, 0f, Ease.ElasticOut(timer / UnhoverAnimDur));
			Scale = startScale.Lerp(Vector2.One, Ease.ExpoOut(timer / UnhoverAnimDur));
			float v = Mathf.Lerp(startV, 1.0f, Ease.ExpoOut(timer / UnhoverAnimDur));
			Modulate = new Color(v, v, v);
			if (!GodotObject.IsInstanceValid(this) || !IsInsideTree()) return;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		Rotation = 0f;
		Scale = Vector2.One;
		Modulate = Colors.White;
	}

	private async Task AnimPressDown(CancellationTokenSource cancelToken)
	{
		float timer = 0f;
		float startAngle = Rotation;
		float targetAngle = startAngle + Mathf.Pi / 9f;
		for (; timer < PressDownDur; timer += (float)GetProcessDeltaTime())
		{
			if (cancelToken.IsCancellationRequested) return;
			float t = Ease.CubicOut(timer / PressDownDur);
			Rotation = Mathf.LerpAngle(startAngle, targetAngle, t);
			float v = Mathf.Lerp(1.1f, 0.4f, t);
			Modulate = new Color(v, v, v);
			if (!GodotObject.IsInstanceValid(this) || !IsInsideTree()) return;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		Rotation = targetAngle;
		Modulate = new Color(0.4f, 0.4f, 0.4f);
	}
}
