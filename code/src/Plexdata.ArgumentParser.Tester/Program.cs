/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;

namespace Plexdata.ArgumentParser.Tester
{
    class Program
    {
        /* SwitchParameterAttribute
        [ParametersGroup]
        class CmdLineArgs
        {
            [SwitchParameter(SolidLabel = "verbose", BriefLabel = "v")]
            public Boolean IsVerbose { get; set; }

            [SwitchParameter(SolidLabel = "debug")]
            public Boolean IsDebug { get; set; }
        }
        
        $> program.exe --verbose --debug
        $> program.exe -v --debug
        $> program.exe --verbose
        $> program.exe -v
        $> program.exe --debug
        */

        /* OptionParameterAttribute
        [ParametersGroup]
        class CmdLineArgs
        {
            [OptionParameter(SolidLabel = "password", DependencyList = "Username", DependencyType = DependencyType.Required)]
            public String Password { get; set; }

            [OptionParameter(SolidLabel = "username", DependencyList = "Password", DependencyType = DependencyType.Required)]
            public String Username { get; set; }
        }

        $> program.exe --username tobi --password !pa55w0rd?
        $> program.exe --password !pa55w0rd? --username tobi
        */

        /* VerbalParameterAttribute
        [ParametersGroup]
        class CmdLineArgs
        {
            [VerbalParameter]
            public String[] Files { get; set; }
        }

        $> program.exe file1.txt
        $> program.exe file1.txt file2.txt
        $> program.exe file1.txt file2.txt file3.txt
        */

        /* Main Method
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    CmdLineArgs cmdLineArgs = new CmdLineArgs();
                    cmdLineArgs.Process(args);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        */

        /* Help Configuration
        [HelpLicense]
        [HelpUtilize]
        [HelpPreface("This program does something useful.")]
        [ParametersGroup]
        class CmdLineArgs
        {
            [HelpSummary("More output during runtime.")]
            [SwitchParameter(SolidLabel = "verbose", BriefLabel = "v")]
            public Boolean IsVerbose { get; set; }

            [HelpSummary("Run program in Debug mode.")]
            [SwitchParameter(SolidLabel = "debug")]
            public Boolean IsDebug { get; set; }
        }
        */

        /* Main Method
        static void Main(string[] args)
        {
            try
            {
                CmdLineArgs cmdLineArgs = new CmdLineArgs();
                Console.WriteLine(cmdLineArgs.Generate());
                Console.ReadKey();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        */

        /* Help Output
        Copyright © <company>

        This program does something useful.

        Usage:

          <program> [options]

        Options:

          --verbose [-v]  More output during runtime.

          --debug         Run program in Debug mode.
        */

        static void Main(String[] args)
        {
            Console.WriteLine("Do something useful...");
            Console.ReadKey();
        }
    }
}

/* Custom type example
 * using Plexdata.ArgumentParser.Attributes;
 * using Plexdata.ArgumentParser.Constants;
 * using Plexdata.ArgumentParser.Extensions;
 * using Plexdata.ArgumentParser.Interfaces;
 * using System;
 * using System.Collections.Generic;
 * 
 * namespace Plexdata.ArgumentParser.Tester
 * {
 *     class Program
 *     {
 *         public class CustomType
 *         {
 *             public Int32 Top { get; set; }
 *             public Int32 Left { get; set; }
 *             public Int32 Height { get; set; }
 *             public Int32 Width { get; set; }
 *         }
 * 
 *         [ParametersGroup]
 *         public class CmdLineArgs
 *         {
 *             [OptionParameter(SolidLabel = "region", BriefLabel = "r")]
 *             public CustomType Region { get; set; }
 *         }
 * 
 *         public class CustomConverter : ICustomConverter<CustomType>
 *         {
 *             public CustomType Convert(String parameter, String argument, String delimiter)
 *             {
 *                 if (argument is null)
 *                 {
 *                     return null;
 *                 }
 * 
 *                 delimiter = delimiter ?? ArgumentDelimiters.DefaultDelimiter;
 * 
 *                 String[] splitted = argument.Split(delimiter.ToCharArray());
 * 
 *                 return new CustomType
 *                 {
 *                     Top = Int32.Parse(splitted[0]),
 *                     Left = Int32.Parse(splitted[1]),
 *                     Height = Int32.Parse(splitted[2]),
 *                     Width = Int32.Parse(splitted[3])
 *                 };
 *             }
 *         }
 * 
 *         static void Main(string[] args)
 *         {
 *             try
 *             {
 *                 // Adding of custom type converter.
 *                 CustomConverter converter = new CustomConverter();
 *                 converter.AddConverter();
 * 
 *                 List<String> options = new List<String>(Environment.CommandLine.Extract());
 * 
 *                 if (options != null && options.Count > 1)
 *                 {
 *                     // Removing of first line because it contains the executable name.
 *                     options.RemoveAt(0);
 * 
 *                     // Parsing of remaining command line arguments.
 *                     CmdLineArgs cmdLineArgs = new CmdLineArgs();
 *                     cmdLineArgs.Process(options);
 *                 }
 *             }
 *             catch (Exception exception)
 *             {
 *                 Console.WriteLine(exception);
 *             }
 *         }
 *     }
 * }
 *
 * $> program.exe --region 10:20:30:40
 *
 */

