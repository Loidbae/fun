﻿using fun.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fun.Communication
{
    public interface IPerceiver
    {
        IEnumerable<Entity> Seen { get; }
    }
}
