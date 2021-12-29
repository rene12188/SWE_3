﻿using System;

namespace SWE3.OrmFramework
{

    public class FieldAttribute: Attribute
    {
        public string ColumnName = null;

        public Type ColumnType = null;

        public bool Nullable = false;
    }
}
