﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public record Card
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public IReadOnlyCollection<ResourceAmount> Cost { get; init; }
    }
}