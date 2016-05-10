﻿using fun.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = fun.Core.Environment;

namespace fun.Network
{
    public sealed class NetworkProcessClientElement : Element
    {
        public NetworkProcessClientElement(Environment environment, Entity entity)
            : base(environment, entity)
        {

        }
    }
}