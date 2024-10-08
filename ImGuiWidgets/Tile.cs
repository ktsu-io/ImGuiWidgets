namespace ktsu.ImGuiWidgets;

using System.Numerics;
using ImGuiNET;



public static partial class ImGuiWidgets
{
	public class TileWidgetResponseDelegates
	{
		public Action? OnClick { get; init; }
		public Action? OnDoubleClick { get; init; }
		public Action? OnRightClick { get; init; }
		public Action? OnContextMenu { get; init; }
	}

	public static bool Tile(string id, float width, float padding, Action? onShow) =>
	TileImpl.Show(id, width, padding, onShow, new TileWidgetResponseDelegates());
	public static bool Tile(string id, float width, float padding, Action? onShow, TileWidgetResponseDelegates responseDelegates) =>
		TileImpl.Show(id, width, padding, onShow, responseDelegates);

	internal static class TileImpl
	{
		public static bool Show(string id, float width, float padding, Action? onShow, TileWidgetResponseDelegates responseDelegates)
		{
			bool wasClicked = false;

			ImGui.PushID(id);

			bool isHovered = false;
			var cursorScreenStartPos = ImGui.GetCursorScreenPos();

			ImGui.BeginGroup();
			var cursorStartPos = ImGui.GetCursorPos();
			ImGui.Dummy(new Vector2(0, padding));
			ImGui.Indent(padding);
			onShow?.Invoke();
			ImGui.Unindent(padding);
			var cursorEndPos = ImGui.GetCursorPos() + new Vector2(width + (padding * 2), padding);
			ImGui.EndGroup();

			var contentSize = cursorEndPos - cursorStartPos;

			ImGui.SetCursorScreenPos(cursorScreenStartPos);
			ImGui.Dummy(contentSize);

			isHovered = ImGui.IsItemHovered();

			if (isHovered)
			{
				if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
				{
					responseDelegates.OnClick?.Invoke();
					wasClicked = true;
				}
				if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
				{
					responseDelegates.OnDoubleClick?.Invoke();
				}
				if (ImGui.IsMouseClicked(ImGuiMouseButton.Right))
				{
					responseDelegates.OnRightClick?.Invoke();
				}
				if (ImGui.IsMouseReleased(ImGuiMouseButton.Right) && responseDelegates.OnContextMenu is not null)
				{
					ImGui.OpenPopup($"{id}_Context");
				}
			}

			if (isHovered)
			{
				uint color = ImGui.GetColorU32(ImGui.GetStyle().Colors[(int)ImGuiCol.Border]);
				ImGui.GetWindowDrawList().AddRect(cursorScreenStartPos, cursorScreenStartPos + contentSize, color);
			}

			if (ImGui.BeginPopup($"{id}_Context"))
			{
				responseDelegates.OnContextMenu?.Invoke();
				ImGui.EndPopup();
			}

			ImGui.PopID();

			return wasClicked;
		}

		public static bool Show(string id, float width, float padding, Action? onShow) => Show(id, width, padding, onShow, new());
	}
}
