using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funkis.Compiler;

namespace FunkisTests.Compiler
{
    [TestClass]
    public class NameAnalysisTests
    {
        private Tuple<ProgramNode, AnalysisScope> Analyze(string text)
        {
            var parser = new Parser(text);

            var ast = parser.Parse();

            var nameAnalyzer = new NameAnalyzer(ast);

            return nameAnalyzer.Analyze();
        }
        [TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.funk")]
        public void NameAnalyzeHelloWorldTest()
        {
            var text = TestFilesHelper.ReadHelloWorldTestFile();

            var tuple = Analyze(text);

            var ast = tuple.Item1;
            var analysisScope = tuple.Item2;
        }
    }
}
