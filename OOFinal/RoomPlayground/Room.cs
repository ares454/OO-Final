using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomPlayground
{
    public class Room
    {
        //Enemy[] enemies;
        Room North;
        Room South;
        Room West;
        Room East;

        public Room()
        {
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
    }

}
