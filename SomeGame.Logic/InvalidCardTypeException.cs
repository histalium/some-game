using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class InvalidCardTypeException
        : Exception
    {
        public InvalidCardTypeException()
            : base ("Invalid type of card")
        {

        }
    }
}
