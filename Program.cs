using System;
using System.Threading;

namespace MetabolismSim
{
    public class Program
    {
        // Timer
        static Timer timer;
        static int programDuration = 1000000;
        static int tickRate = 250; // in ms
        static int ti, cti = 0;
        static bool t10 = false;

        // Vitals
        static float currentHP = 3;
        static float maxHP = 3;

        static int currentStam = 100;
        static int maxStam = 100;

        static int energy = 100;
        static int maxEnergy = 100;
        static int metaEfficiency = 20;

        static float foodStorage = 0;
        static float maxFood = 100;

        static int dna = 0;

        public static void Main(string[] args)
        {
            timer = new Timer(MetabolicLoop, null, 0, tickRate);
            Thread.Sleep(programDuration);
            timer.Dispose();
        }

        private static void MetabolicLoop(Object state)
        {
            cti = ti / 10; // Cache previous tick index
            ti++; // Increment tick index
            if (ti / 10 > cti) t10 = true; // Track every 10th tick


            // Die if HP reaches 0
            if (currentHP < 1)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                Console.WriteLine("Creature has died.");
                Console.Read();
                return;
            }

            // Simulate foraging - Stamina use, Food gain
            if (t10)
            {
                if (energy > 0 && foodStorage < maxFood)
                {
                    currentStam -= new Random().Next(1, 20);
                    float foragedFood = new Random().Next(0, 20);
                    if (foodStorage + foragedFood <= maxFood)
                    {
                        foodStorage += foragedFood;
                    }
                }
            }

            // METABOLIC LOOP
            if (foodStorage > 0 && energy < maxEnergy) // Digest stored food
            {
                foodStorage -= 1;
                energy += 2;
            }
            if (energy > 0) // If energy remains
            {
                energy--; // Burn energy
                if (currentStam < maxStam && energy > 1) // If Stam not Max, regen Stam
                {
                    currentStam++;
                    energy--;
                }
                else if (currentHP < maxHP && energy > 1) // If HP not Max, regenerate HP fraction
                {
                    currentHP += 0.01f;
                    energy--;
                }
                else dna++; // If HP and Stam Max, gain DNA
            }
            else // If no energy remains
            {
                if (currentHP > 0) // If HP remains
                {
                    currentHP--; // Sacrifice HP for energy at Metabolism Efficiency Rate
                    energy += metaEfficiency - 1;
                }
            }

            Console.WriteLine($@"E: {energy}, F: {foodStorage}, DNA: {dna}
HP: {Math.Round(currentHP)}, S: {currentStam}
");
            t10 = false;
        }
    }
}
