using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
            Console.WriteLine(attacker.Attack(attack, defender));
        }
    }

    class RiposteCommand : Command
    {
        private static  RiposteCommand rc;
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
}
