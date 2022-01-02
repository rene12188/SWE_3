namespace SWE3.OrmFramework.MetaModel
{
    /// Definition for QueryOperation
    internal enum __QueryOperation: int
    {
        NOP     =  0,
        NOT     =  1,
        AND     =  2,
        OR      =  3,
        GRP     =  4,
        ENDGRP  =  5,
        EQUALS  =  6,
        LIKE    =  7,
        IN      =  8,
        GT      =  9,
        LT      = 10
    }
}
