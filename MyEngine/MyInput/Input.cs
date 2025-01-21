using SFML.System;
using SFML.Window;

namespace MyEngine;

public static class Input
{
    public static Vector2f MousePositionOnWindow;
    public static Vector2f MousePositionFromWindowCenter;

    public static void UpdateInput(Window window)
    {
        MousePositionOnWindow = (Vector2f)Mouse.GetPosition(window);

        MousePositionFromWindowCenter = MousePositionOnWindow - ((Vector2f)window.Size * 0.5f);
    }
}