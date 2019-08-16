using System;
using System.Collections.Generic;
using System.Text;

namespace Dependous.Configuration
{
    public class ProbingPath
    {
        public string Path { get; }
        public Func<AssemblySearchPatternFactory,AssemblySearchPatternFactory> Pattern { get; }

        internal ProbingPath(string path, Func<AssemblySearchPatternFactory,AssemblySearchPatternFactory> func)
        {
            Path = path;
            Pattern = func;
        }
    }
}
