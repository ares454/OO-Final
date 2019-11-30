using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterPlayground
{
    public abstract class Entity
    {
        //Helper function. Not necessary in diagrams
        static private Random roller = new Random();
        static public int Roll(int numSides, int min = 1) { return roller.Next(min, numSides + 1); }
        public string Name { get; protected set; }

        //Derived stats. Have public accessor
        public int Health { get; protected set; }
        public int Stamina { get; protected set; }
        public int Mana { get; protected set; }


        public int Might        {get; protected set;}
        public int Fortitude    {get; protected set;}
        public int Power        {get; protected set;}
        public int Knowledge    {get; protected set;}
        public int Level { get; protected set; }

        virtual protected void DeriveStats()
        {
            Health = Fortitude * 2 + Fortitude * Level;
            Stamina = ((Fortitude + Might) * Level);
            Mana = (Knowledge << 1) * Level;
        }

        virtual public string Attack(Attack attack, Entity target)
        {
            int dmg = target.TakeDamage(attack);
            attack.ActivateEffect(this, target);
            return $"{Name} {attack.DamageText()} {(target is Enemy ? $"a {target.Name}" : target.Name)} for {dmg} damage.";
        }

        abstract protected int TakeDamage(Attack attack);
    }
}
