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
        private static Roommaker instance;

        private Roommaker() {}

        public static Roommaker getInstance()
        {
            if (instance == null)
            {
                instance = new Roommaker();
            }

            return instance;
        }

        public Room createMap()
        {
            //load the file
            XmlDocument doc = new XmlDocument();
            doc.Load("dungeon1.xml");

            //get the num of rooms and create an array of rooms
            XmlNode root = doc.FirstChild;
            int numrooms = root.ChildNodes.Count;

            //initialize array with room objects
            rooms = new Room[numrooms];
            for(int i = 0; i < numrooms; i++)
            {
                rooms[i] = new Room(i);
            }

            //get the first room
            XmlNode currNode = root.FirstChild;
            //start connecting rooms
            for(int i = 0; i < numrooms; i++)
            {

                //Read and set connections. Have to check that the value isn't null first
                if(currNode.ChildNodes[1].InnerText != "null")
                {
                    rooms[i].setNorth(rooms[Int32.Parse(currNode.ChildNodes[1].InnerText)]);
                }
                if (currNode.ChildNodes[2].InnerText != "null")
                {
                    rooms[i].setSouth(rooms[Int32.Parse(currNode.ChildNodes[2].InnerText)]);
                }
                if (currNode.ChildNodes[3].InnerText != "null")
                {
                    rooms[i].setWest(rooms[Int32.Parse(currNode.ChildNodes[3].InnerText)]);
                }
                if (currNode.ChildNodes[4].InnerText != "null")
                {
                    rooms[i].setEast(rooms[Int32.Parse(currNode.ChildNodes[4].InnerText)]);
                }

                //move to the next room
                currNode = currNode.NextSibling;

            }

            //for(int i = 0; i < numrooms; i++)
            //{
            //    rooms[i].print();
            //}

            return rooms[0];
        }
    }

}