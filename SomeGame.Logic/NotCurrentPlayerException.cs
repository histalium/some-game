using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class NotCurrentPlayerException
        : Exception
    {
        public NotCurrentPlayerException()
            : base("Not the current player")
        {

        }
    }
}
