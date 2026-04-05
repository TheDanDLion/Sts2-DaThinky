using Godot;
using System;

namespace DaThinky.Scenes;

public partial class AddChar : Button
{
	private LineEdit _lineEdit = null!;
	
	public override void _Ready()
	{
		_lineEdit = GetOwner().GetNode<LineEdit>("%CalculationLine");

		// add the text of the number to the line edit
		Pressed += () =>
		{
			var owner = GetOwner<ThoughtBubble>();
			if (owner.JustSolved)
				_lineEdit.Text = "";
			owner.JustSolved = false;
			_lineEdit.Text += Text;
		};
	}
}
