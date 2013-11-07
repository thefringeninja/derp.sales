using System.Collections.Generic;
using System.Linq;
using Simple.Testing.Framework;

namespace Derp.Sales.Tests
{
    public class ResultsOfTestRun
    {
        private readonly IEnumerable<RunResult> runResults;

        public IEnumerable<RunResult> Failed
        {
            get { return runResults.Where(SpecificationFailed); }
        }

        public IEnumerable<RunResult> Passed
        {
            get { return runResults.Where(SpecificationPassed); }
        }

        public IEnumerable<RunResult> All
        {
            get { return runResults; }
        } 

        public ResultsOfTestRun(IEnumerable<RunResult> runResults)
        {
            this.runResults = runResults.ToList();
        }

        private static bool SpecificationPassed(RunResult result)
        {
            return false == SpecificationFailed(result);
        }

        private static bool SpecificationFailed(RunResult result)
        {
            return false == result.Passed;
        }

    }
}