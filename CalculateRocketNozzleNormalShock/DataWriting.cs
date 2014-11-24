using System;
using System.Collections.Generic;
using System.IO;

namespace CalculateRocketNozzleNormalShock
{
    class DataWriting
    {
        public bool WriteToFile(string fileNameAndPath, string[,] dataToWrite)
        {
            try
            {
                //Open the filestream
                FileStream fs = File.Open(Directory.GetCurrentDirectory() + "\\" + fileNameAndPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                int numColumns = dataToWrite.GetUpperBound(0);
                int numRows = dataToWrite.GetUpperBound(1);

                //Dump data to file
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numColumns; j++)
                    {
                        sw.Write(dataToWrite[j, i].ToString());
                        if (j == numColumns - 1)
                            sw.Write("\r\n");
                        else
                            sw.Write(", ");
                    }
                }

                //Cleanup
                sw.Close();
                sw = null;
                fs = null;
                System.Console.WriteLine("Data written to: " + Directory.GetCurrentDirectory() + "\\" + fileNameAndPath);
                return true;
            }
            catch (IOException e)
            {
                if (e.Message.Contains("Sharing violation"))
                    System.Console.WriteLine("Error writing to file: file already on use");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.StackTrace);
            }
            return false;
        }
    }
}
