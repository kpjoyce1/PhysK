using System;

namespace PhysK
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameApplication game = new GameApplication())
            {
                game.Run();
            }
        }
    }
#endif
}

