using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public class MinionNotActiveException
        : Exception
    {
        public MinionNotActiveException()
            : base("Minion is not active")
        {

        }
    }
}
