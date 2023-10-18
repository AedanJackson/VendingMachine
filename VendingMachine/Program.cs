using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine
{
    internal class Program
    {
        private class Snack
        {
            public string Name;
            public double Price;
            public int Stock;

            public Snack(string name, double price, int stock)
            {
                if (stock > 30) // Because maximum stock is 30
                {
                    stock = 30;
                }
                this.Name = name;
                this.Price = price;
                this.Stock = stock;
            }
        }
        static void Main(string[] args)
        {
            const int SnacksLength = 12;
            Snack[] AllSnacks = new Snack[SnacksLength];
            SetUpSnacks(AllSnacks);
            DisplayWelcomeScreen(AllSnacks);
            Console.ReadLine();

        }

        // Procedure to assign the snacks
        static void SetUpSnacks(Snack[] AllSnacks)
        {
            AllSnacks[0] = new Snack("Pringles Sour Cream and Onion", 1.20, 7);
            AllSnacks[1] = new Snack("Walkers Max Paprika", 1.00, 13);
            AllSnacks[2] = new Snack("Doritos Chilli Heatwave", 1.10, 4);
            AllSnacks[3] = new Snack("Doritos Tangy Cheese", 1.10, 14);
            AllSnacks[4] = new Snack("Nik Naks Nice'N'Spicy", 1.00, 22);
            AllSnacks[5] = new Snack("Nakd Blueberry Muffin", 0.95, 30);
            AllSnacks[6] = new Snack("Nakd Cherry Bakewell", 0.95, 24);
            AllSnacks[7] = new Snack("Monster Munch Pickled Onion", 1.30, 6);
            AllSnacks[8] = new Snack("Kinder Bueno", 0.90, 10);
            AllSnacks[9] = new Snack("Twix Caramel and Milk Chocolate Fingers", 1.00, 17);
            AllSnacks[10] = new Snack("Tony's Chocolonely Caramel Sea Salt", 1.70, 25);
            AllSnacks[11] = new Snack("Yorkie Original", 0.9, 30);
        }
        
        static void DisplayWelcomeScreen(Snack[] AllSnacks)
        {
            // Length is 4_? I think?
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("+               _                           +");                //               _                          
            Console.WriteLine("+ __      _____| | ___ ___  _ __ ___   ___  +");                // __      _____| | ___ ___  _ __ ___   ___ 
            Console.WriteLine("+ \\ \\ /\\ / / _ \\ |/ __/ _ \\| '_ ` _ \\ / _ \\ +");         // \ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \
            Console.WriteLine("+  \\ V  V /  __/ | (_| (_) | | | | | |  __/ +");                //  \ V  V /  __/ | (_| (_) | | | | | |  __/
            Console.WriteLine("+   \\_/\\_/ \\___|_|\\___\\___/|_| |_| |_|\\___| +");          //   \_/\_/ \___|_|\___\___/|_| |_| |_|\___|
            Console.WriteLine("+                                           +");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            int SnackIndex = 1;
            foreach (Snack CurrentSnack in AllSnacks)
            {
                Console.Write($"Option {SnackIndex}: {CurrentSnack.Name, 40}, £{CurrentSnack.Price, 5}, ");
                if (CurrentSnack.Stock == 0)
                {
                    Console.WriteLine("(Out of stock)");
                }
                else
                {
                    Console.WriteLine($"({CurrentSnack.Stock} left)");
                }
                SnackIndex++;
            }
        }
    }
}
