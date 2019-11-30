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

        public override string DamageText()
        {
            return base.DamageText();
        }
    }

    public class BasicSpell : Spell
    {
        public BasicSpell(string name, Entity c) : base(name) 
        {
            UpdateDamage(c);
            Cost = 0; 
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

    public class IntermediateSpell: Spell
    {
        public IntermediateSpell(string name, Entity c) : base(name)
        {
            UpdateDamage(c);
            Cost = 10;
        }

        public override void UpdateDamage(Entity c)
        {
            int min = (c.Level + c.Power)/2;
            int max = c.Level+ c.Power;
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
