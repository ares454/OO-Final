using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using CharacterPlayground;

namespace RoomPlayground
{
    public class Room
    {
        HashSet<Enemy> enemies;
        int roomnum;
        public Room North{ get; private set; }
        public Room South{get; private set;}
        public Room West {get; private set;}
        public Room East {get; private set;}

        public ArrayList EnemyList { get { return new ArrayList(enemies.ToList<Enemy>()); } }

        public Room(int num)
        {
            roomnum = num;
            North = null;
            South = null;
            West = null;
            East = null;
        }

        public void setNorth(Room r)
        {
            North = r;
        }

        public void setSouth(Room r)
        {
            South = r;
        }

        public void setWest(Room r)
        {
            West = r;
        }

        public void setEast(Room r)
        {
            East = r;
        }

        public void print()
        {
            Console.WriteLine("Room number: " + this.roomnum);
            if(this.North != null)
            {
                Console.WriteLine("North: " + this.North.roomnum);
            }
            if (this.South != null)
            {
                Console.WriteLine("South: " + this.South.roomnum);
            }
            if (this.West != null)
            {
                Console.WriteLine("West: " + this.West.roomnum);
            }
            if (this.East != null)
            {
                Console.WriteLine("East: " + this.East.roomnum);
            }
        }
    }

}
