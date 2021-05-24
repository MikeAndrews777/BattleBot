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

        public static Brick<TouchSensor, TouchSensor, Sensor, Sensor> UK = null;
        public static UInt16 lightsensorval = 0, ultrasonicsensorval = 0, alignCheck = 0;
        public static int touchCountR = 0, touchCountL = 0, facing = 0, row = 0, column = 0;    // 0-North, 1-East, 2-South, 3-West
        public static List<int> facingMoveTrack = new List<int>(); //List used for returning via fastest route
        public static bool found = false, stillInTheList = false;
        //==========================================================================================================
        public static int rowStart, colStart, rowEnd, colEnd, facingInput;
        public static string nwall = "North Wall", ewall = "East Wall", swall = "South Wall", wwall = "West Wall";
        public static List<string> wallTrack = new List<string>(); //List for keeping the wall information
        public static List<int> rowTrack = new List<int>(); //List for keeping the row information
        public static List<int> colTrack = new List<int>(); //List for keeping the column information
        public static List<int> facingTrack = new List<int>(); //List for keeping the facing information
        //=============================================================================================================

        //----------------------------------------------Maze Function---------------------------------------------------------------------
        //A left wheel
        //D right wheel
        //===================================================================================================
        static void wallInformation()
        {
            if (facing == 0)
            {
                Console.WriteLine("Facing North");
                Console.WriteLine();

            }

            if (facing == 1)
            {

                Console.WriteLine("Facing East");
                Console.WriteLine();
            }

            if (facing == 2)
            {

                Console.WriteLine("Facing South");
                Console.WriteLine();
            }

            if (facing == 3)
            {
                Console.WriteLine("Facing West");
                Console.WriteLine();
            }
        }

        static void wallCheck()
        {
            ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
            if (facing == 0 && ultrasonicsensorval > 7)
            {
                Console.WriteLine("No {0}", nwall);
                Console.WriteLine();
                wallTrack.Add("No North Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
            else if (facing == 0 && ultrasonicsensorval < 7)
            {
                Console.WriteLine(nwall);
                Console.WriteLine();
                wallTrack.Add("North Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
            else if (facing == 1 && ultrasonicsensorval > 7)
            {
                Console.WriteLine("No {0}", ewall);
                Console.WriteLine();
                wallTrack.Add("No East Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
            else if (facing == 1 && ultrasonicsensorval < 7)
            {
                Console.WriteLine(ewall);
                Console.WriteLine();
                wallTrack.Add("East Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
            else if (facing == 2 && ultrasonicsensorval > 7)
            {
                Console.WriteLine("No {0}", swall);
                Console.WriteLine();
                wallTrack.Add("No South Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
            else if (facing == 2 && ultrasonicsensorval < 7)
            {
                Console.WriteLine(swall);
                Console.WriteLine();
                wallTrack.Add("South Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
            else if (facing == 3 && ultrasonicsensorval > 7)
            {
                Console.WriteLine("No {0}", wwall);
                Console.WriteLine();
                wallTrack.Add("No West Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
            else if (facing == 3 && ultrasonicsensorval < 7)
            {
                Console.WriteLine(wwall);
                Console.WriteLine();
                wallTrack.Add("West Wall");
                rowTrack.Add(row);
                colTrack.Add(column);
                facingTrack.Add(facing);
            }
        }
        //=======================================================================================
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

        static void straightenOut()
        {
            ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());

            if (alignCheck > 4 && ultrasonicsensorval < 7)
            {
                touchCountR = 0;
                touchCountL = 0;
                UK.Sensor1.Reset();
                UK.Sensor2.Reset();
                Console.WriteLine("Alignment: (Touch Sensor 1 = {0}, Touch Sensor 2 = {1}, UltraSensor = {2})", touchCountR, touchCountL,ultrasonicsensorval);
                System.Threading.Thread.Sleep(100);

                while (touchCountL == 0 || touchCountR == 0)
                {
                    touchCountL = UK.Sensor1.Read();
                    touchCountR = UK.Sensor2.Read();
                    System.Threading.Thread.Sleep(100);
                    if (touchCountL == 0)
                    { UK.MotorA.On(-70, false); }
                    else { UK.MotorA.Off(); }

                    if (touchCountR == 0)
                    { UK.MotorD.On(-70, false); }
                    else { UK.MotorD.Off(); }

                }

                alignCheck = 0;
                touchCountR = 0;
                touchCountL = 0;
                UK.Sensor1.Reset();
                UK.Sensor2.Reset();

                UK.MotorA.On(7, 2, false); //Re-alignment 1
                UK.MotorD.On(7, 2, false);
                System.Threading.Thread.Sleep(400);
                UK.MotorA.On(7, 2, false); //Re-alignment 2
                UK.MotorD.On(7, 2, false);
                System.Threading.Thread.Sleep(400);

                UK.MotorA.On(70, 570, false); //Backup half a cell
                UK.MotorD.On(70, 570, false);
                System.Threading.Thread.Sleep(2500);

                UK.MotorA.On(-7, 2, false); //Re-alignment 3
                UK.MotorD.On(-7, 2, false);
                System.Threading.Thread.Sleep(400);
                UK.MotorA.On(-7, 2, false); //Re-alignment 4
                UK.MotorD.On(-7, 2, false);
                System.Threading.Thread.Sleep(400);

            }



        }

        static void right90()
        {
            Console.WriteLine("Turn Right 90");
            Console.WriteLine();

            //UK.MotorSync.On(-70, 80);

            UK.MotorA.On(-70, 950, false); //950 with Mike's Surface ----- between 920-930 with computer lab
            UK.MotorD.On(70, 950, false);
            facing++;
            facingCheck();
            wallInformation();//======================================================
            System.Threading.Thread.Sleep(3000);
            //UK.MotorA.On(7, 7, false); //950 with Mike's Surface ----- between 7 with computer lab
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(400);
            UK.MotorA.On(-7, 2, false);
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(400);
            UK.MotorA.On(-7, 2, false);
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(400);
            alignCheck++;
            straightenOut();
        }

        static void left90()
        {
            Console.WriteLine("Turn Left 90");
            Console.WriteLine();

            //UK.MotorSync.On(-70, -80);

            UK.MotorA.On(70, 950, false); //950 with Mike's Surface ----- between 920-930 with computer lab
            UK.MotorD.On(-70, 950, false);
            facing--;
            facingCheck();
            wallInformation();//============================================================
            System.Threading.Thread.Sleep(3000);
            UK.MotorA.On(-7, 2, false);//950 with Mike's Surface ----- between 7 with computer lab
            //UK.MotorD.On(7, 7, false);
            System.Threading.Thread.Sleep(400);
            UK.MotorA.On(-7, 2, false);
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(400);
            UK.MotorA.On(-7, 2, false);
            UK.MotorD.On(-7, 2, false);
            System.Threading.Thread.Sleep(400);
            alignCheck++;
            straightenOut();
        }


        static void forward()
        {
            Console.WriteLine("Move forward");
            Console.WriteLine();

            //UK.MotorSync.StepSync(-70, 0, 2320, false);

            UK.MotorA.On(-70, 2320, false); //2320 with Mike's surface
            UK.MotorD.On(-70, 2320, false);

            updateMap();
            wallInformation(); //==============================================================
            System.Threading.Thread.Sleep(5000);
            alignCheck++;
            System.Threading.Thread.Sleep(400);
            straightenOut();


        }

        static void turn180()
        {
            right90();
            right90();
           
        }


        static void Main(string[] args)
        {
            try
            {
                UK = new Brick<TouchSensor, TouchSensor, Sensor, Sensor>("COM3");

                UK.Connection.Close();
                UK.Connection.Open();

                UK.MotorA.ResetTacho();
                UK.MotorD.ResetTacho();
                //UK.Sensor2 = new ColorSensor(ColorMode.Reflection);
                UK.Sensor3 = new IRSensor(IRMode.Proximity);
                UK.Sensor1.Mode = TouchMode.Count;
                UK.Sensor2.Mode = TouchMode.Count;
                
                UK.Sensor1.Reset();
                UK.Sensor2.Reset();
                


                //====================================================
                Console.WriteLine("Input Facing: ");
                string inputFacing = Console.ReadLine();
                Console.WriteLine("Input Row Start: ");
                string inputRowStart = Console.ReadLine();
                Console.WriteLine("Input Column Start: ");
                string inputColStart = Console.ReadLine();
                Console.WriteLine("Input Row End: ");
                string inputRowEnd = Console.ReadLine();
                Console.WriteLine("Input Column End: ");
                string inputColEnd = Console.ReadLine();
                Console.WriteLine();


                facingInput = Convert.ToInt16(inputFacing);
                rowStart = Convert.ToInt16(inputRowStart);
                colStart = Convert.ToInt16(inputColStart);
                rowEnd = Convert.ToInt16(inputRowEnd);
                colEnd = Convert.ToInt16(inputColEnd);
                row = rowStart;
                column = colStart;
                facing = facingInput;
                //=====================================================


                //Move from the start to the Cheese ----------------------------------------------------------------------
                while (row != rowEnd || column != colEnd)
                {

                    ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                    if (ultrasonicsensorval > 7)
                    {

                        right90();
                        wallCheck();//======================================
                        ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                        if (ultrasonicsensorval > 7)
                        {
                            //Console.WriteLine("No Wall");
                            wallCheck();//======================================
                            forward();
                            Console.WriteLine("--------------------------------------------------------------");
                        }
                        else
                        {
                            //Console.WriteLine("Wall");
                            wallCheck();//======================================
                            left90();
                            wallCheck();//======================================
                            forward();
                            Console.WriteLine("--------------------------------------------------------------");
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Wall");
                        right90();
                        wallCheck();//======================================
                        ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                        if (ultrasonicsensorval > 7)
                        {
                            //Console.WriteLine("No Wall");
                            wallCheck();//======================================
                            forward();
                            System.Threading.Thread.Sleep(400);
                            Console.WriteLine("--------------------------------------------------------------");
                        }
                        else
                        {
                            //Console.WriteLine("Wall");

                            left90();
                            wallCheck();//======================================
                            left90();
                            wallCheck();//======================================
                            ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                            if (ultrasonicsensorval > 7)
                            {
                                //Console.WriteLine("No Wall");
                                wallCheck();//======================================
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
                }  //end of first while loop for finding cheese
                //======================================================================
                Console.WriteLine("Wall Information List");
                for (int i = 0; i < wallTrack.Count; i++)
                {
                    Console.WriteLine(wallTrack[i]);
                }
                Console.WriteLine();
                //======================================================================
                UK.Beep(100, 2000);  //beeps when cheese is found
                Console.WriteLine();
                Console.WriteLine("The cheese is found!!!!");
                Console.WriteLine();
                System.Threading.Thread.Sleep(2500);

                do       // do-while loop that filters out redundant moves
                {
                    for (int i = 0; i < facingMoveTrack.Count - 1; i++) // filters out redundant moves
                    {
                        if (facingMoveTrack[i] - facingMoveTrack[i + 1] == 2 || facingMoveTrack[i] - facingMoveTrack[i + 1] == -2)
                        {

                            facingMoveTrack.RemoveAt(i);
                            facingMoveTrack.RemoveAt(i++);
                            found = true;
                            break;
                        }
                        else { found = false; }
                    }

                } while (found == true);   // end of filtering do-while loop

                Console.WriteLine("List after filtered");
                for (int i = 0; i < facingMoveTrack.Count; i++)
                {
                    Console.WriteLine("#{0} : {1}", i, facingMoveTrack[i]);
                }
                Console.WriteLine();
                //System.Threading.Thread.Sleep(60000);
                //----------------------------------------------------------------------------------------------



                //----------------------------------------------------------------------------------------------
                //Move back from the end to the start
                facingMoveTrack.Reverse(); //Reverse the list

                Console.WriteLine("Reverse List");
                for (int i = 0; i < facingMoveTrack.Count; i++)
                {
                    Console.WriteLine("#{0} : {1}", i, facingMoveTrack[i]);
                }
                Console.WriteLine();


                Console.WriteLine("Go back!!");
                turn180();

                System.Threading.Thread.Sleep(2000);

                while (row != rowStart || column != colStart)
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
                            if (row == rowStart && column == colStart) { break; }
                        }

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
                            if (row == rowStart && column == colStart) { break; }
                        }

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
                            if (row == rowStart && column == colStart) { break; }
                        }

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
                            if (row == rowStart && column == colStart) { break; }
                        }

                    }
                }  //end of final while loop for returning home
                Console.WriteLine();
                Console.WriteLine("Back Home with the Cheese!!!! Happy UK mouse!");
                Console.ReadKey();

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