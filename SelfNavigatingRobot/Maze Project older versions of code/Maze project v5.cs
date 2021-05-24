using System;
using MonoBrick.EV3;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class QMAProgram
    {

        public static Brick<TouchSensor, Sensor, Sensor, Sensor> UK = null;
        public static UInt16 touchCount = 0, lightsensorval = 0, ultrasonicsensorval = 0;
        public static Boolean Found = false;
        public static int facing = 0, row = 0, column = 0;    // 0-North, 1-East, 2-South, 3-West
        public static List<int> moveTrack = new List<int>();
        public static bool found = false;

        //----------------------------------------------Maze Function---------------------------------------------------------------------
        //A left
        //D right

        static void facingCheck()
        {
            if (facing == 4)
            {
                facing = 0;
            }
            else if (facing == -1)
            {
                facing = 3;
            }
        }

        static void updateMap()
        {

            if (facing == 0)
            {
                Console.WriteLine("North ---- 0 ");
                row++;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                moveTrack.Add(facing);
            }

            if (facing == 1)
            {
                Console.WriteLine("East ---- 1");
                column++;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                moveTrack.Add(facing);
            }

            if (facing == 2)
            {
                Console.WriteLine("South ---- 2");
                row--;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                moveTrack.Add(facing);
            }

            if (facing == 3)
            {
                Console.WriteLine("West ---- 3");
                column--;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                moveTrack.Add(facing);
            }
        }

        static void right90()
        {
            Console.WriteLine("Turn Right 90");
            Console.WriteLine();
            UK.MotorA.On(-70, 860, false); //950 with Mike's Surface ----- between 920-930 with computer lab
            UK.MotorD.On(70, 860, false);
            facing++;
            facingCheck();
            System.Threading.Thread.Sleep(3000);
            UK.MotorA.On(7, 7, false); //950 with Mike's Surface ----- between 7 with computer lab
            UK.MotorD.On(-7, 7, false);
            System.Threading.Thread.Sleep(500);
            UK.MotorA.On(-7, 7, false);
            UK.MotorD.On(-7, 7, false);
            System.Threading.Thread.Sleep(500);


        }

        static void left90()
        {
            Console.WriteLine("Turn Left 90");
            Console.WriteLine();
            UK.MotorA.On(70, 920, false); //950 with Mike's Surface ----- between 920-930 with computer lab
            UK.MotorD.On(-70, 920, false);
            facing--;
            facingCheck();
            System.Threading.Thread.Sleep(3000);
            UK.MotorA.On(-7, 7, false);//950 with Mike's Surface ----- between 7 with computer lab
            UK.MotorD.On(7, 7, false);
            System.Threading.Thread.Sleep(500);
            UK.MotorA.On(-7, 7, false);
            UK.MotorD.On(-7, 7, false);
            System.Threading.Thread.Sleep(500);


        }

        static void forward()
        {
            Console.WriteLine("Move forward");
            Console.WriteLine();
            UK.MotorA.On(-70, 2320, false);
            UK.MotorD.On(-70, 2320, false);
            updateMap();
            System.Threading.Thread.Sleep(7000);

        }

        static void uTurn()
        {
            Console.WriteLine("U turn");
            UK.MotorA.On(20, 360, false);
            UK.MotorD.On(-20, 360, false);
            System.Threading.Thread.Sleep(7000);

        }


        static void Main(string[] args)
        {
            try
            {
                UK = new Brick<TouchSensor, Sensor, Sensor, Sensor>("COM87");

                UK.Connection.Close();
                UK.Connection.Open();

                UK.MotorA.ResetTacho();
                UK.MotorD.ResetTacho();
                UK.Sensor2 = new ColorSensor(ColorMode.Reflection);
                UK.Sensor3 = new IRSensor(IRMode.Proximity);
                UK.Sensor1.Mode = TouchMode.Count;
                UK.Sensor1.Reset();

                while (row != 0 || column != 3)
                {

                    ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                    if (ultrasonicsensorval > 7)
                    {
                        right90();
                        Console.WriteLine("No Wall");
                        ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                        if (ultrasonicsensorval > 7)
                        {
                            Console.WriteLine("No Wall");
                            forward();
                            Console.WriteLine("--------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("Wall");
                            left90();
                            forward();
                            Console.WriteLine("--------------------------------------------------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wall");
                        right90();
                        ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                        if (ultrasonicsensorval > 7)
                        {
                            Console.WriteLine("No Wall");
                            forward();
                            Console.WriteLine("--------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("Wall");
                            left90();
                            left90();
                            ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                            if (ultrasonicsensorval > 7)
                            {
                                Console.WriteLine("No Wall");
                                forward();
                                Console.WriteLine("--------------------------------------------------------------");
                            }

                        }

                    }

                    for (int i = 0; i < moveTrack.Count; i++)
                    {
                        Console.WriteLine("List {0} : {1}", i, moveTrack[i]);
                    }
                }

                do{
                    for (int i = 0; i < moveTrack.Count; i++)
                    {
                        if(moveTrack[i+1]-moveTrack[i] == 2 || moveTrack[i+1]-moveTrack[i] == -2){
                            moveTrack.RemoveAt(i);
                            moveTrack.RemoveAt(i+1);
                            found = true;
                            continue;
                        }
                    }

                } while(found == false);

                for (int i = 0; i < moveTrack.Count; i++)
                {
                    Console.WriteLine("List after filtered {0} : {1}",i, moveTrack[i]);
                }

                System.Threading.Thread.Sleep(60000);
            }

            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine("Press any key to end...");
                Console.ReadKey();
            }
        }
    }
}