using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Mono.CSharp;

namespace scriptLoader
{
    public class ScriptEngine
    {
        private Evaluator _evaluator;
        private StringBuilder _stringBuilder;
        private string _error = string.Empty;

        public ScriptEngine(DirectoryInfo projectDirectoryInfo)
        {
            _stringBuilder = new StringBuilder();
            var settings = new CompilerSettings();
            settings.LoadDefaultReferences = true;
            settings.StdLib = false;
            settings.WarningLevel = 2;
            settings.Optimize = true;
            settings.GenerateDebugInfo = true;

            var projectFiles = projectDirectoryInfo.GetFiles("*.csproj", SearchOption.TopDirectoryOnly);
            if (projectFiles.Length == 0)
            {
                Console.WriteLine("NO .CSPROJ FILES FOUND");
                return;
            }
            var projectFile = File.ReadAllText(projectFiles[0].FullName);
            var compileIncludeMatches = Regex.Matches(projectFile, "<Compile Include=\"([^\"]*)\" />");
            foreach (Match match in compileIncludeMatches)
            {
                var name = Path.Combine(projectDirectoryInfo.FullName, match.Groups[1].Value);
                settings.SourceFiles.Add(new SourceFile(name, name, 0));
            }
            var _writer = new StringWriter(_stringBuilder);
            var printer = new StreamReportPrinter(_writer);
            var ctx = new CompilerContext(settings, printer);
            _evaluator = new Evaluator(ctx);
        }

        public bool Run()
        {
            _stringBuilder.Length = 0;
            _evaluator.Compile(@"namespace script{}");  //Just compile my shit up fam.
            var result = _evaluator.Run("new script.Main();");  //Actually run it.
            _error = _stringBuilder.ToString().Trim();
            if (!string.IsNullOrEmpty(_error)) Console.WriteLine("ERROR!\n" + _error);
            return result;
        }

        public void RegisterAssemblies(params Assembly[] apiAssembly)
        {
            foreach (var assembly in apiAssembly)
            {
                _evaluator.ReferenceAssembly(assembly);
            }
        }
    }
}
