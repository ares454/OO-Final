using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using OOFinal;

namespace CharacterPlayground
{
    abstract class Command
    {
        public abstract void Execute();
    }

    class AttackCommand : Command
    {
        Attack attack;
        Entity defender;
        Entity attacker;

        public AttackCommand(Entity a, Attack h, Entity b)
        {
            attacker = a;
            defender = b;
            attack = h;
        }

        public override void Execute()
        {
            if(defender.IsAlive())
               Console.WriteLine(attacker.Attack(attack, defender));
        }
    }

    class RiposteCommand : Command
    {
        private static RiposteCommand rc;
        HashSet<Entity> attackers;
        private RiposteCommand() { attackers = new HashSet<Entity>(); }

        public static RiposteCommand GetInstance()
        {
            if (rc == null)
                rc = new RiposteCommand();
            return rc;
        }

        public override void Execute()
        {
            foreach (Enemy e in attackers)
                if(e.IsAlive())
                    CommandPost.GetInstance().AddCommand(new AttackCommand(e, e.Weapon, Player.GetInstance()));

            attackers.Clear();
        }

        public void ToRiposte(Entity e)
        {
            if (e is Player)
                return;

            attackers.Add(e);
        }
    }

    class BossProclamationCommand : Command
    {
        string message;

        public BossProclamationCommand(string m) { message = m; }
        public override void Execute()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    class DeathCommand : Command
    {
        Entity deadman;

        public DeathCommand(Entity d) { deadman = d; }

        public override void Execute()
        {
            string deadname = deadman.Name;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (deadman is Player || deadman is Boss)
            {
                if (deadman is Player)
                    Console.WriteLine("You done messed up and died.");
                if (deadman is Boss)
                    Console.WriteLine($"Congratulations! You have killed {deadname}. You are free to leave");
                Game.GetInstance().Shutdown();
            }
            else
            {
                Console.WriteLine($"A {deadname} has been slain.");
                Player.GetInstance().CollectExperience(deadman.Experience);
                Game.GetInstance().CurrentRoom.RemoveDead(deadman as Race);
            }

            Console.ResetColor();
        }
    }
}
