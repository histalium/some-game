using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public class MinionNotFoundException
        : Exception
    {
        public MinionNotFoundException()
            : base("Minion is not found")
        {

        }
    }
}
