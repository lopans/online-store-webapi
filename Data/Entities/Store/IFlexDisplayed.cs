﻿using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Store
{
    public interface IFlexDisplayed
    {
        string Color { get; set; }
        FlexSize FlexSize { get; set; }
    }
}