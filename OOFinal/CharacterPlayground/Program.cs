using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.GetInstance().Loop();

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
