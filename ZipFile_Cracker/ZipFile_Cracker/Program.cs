using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZipFile_Cracker
{
    class Program
    {
        static bool unZippedOK = false;


        static void Main(string[] args)
        {
            SevenZipBase.SetLibraryPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory, "7z.dll"));


            string unZippedFiles = Environment.CurrentDirectory + "\\Cracked Files"; //this is a folder that we will use to extract any cracked files



            Here: //a return point

            Console.WriteLine("Enter the Password List File Name :: example (rockyou.txt)");

            string passwordList = Console.ReadLine();

            string fileExtension = ""; //we will have to check if the file chosen is ok

            if(File.Exists(passwordList))
            {
                Console.WriteLine("Password list is found ok");

            }
            else
            {
                Console.WriteLine("Password missing");

                goto Here;
            }

            Console.WriteLine("Enter the Path to A protected Zip File");

            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                Console.WriteLine("Zip file found ok");

            }
            else
            {
                Console.WriteLine("file missing");

                goto Here;
            }

            fileExtension = Path.GetExtension(filePath);

            if(fileExtension == ".zip" | fileExtension == ".7z")
            {
                string pass = "";
                int counter = 0;
                bool closeLoop = false; //this will close the while loop if the file is cracked


                using (StreamReader file = new StreamReader(passwordList))
                {

                    while(closeLoop == false && (pass = file.ReadLine()) != null)
                    {

                        try
                        {
                            UnZip(filePath, unZippedFiles, pass);

                        }
                        catch (Exception)
                        {
                            //just carry on because if it is the wrong password it will throw an error
                           
                        }

                        if(unZippedOK == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(pass);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Cracked Password = " + pass);
                            Console.ResetColor();

                            file.Close(); //this will close the file stream but in a using statement it will do it anyway
                            closeLoop = true; //this will close the while loop

                            Console.ReadKey();
                           

                        }
                        else
                        {
                            Console.WriteLine(pass);
                        }
                        counter++;
                        Console.Title = "Current Password Count : " + counter.ToString();
                        Thread.Sleep(100); //decrease to go faster but be kind

                    }





                }



            }
            else
            {
                Console.WriteLine("The file you chosen was not a .zip or .7z file type");
                goto Here;
            }







        }

        private static void UnZip (string zippedFilePath , string outputFolder , string password = null)
        {

            SevenZipExtractor zipExtractor = null;

            if(!string.IsNullOrEmpty(password))
            {
                zipExtractor = new SevenZipExtractor(zippedFilePath, password);

                zipExtractor.Extracting += ZipExtractor_Extracting;

                zipExtractor.ExtractArchive(outputFolder); //we will export the cracked archive to our output folder
            }

        }

        private static void ZipExtractor_Extracting(object sender, ProgressEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Extracting files Ok !!!");
            Console.ResetColor();
            unZippedOK = true; //we will use this to stop the function
        }
    }
}
