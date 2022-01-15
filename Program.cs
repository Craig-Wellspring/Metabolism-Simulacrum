using System;
using System.Threading;

namespace MetabolismSim
{
    public class Program
    {
        // Timer
        static Timer timer;
        static int programDuration = 100000;
        static int tickRate = 250; // in ms
        static int ti, cti = 0;
        static bool tenthFrame = false;

        // Vitals
        static float currentHP = 3;
        static float maxHP = 3;

        static int currentStam = 100;
        static int maxStam = 100;

        static int energy = 100;
        static int metaEfficiency = 20;

        static int dna = 0;

        public static void Main(string[] args)
        {
            timer = new Timer(Metabolism, null, 0, tickRate);
            Thread.Sleep(programDuration);
            timer.Dispose();
        }

        private static void Metabolism(Object state)
        {
            cti = ti / 10; // Cache previous tick index
            ti++; // Increment tick index
            if (ti / 10 > cti) tenthFrame = true;


            // Die if HP reaches 0
            if (currentHP < 1)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                Console.WriteLine("Creature has died.");
                Console.Read();
                return;
            }

            // Simulate Stamina use
            if (tenthFrame)
            {
                currentStam -= 5;
            }


            // METABOLIC LOOP
            if (energy > 0) // If energy remains
            {
                energy--; // Burn energy
                if (currentHP == maxHP) dna++; // If HP Max, gain DNA
                else currentHP += 0.01f; // If HP not Max, regenerate HP fraction
            }
            else // If no energy remains
            {
                if (currentHP > 0) // If HP remains
                {
                    currentHP--; // Sacrifice HP for energy at Metabolism Efficiency Rate
                    energy += metaEfficiency - 1;
                }
            }

            Console.WriteLine($"E: {energy}, HP: {Math.Round(currentHP)}, S: {currentStam}, DNA: {dna}");
            tenthFrame = false;
        }
    }
}
