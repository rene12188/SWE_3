namespace SWE3.ORM.MetaModel
{
    internal enum QueryOperationEnum
    {
        Nop = 0,
        Not,
        And,
        Or,
        Grp,
        Endgrp,
        Equals,
        Like,
        In,
        Gt,
        Lt,
    }
}
