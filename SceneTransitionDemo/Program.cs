using System;

namespace SceneTransitionDemo
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameBase())
                game.Run();
        }
    }
}
