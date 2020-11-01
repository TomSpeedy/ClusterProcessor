using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ClusterFilterConsole
{

    class CmdLineParser
    {
        private readonly string[] InputLine;
        public CmdLineParser(string[] inputLine) 
        {
            this.InputLine = inputLine;
        }
        private bool ContainsHelp()
        {
            return InputLine.Contains("--help");
        }

        private Dictionary<string, (double from, double to)> GetFilterParams()
        {
            var result = new Dictionary<string, (double from, double to)>();
            for (int i = 1; i < InputLine.Length; i+=3)
            {
                switch (InputLine[i])
                {
                    case "--pxCount":
                    case "-pc":
                        result.Add("-pc", (from : Convert.ToDouble(InputLine[i + 1]), to : Convert.ToDouble(InputLine[i + 2])));
                        break;
                    case "--totEnergy":
                    case "-te":
                        result.Add("-te", (from: Convert.ToDouble(InputLine[i + 1]), to: Convert.ToDouble(InputLine[i + 2])));
                        break;
                    case "--convexity":
                    case "-cv":
                        result.Add("-cv", (from: Convert.ToDouble(InputLine[i + 1]), to: Convert.ToDouble(InputLine[i + 2])));
                        break;
                }
            }
            return result;
        }
        public ProcessedCmdArgs ProcessCmdArgs() //inputFile = null means error has occured
        {
            try
            {
                if (ContainsHelp())
                {
                    return new ProcessedCmdArgs(inputFile : null, printHelp : true);
                }
                return new ProcessedCmdArgs(InputLine[0], printHelp: false)
                {
                    FilterParams = GetFilterParams()
                };
                
            }
            catch 
            {
                Console.WriteLine("Incorrect syntax, use option --help for displaying help");
                return new ProcessedCmdArgs(inputFile: null, printHelp: false);
            }

        }
        public struct ProcessedCmdArgs
        {
            public readonly string InputFile;
            public readonly bool PrintHelp;
            public Dictionary<string, (double from, double to)> FilterParams;
            public ProcessedCmdArgs(string inputFile, bool printHelp)
            {
                this.InputFile = inputFile;
                this.PrintHelp = printHelp;
                FilterParams = null; //FilterParams are initialized by CmdLineParser later on
            }
        }
    }

}

