using System;
using System.IO;
using System.Linq;
using Derp.Sales.Tests.Printing;
using Simple.Testing.Framework;

namespace Derp.Sales.Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var results = SimpleRunner
                .RunFromGenerator(new Generator(typeof (Program).Assembly))
                .ToList();

            Environment.ExitCode = results.Count(x => !x.Passed);

            var output = args != null && args.Length > 0
                ? new StreamWriter(File.OpenWrite(args[0]))
                : Console.Out;

            var formatter = output == Console.Out
                ? (IFormatSpecifications) new TextSpecificationFormatter(output)
                : new HtmlSpecificationFormatter(output);

            formatter.Format(new ResultsOfTestRun(results));

            output.Close();
        }
    }
}
