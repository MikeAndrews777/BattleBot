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
        public static int facing = 2, row = 1, column = 3;    // 0-North, 1-East, 2-South, 3-West


        //----------------------------------------------Maze Function---------------------------------------------------------------------
        //A left
        //D right

        static List<int> moveHistory(int move)
        {
            List<int> moveTrack = new List<int>();

            moveTrack.Add(move);
            return moveTrack;
        }
        

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
            List<int> robotMove = moveHistory(facing);
            
            if (facing == 0)
            {
                Console.WriteLine("North ---- 0 ");
                row++;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                robotMove.Add(facing);
            }

            if (facing == 1)
            {
                Console.WriteLine("East ---- 1");
                column++;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                robotMove.Add(facing);
            }

            if (facing == 2)
            {
                Console.WriteLine("South ---- 2");
                row--;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                robotMove.Add(facing);
            }

            if (facing == 3)
            {
                Console.WriteLine("West ---- 3");
                column--;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                robotMove.Add(facing);
            }

            for (int i = 0; i < robotMove.Count; i++)
            {
                Console.WriteLine("Move History: {0} (List #{1})", robotMove[i],i);
            }   
        }

        static void right90()
        {
            Console.WriteLine("Turn Right 90");
            Console.WriteLine();
            UK.MotorA.On(-70, 950, false);
            UK.MotorD.On(70, 950, false);
            facing++;
            facingCheck();
            System.Threading.Thread.Sleep(3000);
            UK.MotorA.On(7, 2, false);
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(500);
            UK.MotorA.On(-7, 7, false);
            UK.MotorD.On(-7, 7, false);
            System.Threading.Thread.Sleep(500);
           

        }

        static void left90()
        {
            Console.WriteLine("Turn Left 90");
            Console.WriteLine();
            UK.MotorA.On(70, 950, false);
            UK.MotorD.On(-70, 950, false);
            facing--;
            facingCheck();
            System.Threading.Thread.Sleep(3000);
            UK.MotorA.On(-7, 2, false);
            UK.MotorD.On(7, 2, false);
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
                UK = new Brick<TouchSensor, Sensor, Sensor, Sensor>("COM10");

                UK.Connection.Close();
                UK.Connection.Open();

                UK.MotorA.ResetTacho();
                UK.MotorD.ResetTacho();
                UK.Sensor2 = new ColorSensor(ColorMode.Reflection);
                UK.Sensor3 = new IRSensor(IRMode.Proximity);
                UK.Sensor1.Mode = TouchMode.Count;
                UK.Sensor1.Reset();

                List<int> history = moveHistory(facing);
                

                while (row != 2 || column != 3)
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

                    //foreach (int item in history)
                    //{
                    //    Console.WriteLine("Move: {0}", moveHistory(facing));
                    //}
                }
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