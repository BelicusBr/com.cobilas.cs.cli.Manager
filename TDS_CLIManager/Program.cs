using System;
using System.Linq;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace TDS_CLIManager
{
    class Program
    {

        static readonly CLICommand root = new(
                "root",
                new CLICommand(15,
                        "init/-i",
                        CLICMDArg.Default
                    ),
                new CLICommand(32,
                        "conf/-c",
                        new CLIOption(1, "--access/-a"),
                        new CLIOption(1, "--moscoi/-m"),
                        CLICMDArg.Default
                    )
            );

        static void Main(string[] args)
        {
            CLIArgCollection cLIArgs = new CLIArgCollection();
            ErrorMensager error = new ErrorMensager();

            CLICommand.Cateter(new StringArrayToIEnumerator(new string[] {
                "-i", "loppo"
            }), root, cLIArgs, error, out int funcID);

            Console.WriteLine("FuncID:{0}", funcID);

            foreach (var item in cLIArgs)
                Console.WriteLine(item);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (var item in error)
                Console.WriteLine(item);
            Console.ResetColor();

            _ = Console.ReadLine();
        }
    }
}
