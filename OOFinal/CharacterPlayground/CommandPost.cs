using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CharacterPlayground
{
    class CommandPost
    {
        ArrayList commands;
        static private CommandPost cp;
        private CommandPost() 
        {
            commands = new ArrayList();
        }
        public static CommandPost GetInstance()
        {
            if (cp == null)
                cp = new CommandPost();
            return cp;
        }
        public void AddCommand(Command c) { commands.Add(c); }

        public void Execute()
        {
            while(commands.Count != 0)
            {
                Command c = commands[0] as Command;
                c.Execute();
                commands.RemoveAt(0);
            }
        }
    }
}
