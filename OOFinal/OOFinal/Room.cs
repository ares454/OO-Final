﻿using System;
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

        /// <summary>
        /// Testing
        /// </summary>
        /// <param name="num"></param>
        public void FillEnemyList()
        {

            int level = Player.GetInstance().Level;
            int numClass = Class.Type.GetNames(typeof(Class.Type)).Length;
            int numRace = Race.Type.GetNames(typeof(Race.Type)).Length;
            Class c;

            for(int i = 0; i < level; ++i)
            {
                int val = Player.Roll(numClass* 10) % numClass + 1;
                int lvl = Player.Roll(level + 2, level - 2 < 1 ? 1 : level - 2);
                c = Class.CreateEnemy((Class.Type)val, lvl);
                val = Player.Roll(numRace);
                enemies.Add(Race.CreateEnemy((Race.Type)val, c));
            }
        }

        public void RemoveDead(Race e)
        {
            enemies.Remove(e);
            if (enemies.Count == 0)
                FillEnemyList();
        }

        public Enemy GetEnemy(string name, int count = 1)
        {
            if (count < 1)
                return null;

            foreach(Enemy enemy in enemies)
            {
                if (enemy.Name.Contains(name) && --count == 0)
                    return enemy;
            }

            return null;
        }

        public Room(int num)
        {
            roomnum = num;
            North = null;
            South = null;
            West = null;
            East = null;
            enemies = new HashSet<Enemy>();
            FillEnemyList();
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

        public bool hasNorth()
        {
            return North != null;
        }

        public bool hasSouth()
        {
            return South != null;
        }

        public bool hasWest()
        {
            return West != null;
        }

        public bool hasEast()
        {
            return East != null;
        }

        public Room getNorth()
        {
            return North;
        }

        public Room getSouth()
        {
            return South;
        }

        public Room getWest()
        {
            return West;
        }

        public Room getEast()
        {
            return East;
        }
    }

}
