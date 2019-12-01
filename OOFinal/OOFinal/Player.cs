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

        public void Recover()
        {
            CurrentHealth += (Might + Fortitude) / 2;
            CurrentMana += Knowledge;
            CurrentStamina += Fortitude;

            if (CurrentStamina > Stamina)
                CurrentStamina = Stamina;
            if (CurrentMana > Mana)
                CurrentMana = Mana;
            if (CurrentHealth > Health)
                CurrentHealth = Health;
        }

        public override string Attack(Attack attack, Entity target)
        {
            if (attack.TypeOfAttack() == typeof(Spell) && attack.Cost > CurrentMana)
                return "Not enough mana to cast this spell.";
            else if (attack.TypeOfAttack() == typeof(Weapon) && attack.Cost > CurrentStamina)
                return "You are too tired to even raise your eyebrow.";

            if (attack.TypeOfAttack() == typeof(Spell))
                CurrentMana -= attack.Cost;
            else
                CurrentStamina -= attack.Cost;

            return base.Attack(attack, target);
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
            CurrentExperience -= Experience;
            Experience = 0;
            for (int i = 1; i <= Level; ++i)
                Experience += i;
            Experience *= 100;
        }

        public bool LevelUpAvailable() { return CurrentExperience > Experience; }

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

    public partial class Mage : Player
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
            string text = $"\tLevel 2: {basic.Name} hits an extra time\n";
            text += $"\tLevel 4: {basic.Name} hits an extra time\n";
            text += $"\tLevel 5: {advanced.Name} hits an extra time and {intermediate.Name} hits all enemies in a room\n";
            text += $"\tLevel 6: {basic.Name} hits an extra time\n";
            text += $"\tLevel 8: {basic.Name} hits an extra time\n";
            text += $"\tLevel 9: {basic.Name}'s damage is greatly increased\n";
            text += $"\tLevel 10: {basic.Name} hits an extra time and Arcane Scream hits a third time\n";
            text += base.LevelUpGuide();
            return text;
        }

        public override void LevelUp()
        {
            if (Level == 10)
                return;
            base.LevelUp();

            basic = new BasicSpell("Missiles", this);
            intermediate = new IntermediateSpell(Level >= 5 ? "Lightning Storm" : "Lightning Bolt", this);
            advanced = new AdvancedSpell("Arcane Scream", this);

            if (Level >= 2)
            {
                basic = new Repeater(basic, 2);
                basic.UpdateDamage(this);
            }

            if(Level >= 5)
            {
                intermediate = new AreaOfEffect(intermediate);
                intermediate.UpdateDamage(this);
                advanced = new Repeater(advanced, 5);
                advanced.UpdateDamage(this);
            }

            if(Level == 9)
            {
                basic = new AmplifyAttack(basic, 3);
                basic.UpdateDamage(this);
            }
        }

        private Mage(string name)
        {
            Name = name;
            Level = 0;
            int[] attributes = RollAttributes();
            Power = attributes[0] + 2;
            Knowledge = attributes[1] + 1;
            Fortitude = attributes[2];
            Might = attributes[3] - 1;
            DeriveStats();

            LevelUp();
        }
    }

    public partial class Warrior : Player
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
            string text = "";
            text += $"\tLevel 3: {basic.Name} hits an extra time\n";
            text += $"\tLevel 4: {advanced.Name}'s damage is greatly increased\n";
            text += $"\tLevel 5: {intermediate.Name} hits all enemies in the room\n";
            text += $"\tLevel 6: {basic.Name} hits an extra time\n";
            text += $"\tLevel 7: {advanced.Name}'s damage is greatly increased\n";
            text += $"\tLevel 9: {basic.Name} hits an extra time\n";
            text += $"\tLevel 10: {advanced.Name}'s damage is greatly increased\n";
            text += base.LevelUpGuide();
            return text;
        }

        public override void LevelUp()
        {
            if (Level == 10)
                return;
            base.LevelUp();

            basic = new BasicWeapon("Slice and Dice", this);
            intermediate = new IntermediateWeapon("Great Cleave", this);

            decimal scale = 1 + (decimal)(.2 * (Level - 1)) + (int)(Level / 3);
            advanced = new AmplifyAttack(new AdvancedWeapon("Limit Break", this), scale);


        }
    }


}
