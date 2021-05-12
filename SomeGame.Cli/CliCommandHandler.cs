using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal abstract class CliCommandHandler
    {
        public CliCommandHandler(string commandPattern, string[] argNames)
        {
            CommandPattern = new Regex(commandPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            ArgNames = argNames;
        }

        public Regex CommandPattern { get; }

        public string[] ArgNames { get; }

        public abstract IEnumerable<string> Handle(string[] args);
    }
}
