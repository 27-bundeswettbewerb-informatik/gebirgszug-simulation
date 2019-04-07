using System;

namespace Gebirgszug_Simulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Gebirgszug_Simulation_Allgemein game = new Gebirgszug_Simulation_Allgemein())
            {
                game.Run();
            }
        }
    }
}

