using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterPlayground
{
    abstract public class Enemy : Entity
    {
        public Attack Weapon { get; protected set; }
        protected Enemy() { }
        public virtual void Alert() { }

        public Enemy(int level)
        {
            Level = level;
            
        }

        public Enemy(Enemy e)
        {
            this.Knowledge = e.Knowledge;
            this.Level = e.Level;
            this.Might = e.Might;
            this.Power = e.Power;
            this.Stamina = e.Stamina;
            this.Fortitude = e.Fortitude;
            DeriveStats();
            currentHealth = Health;

            Experience = e.Experience;
        }


        protected override int TakeDamage(Attack attack)
        {
            int damage = Roll(attack.Damage.Max, attack.Damage.Min);
            damage -= Level;

            CurrentHealth -= damage;
            Console.WriteLine($"System: {Name} has {CurrentHealth} health left");
            return damage;
        }
    }

    public class Race : Enemy
    {
        protected Enemy eClass;
        public Race(Enemy e) : base(e)
        {

        }
    }

    public class Class : Enemy
    {
        public Class(int level) : base(level)
        {

        }
    }
        

    public partial class Archer : Class
    {
        public Archer(int level) : base(level)
        {
            int[] stats = RollAttributes();
            Might = stats[0] + (level * 4);
            Fortitude = stats[1] + (level * 3);
            Knowledge = stats[2] + (level * 2);
            Power = stats[3] + (level);
            DeriveStats();
            CurrentHealth = Health;
            Experience = 10 * Level;
            Name = "skirmisher";

            Weapon = new Arrows(this);
        }
    }

    public partial class Fighter : Class
    {
        public Fighter(int level) : base(level)
        {
            int[] stats = RollAttributes();
            Fortitude = stats[0] + (level * 4);
            Might = stats[1] + (level * 3);
            Knowledge = stats[2] + (level * 2);
            Power = stats[3] + (level);
            DeriveStats();
            CurrentHealth = Health;
            Name = "swordsman";
            Experience = 20 * level;

            Weapon = new Sword(this);
        }
    }

    public partial class Wizard : Class
    {
        public Wizard(int level) : base(level)
        {
            int[] stats = RollAttributes();
            Power = stats[0] + (level * 4);
            Knowledge = stats[1] + (level * 3);
            Fortitude = stats[2] + (level * 2);
            Might = stats[3] + (level);
            DeriveStats();
            CurrentHealth = Health;
            Name = "wizard";
            Experience = 15 * level;

            Weapon = new Wand(this);
        }
    }

    public partial class Human : Race
    {
        public Human(Class c) : base(c)
        {
            eClass = c;
            Might += (eClass.Level);
            Fortitude += (eClass.Level);
            Weapon = new Repeater(c.Weapon, 4);
            Name = $"human {eClass.Name}";
            Experience += 10 * Level;
        }


    }

}
