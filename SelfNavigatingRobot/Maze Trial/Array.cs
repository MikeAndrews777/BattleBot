using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testprogam
{
    class Program
    {
        const int MazeRow = 6;
        const int MazeColumn = 4;

        //public static void mazeblock()
        //{
        //    int[,] table = new int[,] {
        //        {1,2,3,4},
        //        {2,3,4,5},
        //        {3,4,5,6},
        //        {4,5,6,7},
        //        {5,6,7,8},
        //        {6,7,8,9}
        //    };

        //    //Console.WriteLine("{0}", table[0,0]);

        //    for (int x = 0; x < 6; x++)
        //    {
        //        Console.WriteLine("{0} {1} {2} {3}", table[x, 0], table[x, 1], table[x, 2], table[x, 3]);
        //    }
        //}

        static void Main(string[] args)
        {
            string[,] table = new string[MazeRow, MazeColumn] {
                {"A1","A2","A3","A4"},
                {"B1","B2","B3","B4"},
                {"C1","C2","C3","C4"},
                {"D1","D2","D3","D4"},
                {"E1","E2","E3","E4"},
                {"F1","F2","F3","F4"}
            };

            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (x == 4 && y == 2)
                    {
                        Console.WriteLine("{0}", table[x, y]);
                    }
                }
            }

            Console.ReadLine();

            

            /*
            for (int x = 0; x < 6; x++)
            {
                //Console.WriteLine("{0} {1} {2} {3}", table[x, 0], table[x, 1], table[x, 2], table[x, 3]);
                for (int y = 0; y < 4; y++)
                {
                    //Console.WriteLine("{0}", table[x, y]);
                }
            }

            
            string start = table[0, 1];
            string end = table[4, 3];
            Console.WriteLine("Start: {0} End: {1}", start, end);



            Console.WriteLine("Down path: ");
            //Going down
            int rowdown = 0;
            while (rowdown < 6)
            {
                int column = 0;
                string goingdown = table[rowdown, column];
                Console.WriteLine(goingdown);
                rowdown++;

            }


            Console.WriteLine();

            Console.WriteLine("Up path: ");
            //Going up
            int rowup = 5;
            while (rowup >= 0)
            {
                int column = 0;
                string goingup = table[rowup, column];
                Console.WriteLine(goingup);
                rowup--;

            }

            Console.WriteLine();

            Console.WriteLine("Right path: ");
            //Going up
            int columnright = 0;
            while (columnright < 4)
            {
                int row = 0;
                string goingright = table[row, columnright];
                Console.Write("{0} ", goingright);
                columnright++;

            }

            Console.WriteLine();

            Console.WriteLine("Left path: ");
            //Going up
            int columnleft = 3;
            while (columnleft >= 0)
            {
                int row = 0;
                string goingleft = table[row, columnleft];
                Console.Write("{0} ", goingleft);
                columnleft--;

            }
*/


            //for (int y = 0; y < 4; y++)
            //{
            //    
            //}

           
        }
    }
}


//Unsolved Problem:

//1. When going forward for a certain distance, it sets how many rotation on the wheels and it will show going from one block to another block
//2. If going to the right block, the direction is 1, left = 3, top = 0, down = 2
//3. going to the right = column++, left column--, top = row--, down = row++
//4. How to set so that when it moves, the computer will show and detect on the array where the robot goes

