using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterPlayground;

namespace OOFinal
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            Game.GetInstance().Loop();
            Console.ReadKey();

       
        }
    }
}
