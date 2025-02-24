using Microsoft.Xna.Framework.Input;
using System;

namespace cr_mono.Core
{
    public static class Data
    {
        public static int ScreenWidth { get; set; } = 960;
        public static int ScreenHeight { get; set; } = 540;
        public static int NativeWidth { get; set; } = 480;
        public static int NativeHeight { get; set; } = 270;

        public static bool IsFullScreen { get; set; } = false;
        public static bool Exit { get; set; } = false;

        public enum Scenes { Menu, Game, Settings }
        public static Scenes CurrentScene { get; set; } = Scenes.Menu;

        public static KeyboardState previousKeyboardState { get; set; } = Keyboard.GetState();
        public static KeyboardState currentKeyboardState { get; set; } = previousKeyboardState;

        public static Random RNG { get; set; }
    }
}
