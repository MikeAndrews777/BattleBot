using System;
using MonoBrick.EV3;
using System.Threading;

namespace Application
{
    public static class QMAProgram
    {

        public static Brick<TouchSensor, Sensor, Sensor, Sensor> UK = null;
        public static UInt16 touchCount = 0, lightsensorval = 0, ultrasonicsensorval = 0;
        public static Boolean Found = false;


        //----------------------------------------------Maze Function---------------------------------------------------------------------
        //A left
        //D right

        static void right90()
        {
            UK.MotorA.On(-50, 180, true);
            UK.MotorD.On(50, 180, true);

        }

        static void left90()
        {
            UK.MotorA.On(50, 180, true);
            UK.MotorD.On(-50, 180, true);
        }

        static void forward()
        {
            UK.MotorA.On(50, 3*360, true);
            UK.MotorD.On(50, 3*360, true);

        }

        static void uTurn()
        {
            UK.MotorA.On(50, 360, true);
            UK.MotorD.On(-50, 360, true);
        }
        static void direction()
        {
            int facing = 0;

            if (facing == 4)
            {
                facing = 0;
            }
            else if (facing == -1)
            {
                facing = 3;
            }

        }

        static void Main(string[] args)
        {
            try
            {
                UK = new Brick<TouchSensor, Sensor, Sensor, Sensor>("COM3");

                UK.Connection.Close();
                UK.Connection.Open();

                UK.MotorA.ResetTacho();
                UK.MotorD.ResetTacho();
                UK.Sensor2 = new ColorSensor(ColorMode.Reflection);
                UK.Sensor3 = new IRSensor(IRMode.Proximity);
                UK.Sensor1.Mode = TouchMode.Count;
                UK.Sensor1.Reset();

                while (true)
                {
                    right90();
                    ultrasonicsensorval = Convert.ToUInt16(UK.Sensor3.ReadAsString());
                    if (ultrasonicsensorval < 10)
                    {
                        left90();
                        forward();
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