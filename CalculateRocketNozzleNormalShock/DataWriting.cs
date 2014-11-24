/*
Copyright (c) 2014, Michael Ferrara
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace CalculateRocketNozzleNormalShock
{
    class DataWriting
    {
        public bool WriteToFile(string fileNameAndPath, string[,] dataToWrite, string header)
        {
            try
            {
                //Open the filestream
                FileStream fs = File.Open(Directory.GetCurrentDirectory() + "\\" + fileNameAndPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                int numColumns = dataToWrite.GetUpperBound(0) + 1;
                int numRows = dataToWrite.GetUpperBound(1) + 1;

                sw.Write("Model used: " + header + "\r\n");
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
