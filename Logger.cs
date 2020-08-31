// Logger
// Creates log files
// Inputs: name of program, message to write to file
// Output: creates and appends to NAME.log with timestamp
using System;
using System.IO;

namespace NBA_DataBase{

        class Logger{

            public Logger(string name, string message){
            
                //a using block makes sure an object is disposed when
                //it goes out of scope
                //this flushes and closes the stream
                string path = name + ".log";
                if(!File.Exists(path)){
                    using(StreamWriter sw = File.CreateText(path)){};
                }
                using (StreamWriter sw = File.AppendText(path)){
                    DateTime time = DateTime.Now;
                    sw.WriteLine(time.ToString("u") + ":");
                    sw.WriteLine(message + "\n");
                }
            }
        }
}