using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CharacterPlayground
{
    class AttackModifier : Attack
    {
        protected Attack attack;

        public override Type TypeOfAttack()
        {
            return attack.TypeOfAttack();
        }
        public AttackModifier(Attack a) : base(a) { attack = a; }
    }

    class Repeater : AttackModifier
    {
        private int interval;
        public int NumberOfHits { get; private set; }

        public Repeater(Attack r, int interval) : base(r)
        {
            this.interval = interval;
        }

        public override void UpdateDamage(Entity c)
        {
            NumberOfHits = c.Level / interval;
        }

        public override void ActivateEffect(Entity attacker, Entity defender)
        {
            for (int i = 0; i < NumberOfHits; ++i)
            {
                CommandPost.GetInstance().AddCommand(new AttackCommand(attacker, attack, defender));
                RiposteCommand.GetInstance().ToRiposte(defender);
            }
        }

        public override string DamageText()
        {
            return attack.DamageText();
        }

        public override string Description()
        {
            return $"{attack.Description()} {NumberOfHits + 1} times";
        }
    }

    class AreaOfEffect : AttackModifier
    {
        public AreaOfEffect(Attack r) : base(r) { }

        public override string Description()
        {
            return attack.Description() + " and all other enemies in the room";
        }

        public override void ActivateEffect(Entity attacker, Entity defender)
        {
            ArrayList enemies = Game.GetInstance().CurrentRoom.EnemyList;
            enemies.Remove(defender);
            foreach (Enemy e in enemies)
                CommandPost.GetInstance().AddCommand(new AttackCommand(attacker, attack, e));
            attack.ActivateEffect(attacker, defender);

        }

        public override string DamageText()
        {
            return attack.DamageText();
        }

        public override void UpdateDamage(Entity c)
        {
            attack.UpdateDamage(c);
        }
    }

    class AmplifyAttack : AttackModifier
    {
        decimal scale;
        public AmplifyAttack(Attack r, decimal d) : base(r) { scale = d; }

        public override void ActivateEffect(Entity attacker, Entity defender)
        {
            attack.ActivateEffect(attacker, defender);
        }

        public override void UpdateDamage(Entity c)
        {
            Damage = new Range((int)(Damage.Min * scale), (int)(Damage.Max * scale));
            Cost = (int)(scale * Cost);
        }

        public override string DamageText()
        {
            return attack.DamageText();
        }
    }
}
