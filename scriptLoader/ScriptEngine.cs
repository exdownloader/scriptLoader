using System;
using System.IO;
using System.Reflection;
using System.Text;
using Mono.CSharp;

namespace scriptLoader
{
    public class ScriptEngine
    {
        private Evaluator _evaluator;

        private StringBuilder _stringBuilder;
        private ModuleContainer _module;
        private ReflectionImporter _importer;
        private MethodInfo _importTypes;
        private string _error = string.Empty;

        public ScriptEngine()
        {
            _stringBuilder = new StringBuilder();
            TextWriter _writer = new StringWriter(_stringBuilder);
            CompilerSettings settings = new CompilerSettings();
            settings.LoadDefaultReferences = false;
            settings.StdLib = false;
            settings.WarningLevel = 2;
            ReportPrinter printer = new StreamReportPrinter(_writer);
            CompilerContext ctx = new CompilerContext(settings, printer);
            _evaluator = new Evaluator(ctx);

            InitEvaluator();
        }

        public bool Run(string code)
        {
            _stringBuilder.Length = 0;
            var result = _evaluator.Run(code);
            _evaluator.Run("new script.Main();");
            _error = _stringBuilder.ToString().Trim();
            if(!string.IsNullOrEmpty(_error)) Console.WriteLine("ERROR!\n" + _error);
            return result;
        }

        public bool Evaluate<T>(string code, out T result)
        {
            object resultVal;
            bool resultSet;

            result = default(T);
            _stringBuilder.Length = 0;
            string res = _evaluator.Evaluate(code, out resultVal, out resultSet);
            _error = _stringBuilder.ToString();

            if (res != null)
            {
                return false;
            }

            if (resultSet)
            {
                result = (T)resultVal;
            }
            return true;
        }

        public string GetLastError()
        {
            return _error;
        }

        public void RegisterAssemblies(params Assembly[] apiAssembly)
        {
            foreach (var assembly in apiAssembly)
            {
                _evaluator.ReferenceAssembly(assembly);
            }
        }

        public void RegisterTypes(params Type[] types)
        {
            _importTypes.Invoke(_importer, new object[] { types, _module.GlobalRootNamespace, false });
        }

        private void InitEvaluator()
        {
            var fieldInfo1 = _evaluator.GetType().GetField("importer", BindingFlags.Instance | BindingFlags.NonPublic);
            _importer = (ReflectionImporter)fieldInfo1.GetValue(_evaluator);

            var fieldInfo2 = _evaluator.GetType().GetField("module", BindingFlags.Instance | BindingFlags.NonPublic);
            _module = (ModuleContainer)fieldInfo2.GetValue(_evaluator);

            _importTypes = _importer.GetType().GetMethod("ImportTypes", BindingFlags.NonPublic | BindingFlags.Instance, null,
                CallingConventions.Any, new Type[] { typeof(Type[]), typeof(Namespace), typeof(bool) }, null);
        }
        
    }
}
