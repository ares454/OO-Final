using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomPlayground;
using OOFinal;

namespace CharacterPlayground
{
    class Game
    {
        private static Game g;
        bool active;
        public Room CurrentRoom { get; private set; }



        private Game() 
        {
            active = true;
            SetUp();
            func = PercentStats;
            Boss.GetInstance();
            CurrentRoom = new Room(1);
        }
        public static Game GetInstance()
        {
            if(g == null)
                g = new Game();
            return g;
        }

        delegate void Stats();
        Stats func;

        public void SetUp()
        {
            Console.WriteLine("Welcome to our little corner of reality.\nLet's have some fun.");
            Console.Write("First off, what is your name? ");
            string name = Console.ReadLine();
            Console.WriteLine("Are you a a wizard or a fighter?\n\t1) Wizard\n\t2) Fighter");
            int val = -1;
            do
            {
                Console.Write("Enter 1 or 2 to continue : ");
                try
                {
                    val = int.Parse(Console.ReadLine());
                }
                catch (Exception) { };
            } while (val != 1 && val != 2);

            if (val == 1)
                Mage.New(name);
            else
                Warrior.New(name);
        }

        public void Loop()
        {
            CommandInterpreter ci = CommandInterpreter.GetInstance();
            CommandPost cp = CommandPost.GetInstance();

            while (active)
            {
                func();
                Console.Write(">>");
                string input = Console.ReadLine().ToLower();
                ci.Interpret(input);
                cp.Execute();
            }

            Console.WriteLine("All right. We'll miss you. We aren't great shots.");
        }

        #region Stat Display
        void PercentStats()
        {
            Player p = Player.GetInstance();
            Console.Write("Health: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{(int)(100 * p.CurrentHealth / p.Health)}%");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Stamina: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{(int)(100 * p.CurrentStamina / p.Stamina)}%");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Mana: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{(int)(100 * p.CurrentMana / p.Mana)}%");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Experience: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{(int)(100 * p.CurrentExperience / p.Experience)}%");

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        void RawStats()
        {
            Player p = Player.GetInstance();
            Console.Write("Health: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{p.CurrentHealth} / {p.Health}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Stamina: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{p.CurrentStamina} / {p.Stamina}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Mana: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{p.CurrentMana} / {p.Mana}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Experience: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{p.CurrentExperience} / {p.Experience}");

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public string DisplayType()
        {
            if (func == PercentStats)
                return "percentage";
            else if (func == RawStats)
                return "raw value";
            else return null;
        }

        public void ChangeDisplayType(string s)
        {
            if (s.Equals("percent"))
                func = PercentStats;
            else if (s.Equals("raw"))
                func = RawStats;
        }
        #endregion

        public void Shutdown() { active = false; }
    }
}
