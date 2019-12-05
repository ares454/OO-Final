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


        public int Might { get; protected set; }
        public int Fortitude { get; protected set; }
        public int Power { get; protected set; }
        public int Knowledge { get; protected set; }
        public int Level { get; protected set; }
        public int Experience { get; protected set; }

        public bool IsAlive() { return currentHealth > 0; }

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
            string s = (target is Enemy ? $"a {target.Name}" : target.Name);
            string r = this is Enemy ? $"A {Name}" : Name;
            return $"{r} {attack.DamageText()} {(target is Player ? "you" : s)} for {dmg} damage.";
        }

        protected int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }

            protected set
            {
                currentHealth = value;
                if (currentHealth <= 0)
                    CommandPost.GetInstance().AddCommand(new DeathCommand(this));
            }
        }

        abstract protected int TakeDamage(Attack attack);


        //Returns a list of randomly generated attributes (Note: Ordered)
        protected int[] RollAttributes()
        {
            int[] ret = new int[4];
            int min = 3;
            Random r = new Random();
            for (int i = 0; i < 4; ++i)
            {
                int val = Roll(6, min) + Roll(6, min) + Roll(6, min);
                if (val > 15)
                {
                    int bonus = Roll(6, min);
                    val += bonus;
                    if (bonus == 6)
                        val += Roll(6, min);
                }

                ret[i] = val;
            }
            Array.Sort(ret);
            Array.Reverse(ret);
            return ret;
        }
    }
}
