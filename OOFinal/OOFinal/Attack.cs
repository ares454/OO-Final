using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterPlayground
{
    abstract public class Attack
    {
        public class Range
        {
            public int Min { get; private set; }
            public int Max { get; private set; }

            private Range() { }
            public Range(int min, int max)
            {
                Min = min;
                Max = max;
            }
        }
        public string Name { get; protected set; }
        public Range Damage { get; protected set; }
        public int Cost { get; protected set; }

        public enum AttackType { Physical, Spell}
        public AttackType Type { get; protected set; }
        
        public Attack(string name) { Name = name; }
        public virtual void ActivateEffect(Entity attacker, Entity defender) { }
        virtual public void UpdateDamage(Entity c) { }

        public Attack(Attack r) : this(r.Name)
        {
            Name = r.Name;
            Damage = r.Damage;
            Cost = r.Cost;
            Type = r.Type;
        }

        virtual public System.Type TypeOfAttack() { return this.GetType(); }

        virtual public string DamageText() { return ""; }

        virtual public string Description()
        {
            return $"{Name} does {Damage.Min}-{Damage.Max} damage to target";
        }
    }

    public class Spell : Attack
    {
        public Spell(string name) : base(name)
        {
            Type = AttackType.Spell;
        }

        public override Type TypeOfAttack()
        {
            return typeof(Spell);
        }
    }

    public class Weapon : Attack
    {
        public Weapon(string name) : base(name)
        {
            Type = AttackType.Physical;
        }

        public override Type TypeOfAttack()
        {
            return typeof(Weapon);
        }
    }

    partial class Mage
    {

        public class BasicSpell : Spell
        {
            public BasicSpell(string name, Entity c) : base(name)
            {
                UpdateDamage(c);
                Cost = 1;
            }


            public override void UpdateDamage(Entity c)
            {
                int min = c.Level + c.Power / 10;
                int max = c.Level + c.Power / 5;
                Damage = new Range(min, max);
            }

            public override string DamageText()
            {
                return "laughs as arcane bolts slam into";
            }
        }

        public class IntermediateSpell : Spell
        {
            public IntermediateSpell(string name, Entity c) : base(name)
            {
                UpdateDamage(c);
                Cost = 15;
            }

            public override void UpdateDamage(Entity c)
            {
                int min = (c.Level + c.Power) / 2;
                int max = c.Level + c.Power;
                Damage = new Range(min, max);
            }

            public override string DamageText()
            {
                return "calls lightning down to strike";
            }
        }
        public class AdvancedSpell : Spell
        {
            public AdvancedSpell(string name, Entity c) : base(name)
            {
                UpdateDamage(c);
                Cost = 30;
                Type = AttackType.Physical;
            }

            public override void UpdateDamage(Entity c)
            {
                int min = c.Power + c.Level;
                int max = c.Level + c.Power + c.Knowledge;
                Damage = new Range(min, max);
            }

            public override string DamageText()
            {
                return "screams, sending sharp streams of air into";
            }
        }
    }

    partial class Warrior
    {
        public class BasicWeapon : Weapon
        {
            public BasicWeapon(string n, Entity e) : base(n)
            {
                UpdateDamage(e);
                Cost = 1;
            }

            public override void UpdateDamage(Entity c)
            {
                int min = c.Level * 2;
                int max = min + c.Might / 3;
                Damage = new Range(min, max);
            }

        }

        public class IntermediateWeapon : Weapon
        {
            public IntermediateWeapon(string n, Entity e) : base(n)
            {
                UpdateDamage(e);
                Cost = 15;
            }

            public override void UpdateDamage(Entity c)
            {
                int min = c.Level + c.Might;
                int max = min + c.Might / 2;
                Damage = new Range(min, max);
            }

            public override string DamageText()
            {
                return "swings his sword in a great arc, cutting";
            }
        }

        public class AdvancedWeapon : Weapon
        {
            class FeedbackAttack : Weapon
            {
                public FeedbackAttack(string n, int i) : base(n) { Type = AttackType.Spell; SetDamage(i); }

                public override string DamageText()
                {
                    return "strains their muscles and hurts";
                }

                public void SetDamage(int i)
                {
                    int min = i / 5, max = min;
                    Damage = new Range(min, max);
                }
            }

            public AdvancedWeapon(string n, Entity e) : base(n)
            {
                UpdateDamage(e);
                Cost = 30;

            }

            public override void UpdateDamage(Entity c)
            {
                int min = c.Level + c.Might + c.Fortitude;
                int max = 2 * min;
                Damage = new Range(min, max);
            }

            public override void ActivateEffect(Entity attacker, Entity defender)
            {
                CommandPost.GetInstance().AddCommand(new AttackCommand(attacker, new FeedbackAttack("", Roll(Damage.Max, Damage.Min)), defender));
            }

            public override string DamageText()
            {
                return "taps into forbidden reserves of strength and swings wildly at";
            }

        }
    }
}
