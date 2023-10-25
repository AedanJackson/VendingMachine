using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine
{
    internal class Program
    {
        // Snack class
        private class Snack
        {
            public string Name;
            public double Price;
            public int Stock;

            // Assigns the provided name, price and stock to name, price and stock. If stock is >30, stock is maxxed out.
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
            public void AddStock(int StockAdded)
            {
                this.Stock += StockAdded;
                if (this.Stock > 30)
                {
                    this.Stock = 30;
                }
            }

            public void StockSold()
            {
                Stock--;
            }
        }

        // Handles the contents of the Snacks list
        private class SnacksHandler
        {
            public List<Snack> Snacks = new List<Snack>();
            public SnacksHandler()
            {
                ReadSnacks();
            }

            // Procedure to assign the snacks new values
            public void SetUpSnacks()
            {
                this.Snacks.Add(new Snack("Pringles Sour Cream and Onion", 1.20, 7));
                this.Snacks.Add(new Snack("Walkers Max Paprika", 1.00, 13));
                this.Snacks.Add(new Snack("Doritos Chilli Heatwave", 1.10, 4));
                this.Snacks.Add(new Snack("Doritos Tangy Cheese", 1.10, 14));
                this.Snacks.Add(new Snack("Nik Naks Nice'N'Spicy", 1.00, 22));
                this.Snacks.Add(new Snack("Nakd Blueberry Muffin", 0.95, 30));
                this.Snacks.Add(new Snack("Nakd Cherry Bakewell", 0.95, 24));
                this.Snacks.Add(new Snack("Monster Munch Pickled Onion", 1.30, 6));
                this.Snacks.Add(new Snack("Kinder Bueno", 0.90, 10));
                this.Snacks.Add(new Snack("Twix Caramel and Milk Chocolate Fingers", 1.00, 17));
                this.Snacks.Add(new Snack("Tony's Chocolonely Caramel Sea Salt", 1.70, 25));
                this.Snacks.Add(new Snack("Yorkie Original", 0.9, 30));
            }
            // Write the list of snacks to a file
            public void WriteSnacks()
            {
                FileStream SnacksFile = new FileStream("SnackInfo.bin", FileMode.Create);
                BinaryWriter BW = new BinaryWriter(SnacksFile);
                foreach (Snack Snack in this.Snacks)
                {
                    BW.Write(Snack.Name);
                    BW.Write(Snack.Price);
                    BW.Write(Snack.Stock);
                }
                BW.Close();
                SnacksFile.Close();
            }

            // Read the list of snacks from a file
            public void ReadSnacks()
            {
                try
                {
                    FileStream SnacksFile = new FileStream("SnackInfo.bin", FileMode.Open, FileAccess.Read);
                    BinaryReader BR = new BinaryReader(SnacksFile);
                    while (BR.BaseStream.Length > BR.BaseStream.Position)
                    {
                        this.Snacks.Add(new Snack(BR.ReadString(), BR.ReadDouble(), BR.ReadInt32()));
                    }
                    BR.Close();
                    SnacksFile.Close();
                }
                catch
                {
                    // File does not exist, so put in some default values for snacks which will then be written when the program ends.
                    SetUpSnacks();
                }
            }
        }
        
        // holds info about the money input, 
        public class UserOptions
        {
            public double MoneyInserted;

            //Adds coins to MoneyInserted
            public void InsertCoins(double CoinAdded)
            {
                this.MoneyInserted += CoinAdded;
                this.MoneyInserted = Math.Round(this.MoneyInserted, 2);
            }

            // Removes money from MoneyInserted
            public void RemoveBalance(double MoneyRemoved)
            {
                this.MoneyInserted -= MoneyRemoved;
                this.MoneyInserted = Math.Round(this.MoneyInserted, 2);
            }
        }

        // Class that holds all the things the admin can do WIP
        public class AdminOptions
        {
            public const string pwd = "3341"; // Stores the admin password

        }

        // Main program
        static void Main(string[] args)
        {
            SnacksHandler SnacksList = new SnacksHandler();
            UserOptions Balance = new UserOptions();
            string[] Options = { "insert coins", "select product", "exit", "admin" }; 
            while (true)
            {
                bool BreakLoop = false;
                while (true)
                {
                    DisplayWelcomeScreen(SnacksList.Snacks, Options);
                    HandleUserSelection(Balance, SnacksList, Options, ref BreakLoop);
                    SnacksList.WriteSnacks();
                }
            }
        }

        // TO DO:
        // FIX FLOATING POINT ERROR
        // 

        // Notes to self and maybe stuart: what I'd ideally like to do (conceptually) is make an array full of procedures or something like that, so I'd have
        // something like Options = { InsertCoins(), SelectProduct(), Admin(), Exit() }. This way I can make things more scalable and avoid selection, so e.g. 
        // I can do RUN Options[UserSelection - 1] instead of if UserSelection = 2{RUN SelectProduct();}

        // Maybe find out if it's possible to do something like this with classes? I really want to try and make the code as scalable as possible, so if I want
        // to add a new option all I have to do is add a new procedure and then the rest of the program is updated automatically.

        static void HandleUserSelection(UserOptions UserOptions, SnacksHandler SnacksList, string[] Options, ref bool BreakLoop)
        {
            // Checks the input of the user is valid
            int UserOption = 0;
            bool ValidOption = false;
            do
            {
                Console.Write("Enter action: ");
                try
                {
                    UserOption = int.Parse(Console.ReadLine());
                    if (UserOption > Options.Length || UserOption < 1)
                    {
                        Console.WriteLine("Not a valid option");
                    }
                    else
                    {
                        ValidOption = true;
                    }
                }
                catch
                {
                    Console.WriteLine("That is not a number");
                }
            } while (!ValidOption);

            switch (UserOption)
            {
                case 1:
                    InsertCoins(UserOptions);
                    break;
                case 2:
                    SelectItem(UserOptions, SnacksList);
                    break;
                case 3:
                    BreakLoop = true;
                    return;
            }
        }

        static void SelectItem(UserOptions UserOptions, SnacksHandler SnacksList)
        {
            
            bool DoneChoosingSnack = false;
            while (!DoneChoosingSnack)
            {
                Console.Write("Enter the snack you would like to buy, or press E to exit: ");
                string UserInput = Console.ReadLine();
                int ChosenSnackCode = 0;
                if (UserInput == "E")
                {
                    DoneChoosingSnack = true;
                }
                else
                {
                    // Checks if the snack code is an integer, then checks if it exists, then checks if it is in stock.
                    try
                    {
                        ChosenSnackCode = int.Parse(UserInput);
                        try
                        {
                            if (SnacksList.Snacks[ChosenSnackCode - 1].Stock > 0)
                            {
                                if (UserOptions.MoneyInserted - SnacksList.Snacks[ChosenSnackCode - 1].Price >= 0)
                                {
                                    UserOptions.RemoveBalance(SnacksList.Snacks[ChosenSnackCode - 1].Price); // Removes the cost of the snack from the balance.
                                    Console.WriteLine("Snack Bought successfully. Dispensing...");
                                    SnacksList.Snacks[ChosenSnackCode - 1].StockSold();
                                    // Refund section
                                    bool DoneRefunding = true;
                                    if (UserOptions.MoneyInserted > 0)
                                    {
                                        DoneRefunding = false;
                                    }
                                    while (!DoneRefunding)
                                    {
                                        Console.Write($"Would you like to refund £{UserOptions.MoneyInserted}? Cannot dispense 1/2p. Y/N ");
                                        string DoRefund = Console.ReadLine().ToUpper();
                                        if (DoRefund == "Y")
                                        {
                                            Console.WriteLine("Dispensing Change...");
                                            double[] Coins = { 2, 1, 0.5, 0.2, 0.1, 0.05 };
                                            while (Math.Round(UserOptions.MoneyInserted, 2) > 0.04) // 0.04 because can't dispense 1/2p
                                            {
                                                for (int i = 0; i < Coins.Length; i++)
                                                {
                                                    if (UserOptions.MoneyInserted - Coins[i] > 0)
                                                    {
                                                        UserOptions.RemoveBalance(Coins[i]);
                                                        Console.WriteLine($"Dispensed {Coins[i]}");
                                                    }
                                                }
                                            }
                                        }
                                        else if (DoRefund == "N")
                                        {
                                            DoneRefunding = true;
                                        }
                                        else // incorrect input entered
                                        {
                                            Console.WriteLine("Enter Y/N");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Not enough balance to buy. You have £{UserOptions.MoneyInserted}, you need £{SnacksList.Snacks[ChosenSnackCode - 1].Price}");
                                }
                                DoneChoosingSnack = true;
                            }
                            else
                            {
                                Console.WriteLine("That snack is out of stock");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("That snack does not exist");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("That is not a valid item code");
                    }
                }
            }
        }

        static void InsertCoins(UserOptions UserOptions)
        {
            double[] Coins = { 0.05, 0.1, 0.2, 0.5, 1, 2 };
            bool DoneAddingMoney = false;
            double CurrentCoin = 0;
            while (!DoneAddingMoney)
            {
                UserOptions.InsertCoins(CurrentCoin);
                Console.Write($"Current balance: £{UserOptions.MoneyInserted} Insert coin or press E to exit: "); // Clean this up so that it updates instead of writing a new line.
                string UserInput = Console.ReadLine().ToUpper();
                if (UserInput == "E")
                {
                    DoneAddingMoney = true;
                }
                else
                {
                    try
                    {
                        CurrentCoin = double.Parse(UserInput);
                        if (CurrentCoin < 0 || Array.IndexOf(Coins, CurrentCoin) < 0 || Math.Round(CurrentCoin, 2) != CurrentCoin)
                        {
                            Console.WriteLine("Invalid coin");
                            CurrentCoin = 0;
                        }
                    }
                    catch
                    {
                        Console.WriteLine();
                        Console.WriteLine("Not a coin!");
                        CurrentCoin = 0;
                    }
                }
            }
        }

        // Writes a welcome screen, displays all of the items, prices etc, lists the actions available to the user.
        static void DisplayWelcomeScreen(List<Snack> AllSnacks, string[] Options)
        {
            // Welcome screen
            // Length is 46? I think?
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("+               _                           +");                //               _                          
            Console.WriteLine("+ __      _____| | ___ ___  _ __ ___   ___  +");                // __      _____| | ___ ___  _ __ ___   ___ 
            Console.WriteLine("+ \\ \\ /\\ / / _ \\ |/ __/ _ \\| '_ ` _ \\ / _ \\ +");         // \ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \
            Console.WriteLine("+  \\ V  V /  __/ | (_| (_) | | | | | |  __/ +");               //  \ V  V /  __/ | (_| (_) | | | | | |  __/
            Console.WriteLine("+   \\_/\\_/ \\___|_|\\___\\___/|_| |_| |_|\\___| +");          //   \_/\_/ \___|_|\___\___/|_| |_| |_|\___|
            Console.WriteLine("+                                           +");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            
            // List available snacks, prices and stock
            int SnackIndex = 1;
            foreach (Snack CurrentSnack in AllSnacks)
            {
                Console.Write($"Option {SnackIndex, 2}: {CurrentSnack.Name, 40}, £{CurrentSnack.Price, 5}, ");
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

            // Display options
            for (int i  = 0; i < Options.Length - 1; i++)
            {
                Console.WriteLine($"Action {i + 1}: {Options[i]}");
            }
        }
    }
}
