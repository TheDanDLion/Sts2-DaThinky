using Godot;

namespace DaThinky.Scenes;

public partial class Equals : Button
{
	private LineEdit _lineEdit = null!;

	public override void _Ready()
	{
		_lineEdit = GetOwner().GetNode<LineEdit>("%CalculationLine");
		
		Pressed += () =>
		{
			GetOwner<ThoughtBubble>().JustSolved = true;
			
			var equation = _lineEdit.Text;
			if (string.IsNullOrWhiteSpace(equation)) return;

			var expression = new Expression();
			if (expression.Parse(equation) != Error.Ok) return;

			var result = expression.Execute();
			if (!expression.HasExecuteFailed())
				_lineEdit.Text = equation + " = " + result;
		};
	}
}
