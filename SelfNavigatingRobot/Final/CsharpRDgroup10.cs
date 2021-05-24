using System;
using MonoBrick.EV3;
using System.Threading;
using System.IO;

namespace Application
{
    public static class QMAProgram
    {

        public static Brick<TouchSensor, Sensor, Sensor, Sensor> UK = null;
        public static UInt16 touchCount = 0, lightsensorval = 0;
        public static Double tachCount = 0;
        public static string tachoString, lightString, comma = ",";
        public static string CSVFilePath = @"C:\Users\Mike\Dropbox\SFU Schoolwork\MSE 110 (Mech Design)\UK Sensor Readings.csv"; //source path
        

       
        static void writeToCSV() //function for writing the data to csv file
        {

            lightString = Convert.ToString(lightsensorval); //convert the light sensor value to string, and assign it new variable called lightstring
            tachoString = Convert.ToString(tachCount); //convert the tacho count value to string, and assign it new variable tachstring

            
            string[][] output = new string[][]  //create a new instance of string array
            {
                new string[]{tachoString,lightString} // an array of two columns (Tacho count and Light Value)
            };

            System.Text.StringBuilder UKWrite = new System.Text.StringBuilder(); // creates the string UKWrite

            int length = output.GetLength(0);     // returns the length of the array named output
           
            for (int index = 0; index < length; index++)    // loop that fills out the CSV file
            {
                UKWrite.AppendLine(string.Join(comma, output[index])); // creates array string of numbers seperated by a comma (ex: 1000,60)
                File.AppendAllText(CSVFilePath, UKWrite.ToString()); // writes the array UKWrite into a CSV vile located at CSVFilePath
            }
        }



        static void Main(string[] args)
        {
            try
            {
                UK = new Brick<TouchSensor, Sensor, Sensor, Sensor>("COM3");

                UK.Connection.Close();
                UK.Connection.Open();

                
                UK.MotorB.ResetTacho();
               
                UK.Sensor2 = new ColorSensor(ColorMode.Reflection);    // sets color sensor to Color and Reflection (active) mode
                
                UK.Sensor1.Mode = TouchMode.Count;  // used to turn the robot on and off with a touch sensor
                UK.Sensor1.Reset();

                touchCount = Convert.ToUInt16(UK.Sensor1.Read());

                while (touchCount != 1)  //stop when the button pressed after the first time
                {
                    touchCount = Convert.ToUInt16(UK.Sensor1.Read());
                    Console.WriteLine("{0}", touchCount);
                    UK.MotorB.On(0);
                   
                }   // end of touch sensor robot control
              

                if (!File.Exists(CSVFilePath))  //checks if the csv file exists
                {
                    File.Create(CSVFilePath).Close();
                }

                UK.MotorB.On(-5); //move forward

                while (touchCount < 2) //stops when the button pressed twice
                {
                  
                    lightsensorval = Convert.ToUInt16(UK.Sensor2.ReadAsString()); // gets light sensor value, converts to unsigned 16 bit int
                   
                    tachCount = UK.MotorB.GetTachoCount() * (-1); //times -1 to get postive values
                    touchCount = Convert.ToUInt16(UK.Sensor1.Read()); //checks touch sensor to stop robot if pressed

                    
                    writeToCSV(); //call writetocsv function to read the data and store those into a csv file

                }


                UK.MotorB.Off(); // sets motorB off at the end of the program
              
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