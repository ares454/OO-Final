﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Roommaker maker = Roommaker.getInstance();
            maker.createMap();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
