using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public class NotEnoughResourcesException
        : Exception
    {
        public NotEnoughResourcesException()
            : base ("Not enough resources")
        {
                
        }
    }
}
