using System.Text.RegularExpressions;
using Godot;

namespace DaThinky.Scenes;

public partial class Equals : Button
{
	private LineEdit _lineEdit = null!;

	public override void _Ready()
	{
		_lineEdit = GetOwner().GetNode<LineEdit>("%CalculationLine");

		Pressed += Evaluate;
		_lineEdit.TextSubmitted += _ => Evaluate();
	}

	private void Evaluate()
	{
		GetOwner<ThoughtBubble>().JustSolved = true;

		var equation = _lineEdit.Text;
		if (string.IsNullOrWhiteSpace(equation)) return;

		var normalized = equation.Replace("x", "*").Replace("÷", "/");
		// Append .0 to bare integers so Godot uses float division instead of integer division
		var floated = Regex.Replace(normalized, @"\b(\d+)\b(?![\d.])", "$1.0");
		var expression = new Expression();
		if (expression.Parse(floated) != Error.Ok) return;

		var result = expression.Execute();
		if (expression.HasExecuteFailed()) return;

		var d = result.AsDouble();
		var resultStr = d % 1 == 0 ? ((long)d).ToString() : d.ToString();
		_lineEdit.Text = equation + " = " + resultStr;
	}
}
