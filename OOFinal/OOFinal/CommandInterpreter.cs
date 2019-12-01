using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CharacterPlayground
{
    class CommandInterpreter
    {
        delegate void Function(string[] p);
        delegate void Increase(int i);
        static CommandInterpreter ci;
        Dictionary<string, Function> commandList;

        Dictionary<string, Increase> levelCommands;
        private CommandInterpreter()
        {
            commandList = new Dictionary<string, Function>();
            commandList.Add("attack", Attack);
            commandList.Add("north", ChangeRoom);
            commandList.Add("south", ChangeRoom);
            commandList.Add("east", ChangeRoom);
            commandList.Add("west", ChangeRoom);
            commandList.Add("info", Info);
            commandList.Add("quit", Exit);
            commandList.Add("exit", Exit);
            commandList.Add("display", Display);
            commandList.Add("level", Level);
            commandList.Add("skills", Skills);
            commandList.Add("stats", Stats);
            commandList.Add("recover", Rest);
            commandList.Add("rest", Rest);

            levelCommands = new Dictionary<string, Increase>();
            levelCommands.Add("power", Player.GetInstance().IncreasePower);
            levelCommands.Add("might", Player.GetInstance().IncreaseMight);
            levelCommands.Add("fortitude", Player.GetInstance().IncreaseFortitude);
            levelCommands.Add("knowledge", Player.GetInstance().IncreaseKnowledge);
        }

        public static CommandInterpreter GetInstance()
        {
            if (ci == null)
                ci = new CommandInterpreter();
            return ci;
        }

        public void Interpret(string command)
        {
            string[] parts = command.Split(' '), param = new string[parts.Length - 1];
            Array.Copy(parts, 1, param, 0, param.Length);

                if (commandList.ContainsKey(parts[0]))
                    commandList[parts[0]].Invoke(param);
                else
                    Console.WriteLine("This command is unknown.");
        }

        private void Attack(string[] param)
        {
            //TODO: Implement when room is complete
            Attack attack = null;
            Player p = Player.GetInstance();
            try
            {
                switch (param[0])
                {
                    case "special":
                        attack = p.GetAttack("intermediate");
                        break;
                    case "ultimate":
                        attack = p.GetAttack("advanced");
                        break;
                    default:
                        attack = p.GetAttack(null);
                        break;
                }
            }catch(Exception) { Console.WriteLine("Huh? Who are you trying to hit?"); attack = p.GetAttack(null); }


            //TODO: Integrate rooms and enemies
            CommandPost.GetInstance().AddCommand(new AttackCommand(p, attack, p));
            CommandPost.GetInstance().AddCommand(RiposteCommand.GetInstance());
        }

        private void ChangeRoom(string[] param)
        {
            //TODO: Implement when room is complete
        }

        private void Info(string[] param)
        {
            if (param.Length == 0)
            {
                Console.WriteLine("What did you want to know? Need specifics.");
                return;
            }


            switch (param[0])
            {
                case "level":
                    Console.WriteLine(Player.GetInstance().LevelUpGuide());
                    break;
                default:
                    Console.WriteLine("No idea what that is.");
                    break;
            }
        }

        private void Exit(string[] param) { Game.GetInstance().Shutdown(); }

        private void Display(string[] param)
        {
            if (param.Length != 0)
            {
                switch (param[0])
                {
                    case "raw":
                    case "1":
                        Game.GetInstance().ChangeDisplayType("raw");
                        break;
                    case "percent":
                    case "%":
                        Game.GetInstance().ChangeDisplayType("percent");
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"Displaying stats as a {Game.GetInstance().DisplayType()}");
        }

        private void Level(string[] param)
        {
            Player p = Player.GetInstance();
            if (p.Level == 10)
                return;

            //if(!p.LevelUpAvailable()) { Console.WriteLine("You aren't ready for that. Go fight some things."); return; }

            int attributePoints = p.Might + p.Fortitude + p.Knowledge + p.Power;
            attributePoints /= 10;
            Console.Clear();
            Console.WriteLine($"Congratulations on reaching level {p.Level + 1}");

            while(attributePoints != 0)
            {
                Console.WriteLine($"You have {attributePoints} left to distribute.");
                Console.WriteLine($"\t1) Fortitude: {p.Fortitude}\n\t2) Knowledge: {p.Knowledge}\n\t3) Might: {p.Might}\n\t4) Power: {p.Power}");
                Console.WriteLine("Enter 1-4 to select an attribute. Optional: Enter a second number to allocate more than one point");
                Console.Write("Enter your selection: ");

                string[] input = Console.ReadLine().Split(' ');
                if (input.Length == 0)
                    continue;

                int selection;
                try
                {
                    selection = int.Parse(input[0]);
                    switch(selection)
                    {
                        case 1:
                            input[0] = "fortitude";
                            break;
                        case 2:
                            input[0] = "knowledge";
                            break;
                        case 3:
                            input[0] = "might";
                            break;
                        case 4:
                            input[0] = "power";
                            break;
                        default:
                            continue;
                    }
                }
                catch (Exception) { continue; }

                selection = 1;

                try
                {
                    selection = int.Parse(input[1]);
                    selection = attributePoints > selection ? selection : attributePoints;
                }
                catch (Exception) { }

                levelCommands[input[0]].Invoke(selection);
                attributePoints -= selection;
                Console.Clear();
            }

            p.LevelUp();
        }

        private void Skills(string[] param)
        {
            Player p = Player.GetInstance();
            Attack a = p.GetAttack("basic"), b = p.GetAttack("intermediate"), c = p.GetAttack("advanced");
            Console.WriteLine($"\t{a.Name} - {a.Description()}. Cost: {a.Cost}");
            Console.WriteLine($"\t{b.Name} - {b.Description()}. Cost: {b.Cost}");
            Console.WriteLine($"\t{c.Name} - {c.Description()}. Cost: {c.Cost}");
        }

        private void Stats(string[] param)
        {
            Player p = Player.GetInstance();
            Console.WriteLine($"Name : {p.Name}\t\tLevel: {p.Level}");
            Console.WriteLine($"Might: {p.Might}\t\tFortitude: {p.Fortitude}");
            Console.WriteLine($"Power: {p.Power}\t\tKnowledge: {p.Knowledge}");
        }

        private void Rest(string[] param)
        {
            Player.GetInstance().Recover();
            Console.WriteLine("You feel slightly better after a quick rest.");
        }
    }
}
