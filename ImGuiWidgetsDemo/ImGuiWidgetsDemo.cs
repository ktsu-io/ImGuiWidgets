namespace ktsu.io.ImGuiWidgetsDemo;

using ImGuiNET;
using ktsu.io.ImGuiApp;
using ktsu.io.ImGuiWidgets;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
internal class ImGuiWidgetsDemo
{
	private static void Main(string[] args)
	{
		ImGuiWidgetsDemo imGuiWidgetsDemo = new();
		ImGuiApp.Start(nameof(ImGuiWidgetsDemo), new ImGuiAppWindowState(), imGuiWidgetsDemo.Tick, imGuiWidgetsDemo.ShowMenu, imGuiWidgetsDemo.WindowResized);
	}

	private static float value = 0.5f;

	private readonly PopupInputString popupInputString = new();
	private readonly PopupFilesystemBrowser popupFilesystemBrowser = new();
	private readonly PopupMessageOK popupMessageOK = new();
	private string inputString = "String Input Popup";
	private bool ShouldOpenOKPopup { get; set; }
	private string OKPopupMessage { get; set; } = string.Empty;
	private string OKPopupTitle { get; set; } = string.Empty;
	private void Tick(float dt)
	{
		float ms = dt * 1000;
		Knob.Draw("DT", ref ms, 0, 10, 150f);
		ImGui.SameLine();
		Knob.Draw("Value", ref value, 0f, 1f, 150f);



		ImGui.SameLine();
		ColorIndicator.Show(ColorIndicator.Red, true);
		ImGui.SameLine();
		ColorIndicator.Show(ColorIndicator.Red, false);
		ImGui.SameLine();
		ColorIndicator.Show(ColorIndicator.Green, true);
		ImGui.SameLine();
		ColorIndicator.Show(ColorIndicator.Green, false);
		if (ImGui.Button(inputString))
		{
			popupInputString.Open("Enter a string", "Enter", "Yeet", (string result) =>
			{
				inputString = result;
			});
		}

		if (ImGui.Button("Open File"))
		{
			popupFilesystemBrowser.FileOpen("Open File", (f) =>
			{
				ShouldOpenOKPopup = true;
				OKPopupTitle = "File Chosen";
				OKPopupMessage = $"You chose: {f}";
			}, "*.cs");
		}

		if (ImGui.Button("Save File"))
		{
			popupFilesystemBrowser.FileSave("Save File", (f) =>
			{
				ShouldOpenOKPopup = true;
				OKPopupTitle = "File Chosen";
				OKPopupMessage = $"You chose: {f}";
			}, "*.cs");
		}

		if (ShouldOpenOKPopup)
		{
			popupMessageOK.Open(OKPopupTitle, OKPopupMessage);
			ShouldOpenOKPopup = false;
		}

		popupMessageOK.ShowIfOpen();
		popupInputString.ShowIfOpen();
		popupFilesystemBrowser.ShowIfOpen();
	}

	private void ShowMenu()
	{
	}

	private void WindowResized()
	{
	}
}
