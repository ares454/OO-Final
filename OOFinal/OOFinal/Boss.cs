using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterPlayground;

namespace OOFinal
{
    class Boss : Enemy
    {
        static Boss b;
        private Boss()
        {
            Player.GetInstance().AddLevelObserver(this);
            Name = "Magnus";
        }

        public static Boss GetInstance
        {
            get
            {
                if (b == null)
                    b = new Boss();

                return b;
            }
        }

        public override void Alert()
        {
            Player p = Player.GetInstance();
            string m = "";
            switch(p.Level)
            {
                case 2:
                    m = "You have survived my underlings well. Continue forward.";
                    break;
                case 4:
                    m = "You're still alive? Excellent...";
                    break;
                case 6:
                    m = "Yes...Take another step forward to your doom.";
                    break;
                case 8:
                    m = "So close...Just a few more steps to your doom.";
                    break;
                case 10:
                    m = "Welcome to my lair. Prepare to die.";
                    break;
            }

            if(m != "")
                CommandPost.GetInstance().AddCommand(new BossProclamationCommand(m));
        }

        protected override int TakeDamage(Attack attack)
        {
            throw new NotImplementedException();
        }
    }
}
