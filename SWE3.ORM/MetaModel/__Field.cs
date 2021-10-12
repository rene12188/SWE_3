using System;
using System.Reflection;

namespace SWE3.ORM.MetaModel
{
    /// <summary>This class holds field metadata.</summary>
    internal class __Field
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="entity">Parent entity.</param>
        public __Field(__Entity entity)
        {
            Entity = entity;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the parent entity.</summary>
        public __Entity Entity
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

        public bool IsExternal
        {
            get; internal set;
        } = false;

        public bool IsInternal
        {
            get; internal set;
        } = false;



        public bool GiveToChild
        {
            get; internal set;
        } = false;

        /// <summary>Gets if the column is nullable.</summary>
        public bool IsNullable
        {
            get; internal set;
        } = false;

        public object ToColumnType(object value)
        {
            if (IsForeignKey)
            {
                //return Type._GetEntity().PrimaryKey.ToColumnType(Type._GetEntity().PrimaryKey.GetValue(value));
                //Mapper.SaveObject(value);
            }



            if (Type == ColumnType) { return value; }

            if (value is bool)
            {
                if (ColumnType == typeof(int)) { return (((bool)value) ? 1 : 0); }
                if (ColumnType == typeof(short)) { return (short)(((bool)value) ? 1 : 0); }
                if (ColumnType == typeof(long)) { return (long)(((bool)value) ? 1 : 0); }
            }

            return value;
        }
        public object GetValue(object obj)
        {
            if (Member is PropertyInfo) { return ((PropertyInfo)Member).GetValue(obj); }

            throw new NotSupportedException("Member type not supported.");
        }
    }
}
