using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterPlayground
{
    class IllegalBaseClassUseException : Exception { }
    class UnInstantiatedPlayerException : Exception { }
    abstract public class Player : Entity
    {
        static protected Player instance;
        protected Player() { Experience = 0;}

        protected Attack basic;
        protected Attack intermediate;
        protected Attack advanced;

        public int CurrentHealth { get; protected set; }
        public int CurrentStamina { get; protected set; }
        public int CurrentMana { get; protected set; }
        public int CurrentExperience { get; protected set; }
        public int Experience { get; protected set; }

        public void IncreaseFortitude(int num) { Fortitude += num; }
        public void IncreaseMight(int num) { Might += num; }
        public void IncreasePower(int num) { Power += num; }
        public void IncreaseKnowledge(int num) { Knowledge += num; }

        public Attack GetAttack(string input)
        {
            switch (input)
            {
                case "basic":
                    return basic;
                case "intermediate":
                    return intermediate;
                case "advanced":
                    return advanced;
                default:
                    return basic;
            }
        }

        public static Player GetInstance() 
        {
            if (instance == null)
                throw new UnInstantiatedPlayerException();
            return instance;
        }

        virtual public void LevelUp()
        {
            if (Level == 10)
                return;
            ++Level;
            DeriveStats();

            CurrentHealth = Health;
            CurrentMana = Mana;
            CurrentStamina = Stamina;
            Experience = 0;
            for (int i = 1; i <= Level; ++i)
                Experience += i;
            Experience *= 100;
        }

        virtual public string LevelUpGuide() { return "Gain 10% of total attribute points to distribute"; }

        //Returns a list of randomly generated attributes (Note: Ordered)
        protected int[] RollAttributes()
        {
            int[] ret = new int[4];
            int min = 3;
            Random r = new Random();
            for(int i = 0; i < 4; ++i)
            {
                int val = Roll(6, min) + Roll(6, min) + Roll(6, min);
                if(val > 15)
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

    public class Mage : Player
    {
        public static void New(string name)
        {
            instance = new Mage(name);
        }

        protected override int TakeDamage(Attack attack)
        {
            int damage = Roll(attack.Damage.Max, attack.Damage.Min);

            //Effectively a spell shield
            if (attack.Type == CharacterPlayground.Attack.AttackType.Spell)
            {
                damage -= (Knowledge / (11 - Level));
                damage = damage < 0 ? 0 : damage;
            }
            else
            {
                damage -= (Fortitude/ (21 - Level));
                damage = damage < 0 ? 0 : damage;
            }

            CurrentHealth -= damage;

            return damage;
        }
        
        public override string LevelUpGuide()
        {
            string text = "\tLevel 2: Missiles hits a second time when used.\n";
            text += "\tLevel 3:\n";
            text += "\tLevel 4: Missiles hits a third time when used.\n";
            text += "\tLevel 5:\n";
            text += "\tLevel 6: Missiles hits a fourth time when used. Arcane scream hits a second time\n";
            text += "\tLevel 7:\n";
            text += "\tLevel 8: Missiles hits a fifth time when used.\n";
            text += "\tLevel 9:\n";
            text += "\tLevel 10: Missiles hits a sixth time when used. Arcane Scream hits a third time\n";
            text += base.LevelUpGuide();
            return text;
        }

        public override void LevelUp()
        {
            if (Level == 10)
                return;
            base.LevelUp();

            basic = new BasicSpell("Missiles", this);
            intermediate = new IntermediateSpell("Lightning Bolt", this);
            advanced = new AdvancedSpell("Arcane Scream", this);

            if (Level >= 2)
            {
                basic = new Repeater(basic, 2);
                basic.UpdateDamage(this);
            }

            if(Level >= 5)
            {
                advanced = new Repeater(advanced, 5);
                advanced.UpdateDamage(this);
            }

        }

        private Mage(string name)
        {
            Name = name;
            Level = 5;
            int[] attributes = RollAttributes();
            Power = attributes[0] + 2;
            Knowledge = attributes[1] + 1;
            Fortitude = attributes[2];
            Might = attributes[3] - 1;
            DeriveStats();

            LevelUp();
        }
    }

    public class Warrior : Player
    {
        public static void New(string name)
        {
            instance = new Warrior(name);
        }

        private Warrior(string name)
        {
            Name = name;
            Level = 0;
            int[] attributes = RollAttributes();
            Might = attributes[0] + 2;
            Fortitude = attributes[1] + 1;
            Knowledge = attributes[2];
            Power = attributes[3] - 1;
            DeriveStats();

            LevelUp();
        }

        protected override int TakeDamage(Attack attack)
        {
            int damage = Roll(attack.Damage.Max, attack.Damage.Min);

            //Effectively a spell shield
            if (attack.Type == CharacterPlayground.Attack.AttackType.Spell)
            {
                damage -= (Knowledge / (21 - Level));
                damage = damage < 0 ? 0 : damage;
            }
            else
            {
                damage -= (Fortitude / (11 - Level));
                damage = damage < 0 ? 0 : damage;
            }

            CurrentHealth -= damage;

            return damage;
        }

        public override string LevelUpGuide()
        {
            string text = "\tLevel 2: \n";
            text += "\tLevel 3:\n";
            text += "\tLevel 4: \n";
            text += "\tLevel 5:\n";
            text += "\tLevel 6: \n";
            text += "\tLevel 7:\n";
            text += "\tLevel 8: \n";
            text += "\tLevel 9:\n";
            text += "\tLevel 10: \n";
            text += base.LevelUpGuide();
            return text;
        }
    }


}
