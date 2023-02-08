using System.Globalization;

namespace Havit.Bonusario.Web.Client.Components;

public partial class HxSplitView
{
	[Parameter] public RenderFragment FirstPartTemplate { get; set; }
	[Parameter] public RenderFragment SecondPartTemplate { get; set; }

	[Parameter] public string CssClass { get; set; }
	[Parameter] public string HandleCssClass { get; set; }

	private bool resizing = false;
	private double lastMouseXPosition;

	private readonly double resizeSmoothnessCoeficient = 2;

	private double baseOffset = 320;
	private double resizeOffset;

	private void HandleMouseDown(MouseEventArgs mouseEventArgs)
	{
		resizing = true;
		lastMouseXPosition = mouseEventArgs.PageX;
	}

	private void HandleMouseMove(MouseEventArgs mouseEventArgs)
	{
		if (!resizing)
		{
			return;
		}

		resizeOffset += (mouseEventArgs.PageX - lastMouseXPosition) * resizeSmoothnessCoeficient;
		lastMouseXPosition = mouseEventArgs.PageX;

		if (resizeOffset + baseOffset < 0)
		{
			resizeOffset = -baseOffset;
		}
	}

	private void HandleMouseUp()
	{
		resizing = false;
	}

	private string GetFirstPartStyle()
	{
		double flexBasis = resizeOffset + baseOffset;

		NumberFormatInfo numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
		return $"flex: 1 1 {flexBasis.ToString(numberFormatInfo)}px";
	}
}
