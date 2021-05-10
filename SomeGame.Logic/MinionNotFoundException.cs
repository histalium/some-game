using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class MinionNotFoundException
        : Exception
    {
        public MinionNotFoundException(string cardId)
            : base("Minion is not found")
        {
            CardId = cardId;
        }

        public string CardId { get; }
    }
}
