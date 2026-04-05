using Godot;
using System;
using DaThinky.Scenes;

public partial class AC : Button
{
	private LineEdit _lineEdit = null!;
	
	public override void _Ready()
	{
		_lineEdit = GetOwner().GetNode<LineEdit>("%CalculationLine");

		// add the text of the number to the line edit
		Pressed += () =>
		{
			_lineEdit.Text = "";
			GetOwner<ThoughtBubble>().JustSolved = true;
		};
	}
}
