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
			{
				var isOperator = Text is "+" or "-" or "x" or "÷" or "%" or "*";
				if (isOperator)
				{
					var eqIndex = _lineEdit.Text.IndexOf(" = ", StringComparison.Ordinal);
					var result = eqIndex >= 0 ? _lineEdit.Text[(eqIndex + 3)..] : "";
					_lineEdit.Text = result;
				}
				else
				{
					_lineEdit.Text = "";
				}
			}
			owner.JustSolved = false;
			_lineEdit.Text += Text;
		};
	}
}
