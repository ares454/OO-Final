using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RoomPlayground;

namespace RoomPlayground
{

    public class Roommaker
    {
        private Room[] rooms;
        private Roommaker instance;

        private Roommaker() {}

        public static Roommaker getInstance()
        {
            if (instance == null)
            {
                instance = new Roommaker();
            }

            return instance;
        }

        public void createMap()
        {
            //load the file
            XmlDocument doc = new XmlDocument();
            doc.Load("dungeon1.xml");

            //get the num of rooms and create an array of rooms
            XmlNode root = doc.FirstChild;
            int numrooms = Int32.Parse(root.InnerXml);

            //initialize array with room objects
            rooms = new Room[numrooms];
            for(int i = 0; i < numrooms; i++)
            {
                rooms[i] = new Room();
            }

            //start connecting rooms
            for(int i = 0; i < numrooms; i++)
            {
                //move to the next room
                root = root.NextSibling();

                //Read and set connections
                rooms[i].setNorth(rooms[Int32.Parse(root.ChildNodes[0].InnerText)]);
            }

        }
    }

}