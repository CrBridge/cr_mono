namespace cr_mono.Core
{
    public static class Data
    {
        public static int ScreenWidth { get; set; } = 768;
        public static int ScreenHeight { get; set; } = 432;

        public static bool Exit { get; set; } = false;

        public enum Scenes { Menu, Game, Settings }
        public static Scenes CurrentScene { get; set; } = Scenes.Menu;
    }
}
