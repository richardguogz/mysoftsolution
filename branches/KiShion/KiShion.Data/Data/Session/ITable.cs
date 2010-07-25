﻿using System;

namespace KiShion.Data
{
    interface ITable
    {
        string OriginalName { get; }
        string Prefix { set; }
        string Suffix { set; }
        Field this[string fieldName] { get; }
    }
}
