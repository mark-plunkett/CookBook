﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public record BaseEvent(long Version, IDomainEvent Event)
    {
    }
}