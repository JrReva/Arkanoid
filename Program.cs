using System;

namespace Arkanoid
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            String nom = "";
            String prenom = "J-R";
            bool mouse = false;

            if (args.Length == 3)
            {
                prenom = args[0];
                nom = args[1];
                mouse = (args[2] == "y");
            }

            using (Game1 game = new Game1(prenom, nom, mouse))
            {
                game.Run();
            }
        }
    }
}

