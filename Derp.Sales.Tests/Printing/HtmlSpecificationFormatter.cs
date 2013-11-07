using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Simple.Testing.ClientFramework;
using Simple.Testing.Framework;

namespace Derp.Sales.Tests.Printing
{
    public class HtmlSpecificationFormatter : IFormatSpecifications
    {
        private readonly TextWriter output;

        public HtmlSpecificationFormatter(TextWriter output)
        {
            this.output = output;
        }

        public void Format(ResultsOfTestRun results)
        {
            output.WriteLine("<!DOCTYPE html>");
            output.WriteLine("<html>");
            output.WriteLine("<head><style>");
            var stylesheet = GetType().Assembly.GetManifestResourceStream(GetType(), "bootstrap.css");
            using (var reader = new StreamReader(stylesheet))
            {
                output.WriteLine(reader.ReadToEnd());
            }
            output.WriteLine("</style></head>");

            output.WriteLine("<body><section class='container'>");
            var resultsByCategory = results.All.GroupBy(GetSpecificationCategory);
            
            var categories = resultsByCategory.Select(x => x.Key).ToList();

            FormatTableOfContents(categories);

            foreach (var category in resultsByCategory)
            {
                FormatCategory(category);
            }
            output.WriteLine("</section></body>");
            output.WriteLine("</html>");
        }

        private void FormatCategory(IGrouping<string, RunResult> category)
        {
            output.WriteLine("<section id='{0}'>", category.Key.Underscore());
            output.WriteLine("<h1>{0}</h1>", category.Key);
            foreach (var result in category)
            {
                FormatRunResult(result);
            }
            output.WriteLine("</section>");
        }

        private string GetElementId(RunResult result)
        {
            return (result.FoundOnMemberInfo.DeclaringType ?? typeof(Specification)).FullName
                .Underscore() + "_" + result.Name.Underscore();
        }

        private string GetSpecificationCategory(RunResult result)
        {
            if (result.FoundOnMemberInfo.DeclaringType == null)
                return "Other";

            const string specifications = "Specifications";

            var pieces = result.FoundOnMemberInfo.DeclaringType.FullName.Split('.', '+')
                               .SkipWhile(piece => piece != specifications)
                               .Skip(1)
                               .Select(Inflector.Underscore)
                               .Select(Inflector.Titleize)
                               .Select(
                                   piece => piece.EndsWith(specifications)
                                       ? piece.Substring(0, piece.Length - specifications.Length)
                                       : piece);

            return String.Join("/", pieces);
        }

        private void FormatTableOfContents(IEnumerable<string> categories)
        {
            output.WriteLine("<nav><ul>");

            foreach (var category in categories)
            {
                output.WriteLine("<li><a href='#{0}'>{1}</a></li>", category.Underscore(), category);
            }

            output.WriteLine("</ul></nav>");
        }

        

        private void FormatRunResult(RunResult result)
        {
            output.WriteLine("<div class='alert alert-{0}'>", result.Passed ? "success" : "error");
            output.WriteLine("<details id='{0}'>", GetElementId(result));
            output.WriteLine("<summary>" + result.Name.Underscore().Titleize() + " - " + (result.Passed ? "Passed" : "Failed") + "</summary>");
            output.WriteLine("<pre>");
            FormatRunResultBody(result);
            output.WriteLine("</pre>");
            output.WriteLine("</details>");
            output.WriteLine("</div>");
        }

        private void FormatRunResultBody(RunResult result)
        {
            if (result.Thrown != null)
            {
                output.WriteLine("Specification threw an exception.");
                output.WriteLine(result.Thrown);
                output.WriteLine();
                return;
            }
            var @on = result.GetOnResult();
            if (@on != null)
            {
                output.WriteLine("On:");
                output.WriteLine(SpecificationPrinter.NicePrint(@on));
                output.WriteLine();
            }
            if (result.Result != null)
            {
                output.WriteLine("Results with:");
                if (result.Result is Exception)
                    output.WriteLine(result.Result.GetType() + "\n" + ((Exception)result.Result).Message);
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
        }
    }
}