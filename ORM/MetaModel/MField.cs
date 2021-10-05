﻿using System;
using System.Reflection;



namespace SWE3.Demo.OrmFramework.MetaModel
{
    /// <summary>This class holds field metadata.</summary>
    internal class MField
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="entity">Parent entity.</param>
        public MField(MEntity entity)
        {
            Entity = entity;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the parent entity.</summary>
        public MEntity Entity
        {
            get; private set;
        }


        /// <summary>Gets the field member.</summary>
        public MemberInfo Member
        {
            get; internal set;
        }


        /// <summary>Gets the field type.</summary>
        public Type Type
        {
            get
            {
                if(Member is PropertyInfo) { return ((PropertyInfo) Member).PropertyType; }

                throw new NotSupportedException("Member type not supported.");
            }
        }


        /// <summary>Gets the column name in table.</summary>
        public string ColumnName
        {
            get; internal set;
        }


        /// <summary>Gets the column database type.</summary>
        public Type ColumnType
        {
            get; internal set;
        }


        /// <summary>Gets if the column is a primary key.</summary>
        public bool IsPrimaryKey
        {
            get; internal set;
        } = false;


        /// <summary>Gets if the column is a foreign key.</summary>
        public bool IsForeignKey
        {
            get; internal set;
        } = false;


        /// <summary>Gets if the column is nullable.</summary>
        public bool IsNullable
        {
            get; internal set;
        } = false;
    }
}
