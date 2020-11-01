using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace ClusterFilterConsole
{
    class Program //todo: check / or \ at specifying path
    {
        const string configPath = "../../../../config/";

        const string pixelCountOption = "-pc";
        const string totalEnergyOption = "-te";
        const string convexityOption = "-cv";
        static IClusterReader ClusterReader = new MMClusterReader();

       
        static void Main(string[] args)
        {
            //var fakeprocessedArgs = new CmdLineParser.ProcessedCmdArgs(inputPath, false);
            //fakeprocessedArgs.FilterParams = new Dictionary<string, (double from, double to)>();
            //fakeprocessedArgs.FilterParams.Add("-pc", (80, 1000));

            var cmdLineParser = new CmdLineParser(args);
            var processedArgs = cmdLineParser.ProcessCmdArgs();

            //check input
            string outIniPath = processedArgs.InputFile + "_fitered_" + DateTime.Now.ToString().Replace(':', '-' ) + ".ini";
            string outClPath = processedArgs.InputFile + "_fitered_" + DateTime.Now.ToString().Replace(':', '-') + ".cl";
            string outClName = outClPath.Substring(outClPath.LastIndexOf('/') + 1);
            Console.WriteLine(processedArgs.InputFile);
            var workingDirName = processedArgs.InputFile.Substring(0, processedArgs.InputFile.LastIndexOf('/') + 1);
            ClusterReader.GetTextFileNames(new StreamReader(processedArgs.InputFile), processedArgs.InputFile, out string pxFile, out string clFile);
            var filteredOut = new StreamWriter(outClPath);
            CreateNewIniFile(new StreamReader(processedArgs.InputFile), new StreamWriter(outIniPath), clFile, outClName);

            //Create filters
            var usedFilters = new List<ClusterFilter>();
            usedFilters.Add(new SuccessFilter());
            if (processedArgs.FilterParams.ContainsKey(pixelCountOption))
            {
                usedFilters.Add(new PixelCountFilter(new StreamReader(workingDirName + pxFile),
                Convert.ToInt32(processedArgs.FilterParams[pixelCountOption].from),
                Convert.ToInt32(processedArgs.FilterParams[pixelCountOption].to)));
            }
            if (processedArgs.FilterParams.ContainsKey(totalEnergyOption))
            {
                usedFilters.Add(new EnergyFilter(new StreamReader(workingDirName + pxFile), new StreamReader(configPath + "a.txt"),
                new StreamReader(configPath + "b.txt"), new StreamReader(configPath + "c.txt"), new StreamReader(configPath + "t.txt"),
                Convert.ToInt32(processedArgs.FilterParams[totalEnergyOption].from),
                Convert.ToInt32(processedArgs.FilterParams[totalEnergyOption].to)));
            }
            if (processedArgs.FilterParams.ContainsKey(convexityOption))
            {
                usedFilters.Add(new ConvexityFilter(new StreamReader(workingDirName + pxFile),
                Convert.ToInt32(processedArgs.FilterParams[convexityOption].from),
                Convert.ToInt32(processedArgs.FilterParams[convexityOption].to)));
            }
                var pixelCountFilter = processedArgs.FilterParams.ContainsKey(pixelCountOption) ?
                new PixelCountFilter(new StreamReader(workingDirName + pxFile),
                Convert.ToInt32(processedArgs.FilterParams[pixelCountOption].from),
                Convert.ToInt32(processedArgs.FilterParams[pixelCountOption].to))
                
                : new PixelCountFilter(new StreamReader(workingDirName + pxFile), 0, int.MaxValue);

            var energyFilter = processedArgs.FilterParams.ContainsKey(totalEnergyOption) ?
                new EnergyFilter(new StreamReader(workingDirName + pxFile), new StreamReader(configPath + "a.txt"),
                new StreamReader(configPath + "b.txt"), new StreamReader(configPath + "c.txt"), new StreamReader(configPath + "t.txt"),
                Convert.ToInt32(processedArgs.FilterParams[totalEnergyOption].from),
                Convert.ToInt32(processedArgs.FilterParams[totalEnergyOption].to))
                
                : new EnergyFilter(new StreamReader(workingDirName + pxFile), new StreamReader(configPath + "a.txt"),
                new StreamReader(configPath + "b.txt"), new StreamReader(configPath + "c.txt"), new StreamReader(configPath + "t.txt"),
                0 , double.MaxValue);

            var linearityFilter = processedArgs.FilterParams.ContainsKey(convexityOption) ?
                new ConvexityFilter(new StreamReader(workingDirName + pxFile),
                Convert.ToInt32(processedArgs.FilterParams[convexityOption].from),
                Convert.ToInt32(processedArgs.FilterParams[convexityOption].to))

                : new ConvexityFilter(new StreamReader(workingDirName + pxFile), 0, 100);


            var multiFilter = new MultiFilter(usedFilters);
            multiFilter.Process(new StreamReader(workingDirName + clFile), filteredOut);

            filteredOut.Close();
        }
        private static void CreateNewIniFile(StreamReader example, StreamWriter output, string oldClFileName, string newClFileName)
        {
            while (example.Peek() != -1)
            {
                output.WriteLine(example.ReadLine().Replace(oldClFileName,
                    newClFileName));
            }
            output.Close();
        }
    }
    
    
}
