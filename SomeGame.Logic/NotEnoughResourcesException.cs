using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
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
