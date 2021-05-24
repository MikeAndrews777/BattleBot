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
        public static int facing = 0, row = 0, column = 0;    // 0-North, 1-East, 2-South, 3-West
        public static List<int> facingMoveTrack = new List<int>();
        public static bool found = false, stillInTheList = false;

        //----------------------------------------------Maze Function---------------------------------------------------------------------
        //A left wheel
        //D right wheel

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
                facingMoveTrack.Add(facing);
            }

            if (facing == 1)
            {
                Console.WriteLine("East ---- 1");
                column++;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                facingMoveTrack.Add(facing);
            }

            if (facing == 2)
            {
                Console.WriteLine("South ---- 2");
                row--;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                facingMoveTrack.Add(facing);
            }

            if (facing == 3)
            {
                Console.WriteLine("West ---- 3");
                column--;
                Console.WriteLine("(Row, Column) : {0},{1}", row, column);
                Console.WriteLine();
                facingMoveTrack.Add(facing);
            }
        }

        static void right90()
        {
            Console.WriteLine("Turn Right 90");
            Console.WriteLine();

            //UK.MotorSync.On(-70, 80);

            UK.MotorA.On(-70, 940, false); //950 with Mike's Surface ----- between 920-930 with computer lab
            UK.MotorD.On(70, 940, false);
            facing++;
            facingCheck();
            System.Threading.Thread.Sleep(3000);
            //UK.MotorA.On(7, 7, false); //950 with Mike's Surface ----- between 7 with computer lab
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(500);
            UK.MotorA.On(-7, 2, false);
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(500);
        }

        static void left90()
        {
            Console.WriteLine("Turn Left 90");
            Console.WriteLine();

            //UK.MotorSync.On(-70, -80);

            UK.MotorA.On(70, 940, false); //950 with Mike's Surface ----- between 920-930 with computer lab
            UK.MotorD.On(-70, 940, false);
            facing--;
            facingCheck();
            System.Threading.Thread.Sleep(3000);
            UK.MotorA.On(-7, 2, false);//950 with Mike's Surface ----- between 7 with computer lab
            //UK.MotorD.On(7, 7, false);
            System.Threading.Thread.Sleep(500);
            UK.MotorA.On(-7, 2, false);
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(500);
        }



        static void forward()
        {
            Console.WriteLine("Move forward");
            Console.WriteLine();
          
            //UK.MotorSync.On(-70, 0);

            UK.MotorA.On(-70, 2350, false); //2320 with Mike's surface
            UK.MotorD.On(-70, 2350, false);

            updateMap();
            System.Threading.Thread.Sleep(7000);

        }

        static void turn180()
        {
            right90();
            right90();
            //Console.WriteLine("Turn around");
            //UK.MotorA.On(-70, 2*860, false); //950 with Mike's Surface ----- between 920-930 with computer lab
            //UK.MotorD.On(70, 2*860, false);
            //System.Threading.Thread.Sleep(7000);

        }


        static void Main(string[] args)
        {
            try
            {
                UK = new Brick<TouchSensor, Sensor, Sensor, Sensor>("COM52");

                UK.Connection.Close();
                UK.Connection.Open();

                UK.MotorA.ResetTacho();
                UK.MotorD.ResetTacho();
                UK.Sensor2 = new ColorSensor(ColorMode.Reflection);
                UK.Sensor3 = new IRSensor(IRMode.Proximity);
                UK.Sensor1.Mode = TouchMode.Count;
                UK.Sensor1.Reset();

                //Move from the start to the end --------------------------------------------------------------------------
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

                    Console.WriteLine("List");
                    for (int i = 0; i < facingMoveTrack.Count; i++)
                    {
                        Console.WriteLine("#{0} --> {1}", i, facingMoveTrack[i]);
                    }
                    Console.WriteLine();
                }

                do
                {
                    for (int i = 0; i < facingMoveTrack.Count-1; i++)
                    {
                        if (facingMoveTrack[i + 1] - facingMoveTrack[i] == 2 || facingMoveTrack[i + 1] - facingMoveTrack[i] == -2)
                        {
                            facingMoveTrack.RemoveAt(i);
                            facingMoveTrack.RemoveAt(i + 1);
                            found = true;
                            break;
                        }
                        else { found = false; }
                    }

                } while (found == true);

                Console.WriteLine("List after filtered");
                for (int i = 0; i < facingMoveTrack.Count; i++)
                {
                    Console.WriteLine("#{0} : {1}", i, facingMoveTrack[i]);
                }
                Console.WriteLine();
                //System.Threading.Thread.Sleep(60000);
                //----------------------------------------------------------------------------------------------

                System.Threading.Thread.Sleep(5000);

                //----------------------------------------------------------------------------------------------
                //Move back from the end to the start
                facingMoveTrack.Reverse(); //Reverse the list

                Console.WriteLine("Reverse List");
                for (int i = 0; i < facingMoveTrack.Count; i++)
                {
                    Console.WriteLine("#{0} : {1}", i, facingMoveTrack[i]);
                }
                Console.WriteLine();
                //System.Threading.Thread.Sleep(60000);

                System.Threading.Thread.Sleep(3000);

                Console.WriteLine("Go back!!");
                turn180();

                System.Threading.Thread.Sleep(3000);


                while (row != 1 || column != 0)
                {
                    if (facingMoveTrack.ElementAt(0) == 0)
                    {
                        for (int i = 0; i < facingMoveTrack.Count; i++)
                        {
                            if (facingMoveTrack.ElementAt(i) == 0)
                            {
                                forward();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 1)
                            {
                                right90();
                                forward();
                                left90();
                                
                            }
                            else if (facingMoveTrack.ElementAt(i) == 2)
                            {
                                turn180();
                                forward();
                                turn180();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 3)
                            {
                                left90();
                                forward();
                                right90();
                                
                            }
                            stillInTheList = true;
                        }
                        //continue;
                       
                    }

                    else if (facingMoveTrack.ElementAt(0) == 1)
                    {
                        for (int i = 0; i < facingMoveTrack.Count; i++)
                        {
                            if (facingMoveTrack.ElementAt(i) == 0)
                            {
                                left90();
                                forward();
                                right90();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 1)
                            {
                                forward();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 2)
                            {
                                right90();
                                forward();
                                left90();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 3)
                            {
                                turn180();
                                forward();
                                turn180();
                            }
                            stillInTheList = true;
                        }
                        //continue;
                    }

                    else if (facingMoveTrack.ElementAt(0) == 2)
                    {
                        for (int i = 0; i < facingMoveTrack.Count; i++)
                        {
                            if (facingMoveTrack.ElementAt(i) == 0)
                            {
                                turn180();
                                forward();
                                turn180();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 1)
                            {
                                left90();
                                forward();
                                right90();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 2)
                            {
                                forward();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 3)
                            {
                                right90();
                                forward();
                                left90();
                            }
                            stillInTheList = true;
                        }
                        //continue;
                    }

                    else if (facingMoveTrack.ElementAt(0) == 1)
                    {
                        for (int i = 0; i < facingMoveTrack.Count; i++)
                        {
                            if (facingMoveTrack.ElementAt(i) == 0)
                            {
                                right90();
                                forward();
                                left90();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 1)
                            {
                                forward();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 2)
                            {
                                right90();
                                forward();
                                left90();
                            }
                            else if (facingMoveTrack.ElementAt(i) == 3)
                            {
                                turn180();
                                forward();
                                turn180();
                            }
                            stillInTheList = true;
                        }
                        //continue;
                    }
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