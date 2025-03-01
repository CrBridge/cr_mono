using cr_mono.Core.GameLogic;
using Microsoft.Xna.Framework.Input;
using System;

namespace cr_mono.Core
{
    internal static class Data
    {
        internal static int ScreenWidth { get; set; } = 960;
        internal static int ScreenHeight { get; set; } = 540;
        internal static int NativeWidth { get; set; } = 480;
        internal static int NativeHeight { get; set; } = 270;

        internal static bool IsFullScreen { get; set; } = false;
        internal static bool Exit { get; set; } = false;

        internal enum Scenes { Menu, Game, Settings }
        internal static Scenes CurrentScene { get; set; } = Scenes.Menu;

        internal static KeyboardState PreviousKeyboardState { get; set; } = Keyboard.GetState();
        internal static KeyboardState CurrentKeyboardState { get; set; } = PreviousKeyboardState;

        //public static Random RNG { get; set; }
        public static RNG RNG { get; set; }
    }
}
