using System;
using System.IO;
using System.Linq;
using Simple.Testing.Framework;

namespace Derp.Sales.Tests.Printing
{
    public class TextSpecificationFormatter : IFormatSpecifications
    {
        private readonly TextWriter output;

        public TextSpecificationFormatter(TextWriter output)
        {
            this.output = output;
        }

        public void Format(ResultsOfTestRun results)
        {
            foreach (var result in results.All)
            {
                Print(result);
            }

            output.WriteLine(
                "Passed: {0}; Failed: {1}; Total: {2}", results.Passed.Count(), results.Failed.Count(),
                results.All.Count());
        }

        private void Print(RunResult result)
        {
            var passed = result.Passed ? "Passed" : "Failed";
            output.WriteLine(result.Name.Underscore().Titleize() + " - " + passed);
            if (result.Thrown != null)
            {
                output.WriteLine();
                output.WriteLine("Specification threw an exception.");
                output.WriteLine(result.Thrown);
                output.WriteLine();
                return;
            }
            var @on = result.GetOnResult();
            if (@on != null)
            {
                output.WriteLine();
                output.WriteLine("On:");
                output.WriteLine(SpecificationPrinter.NicePrint(@on));
                output.WriteLine();
            }
            if (result.Result != null)
            {
                output.WriteLine();
                output.WriteLine("Results with:");
                if (result.Result is Exception)
                    output.WriteLine(result.Result.GetType() + "\n" + ((Exception) result.Result).Message);
                else
                    output.WriteLine(SpecificationPrinter.NicePrint(result.Result));
                output.WriteLine();
            }

            output.WriteLine("Expectations:");
            foreach (var expecation in result.Expectations)
            {
                if (expecation.Passed)
                    output.WriteLine("\t" + expecation.Text + " " + (expecation.Passed ? "Passed" : "Failed"));
                else
                    output.WriteLine(expecation.Exception.Message);
            }
            if (result.Thrown != null)
            {
                output.WriteLine("Specification failed: " + result.Message);
                output.WriteLine();
                output.WriteLine(result.Thrown);
            }
            output.WriteLine(new string('-', 80));
            output.WriteLine();

            output.Flush();
        }
    }
}