namespace BL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using DTO;

    public class MsTestResultParser
    {
        private static readonly XNamespace ns = @"http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        private readonly XDocument resultsDocument;

        public MsTestResultParser(XDocument resultsDocument)
        {
            this.resultsDocument = resultsDocument;
        }

        public TestInfo GetScenarioResult(Scenario scenario)
        {
            Guid scenarioExecutionId = this.GetScenarioExecutionId(scenario);
            return this.GetExecutionResult(scenarioExecutionId);
        }

        private Guid GetScenarioExecutionId(Scenario queriedScenario)
        {
            string idString = null;
            foreach (XElement element in AllUnitTests())
            {
                if (element.Attribute("name").Value.ToLower() == FormatScenarioName(queriedScenario.Nom).ToLower())
                {
                    idString = element.Attribute("id").Value;
                }
            }

            return !string.IsNullOrEmpty(idString) ? new Guid(idString) : Guid.Empty;
        }

        private TestInfo GetExecutionResult(Guid scenarioExecutionId)
        {
            TestInfo testInfo = new TestInfo() { Id = Guid.NewGuid() };
            XElement elScenario = AllUnitTestResults()
                                    .FirstOrDefault(a => ResultExecutionIdOf(a) == scenarioExecutionId);

            if (elScenario == null)
            {
                return null;
            }

            string resultText = elScenario.Attribute("outcome").Value;
            switch (resultText.ToLowerInvariant())
            {
                case "passed":
                    testInfo.Result = TestResult.Passed.ToString();
                    break;
                case "failed":
                    testInfo.Result = TestResult.Failed.ToString();
                    break;
                default:
                    testInfo.Result = TestResult.Inconclusive.ToString();
                    break;
            }

            testInfo.DateStart = DateTime.Parse(elScenario.Attribute("startTime").Value);
            testInfo.DateEnd = DateTime.Parse(elScenario.Attribute("endTime").Value);
            testInfo.Duree = TimeSpan.Parse(elScenario.Attribute("duration").Value);

            XElement elOutput = elScenario.Element(ns+"Output");
            if (elOutput != null)
            {
                XElement stdOut = elOutput.Element(ns + "StdOut");
                if (stdOut != null)
                {
                    testInfo.ConsoleOutput = stdOut.Value.Trim();
                }

                XElement errorInfo = elOutput.Element(ns + "ErrorInfo");
                if (errorInfo != null)
                {
                    testInfo.ErrorMessage = errorInfo.Element(ns + "Message").Value.Trim();
                    testInfo.ErrorStacktrace = errorInfo.Element(ns + "StackTrace").Value.Trim();
                }
            }

            return testInfo;
        }

        private static string ResultOutcomeOf(XElement scenarioResult)
        {
            var outcomeAttribute = scenarioResult.Attribute("outcome");
            return outcomeAttribute != null ? outcomeAttribute.Value : "failed";
        }

        private static Guid ResultExecutionIdOf(XElement unitTestResult)
        {
            var executionIdAttribute = unitTestResult.Attribute("testId");
            return executionIdAttribute != null ? new Guid(executionIdAttribute.Value) : Guid.Empty;
        }

        private static Guid ScenarioExecutionIdOf(XElement scenario)
        {
            return new Guid(ScenarioExecutionIdStringOf(scenario));
        }

        private static string ScenarioExecutionIdStringOf(XElement scenario)
        {
            return scenario.Element(ns + "Execution").Attribute("id").Value;
        }

        private static string NameOf(XElement scenario)
        {
            return scenario.Element(ns + "Description").Value;
        }

        private static XElement PropertiesOf(XElement scenariosReportes)
        {
            return scenariosReportes.Element(ns + "Properties");
        }

        private static bool FeatureNamePropertyExistsWith(string featureName, XElement among)
        {
            var properties = among;
            return (from property in properties.Elements(ns + "Property")
                    let key = property.Element(ns + "Key")
                    let value = property.Element(ns + "Value")
                    where key.Value == "FeatureTitle" && value.Value == featureName
                    select property).Any();
        }

        private IEnumerable<XElement> AllUnitTests()
        {
            return this.resultsDocument.Root.Descendants(ns + "UnitTest");
        }

        private IEnumerable<XElement> AllUnitTestResults()
        {
            return this.resultsDocument.Root.Descendants(ns + "UnitTestResult");
        }

        public static TestResult Merge(IEnumerable<TestInfo> testResults)
        {
            if (testResults == null)
            {
                throw new ArgumentNullException("testResults");
            }

            TestResult[] items = testResults.Where(t => t != null && !string.IsNullOrEmpty(t.Result)).Select(t => (TestResult)Enum.Parse(typeof(TestResult), t.Result, true)).ToArray();

            if (!items.Any())
            {
                return TestResult.NoTest;
            }

            if (items.Count() == 1)
            {
                return items.Single();
            }

            if (items.Any(i => i == TestResult.Failed))
            {
                return TestResult.Failed;
            }

            if (items.Any(i => i == TestResult.Inconclusive))
            {
                return TestResult.Inconclusive;
            }

            return TestResult.Passed;
        }

        public static string FormatScenarioName(string original)
        {
            return original
                        .Replace(" ", "")
                        .Replace("\"", "")
                        .Replace("é", "e")
                        .Replace("è", "e")
                        .Replace("ê", "e")
                        .Replace("à", "a")
                        .Replace("ô", "o")
                        .Replace("î", "i")
                        .Replace("ù", "u")
                        .Replace("ñ", "n")
                        .Replace("'", "")
                        .Replace("-", "_")
                        .Replace("<", "")
                        .Replace(">", "")
                        .Replace("=", "");
        }
    }
}