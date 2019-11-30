using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterPlayground
{
    class AttackModifier : Attack
    {
        public AttackModifier(string name) : base(name) { }
        public AttackModifier(Attack a) : base(a) { }

        
    }

    class Repeater : AttackModifier
    {
        private int interval;
        public int NumberOfHits { get; private set; }
        public Repeater(string name, int interval) : base(name) { this.interval = interval; }

        public Repeater(Attack r, int interval) : base(r) { this.interval = interval; }

        public override void UpdateDamage(Entity c)
        {
            NumberOfHits = c.Level / interval;
            Cost *= NumberOfHits;
        }

        public override void ActivateEffect(Entity attacker, Entity defender)
        {
            for (int i = 0; i < NumberOfHits; ++i)
            {
                CommandPost.GetInstance().AddCommand(new AttackCommand(attacker, new Repeater(this, int.MaxValue), defender));
                RiposteCommand.GetInstance().ToRiposte(defender);
            }
        }

        public override string Description()
        {
            return $"{base.Description()} {NumberOfHits + 1} times";
        }
    }
}
