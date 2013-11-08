using System;
using System.Collections.Generic;
using Derp.Sales.Domain;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Specifications
{
    public class IsoWeekSpecifications
    {
        public IEnumerable<Specification> iso_week()
        {
            var rules = new Dictionary<string, string>
            {
                {"1/1/2005", "2004-W53"},
                {"1/2/2005", "2004-W53"},
                {"12/31/2005", "2005-W52"},
                {"1/1/2007", "2007-W01"},
                {"12/30/2007", "2007-W52"},
                {"12/31/2007", "2008-W01"},
                {"1/1/2008", "2008-W01"},
                {"12/28/2008", "2008-W52"},
                {"12/29/2008", "2009-W01"},
                {"12/30/2008", "2009-W01"},
                {"12/31/2008", "2009-W01"},
                {"1/1/2009", "2009-W01"},
                {"12/31/2009", "2009-W53"},
                {"1/1/2010", "2009-W53"},
                {"1/2/2010", "2009-W53"},
                {"1/3/2010", "2009-W53"},
            };
            var specifications = new List<Specification>();
            foreach (var rule in rules)
            {
                var date = DateTime.Parse(rule.Key);
                var theIsoWeek = rule.Value;
                var name = "ISO Week of " + date.ToLongDateString();
                specifications.Add(new QuerySpecification<DateTime, string>
                                      {
                                          Name = name,
                                          On = () => date,
                                          When = d => IsoWeek.FromDate(d),
                                          Expect =
                                          {
                                              result => theIsoWeek == result
                                          }
                                      });
            }
            return specifications;
        }

        public IEnumerable<Specification> invalid_input()
        {
            var invalid = new[] {"2009-W0", "2009-W54"};

            List<Specification> specifications = new List<Specification>();

            foreach (var isoWeek in invalid)
            {
                specifications.Add(new FailingSpecification<string, ArgumentOutOfRangeException>
                {
                    Name = "invalid input " + isoWeek,
                    On = () => isoWeek,
                    When = week => IsoWeek.FromString(week),
                    Expect =
                    {
                        ex => ex.ParamName == "week"
                    }
                });
            }
            return specifications;
        }

        public Specification comparisons()
        {
            return new ConstructorSpecification<IsoWeek>
            {
                When = () => IsoWeek.FromDate(DateTime.Parse("1/3/2010")),
                Expect =
                {
                    week => week.Equals(IsoWeek.FromDate(DateTime.Parse("1/2/2010"))),
                    week => week >= IsoWeek.FromDate(DateTime.Parse("1/2/2010")),
                    week => week <= IsoWeek.FromDate(DateTime.Parse("1/2/2010")),
                    week => week == IsoWeek.FromDate(DateTime.Parse("1/2/2010")),
                    week => week > IsoWeek.FromDate(DateTime.Parse("1/2/2009")),
                    week => week < IsoWeek.FromDate(DateTime.Parse("1/9/2010"))
                }
            };
        }
    }
}
