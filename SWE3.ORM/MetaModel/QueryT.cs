using System;
using System.Collections;
using System.Collections.Generic;

namespace SWE3.ORM.MetaModel
{
    /// <summary>This class represents a query.</summary>
    /// <typeparam name="T">Type.</typeparam>
    public sealed class Query<T>: IEnumerable<T>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private members                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Previous query.</summary>
        private Query<T> _Previous;

        /// <summary>Operation.</summary>
        private __QueryOperation _Op = __QueryOperation.NOP;

        /// <summary>Arguments.</summary>
        private object[] _Args = null;

        /// <summary>Query result values.</summary>
        private List<T> _InternalValues = null;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="previous">Previous query.</param>
        internal Query(Query<T> previous)
        {
            _Previous = previous;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private properties                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Fills the object.</summary>
        /// <param name="t">Type.</param>
        /// <param name="localCache">Local cache.</param>
        private void _Fill(Type t, ICollection<object> localCache)
        {
            List<Query<T>> ops = new List<Query<T>>();

            Query<T> q = this;
            while(q != null)
            {
                ops.Insert(0, q);
                q = q._Previous;
            }

            __Entity ent = t._GetEntity();

            string sql = ent.GetSQL();
            List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();
            string conj = (string.IsNullOrWhiteSpace(ent.SubsetQuery) ? " WHERE (" : " AND (");
            bool not = false;
            string opbrk = "";
            string clbrk = "";
            int n = 0;
            string op;

            __Field field;
            foreach(Query<T> i in ops)
            {
                switch(i._Op)
                {
                    case __QueryOperation.OR:
                        if(!conj.EndsWith("(")) { conj = " OR "; }
                        break;
                    case __QueryOperation.NOT:
                        not = true; break;
                    case __QueryOperation.GRP:
                        opbrk += "("; break;
                    case __QueryOperation.ENDGRP:
                        clbrk += ")"; break;
                    case __QueryOperation.EQUALS:
                    case __QueryOperation.LIKE:
                        field = ent.GetFieldByName((string) i._Args[0]);

                        if(i._Op == __QueryOperation.LIKE)
                        {
                            op = (not ? " NOT LIKE " : " LIKE ");
                        }
                        else { op = (not ? " != " : " = "); }

                        sql += clbrk + conj + opbrk;
                        sql += (((bool) i._Args[2] ? "Lower(" + field.ColumnName + ")" : field.ColumnName) + op +
                                ((bool) i._Args[2] ? "Lower(:p" + n.ToString() + ")" : ":p" + n.ToString()));

                        if((bool) i._Args[2]) { i._Args[1] = ((string) i._Args[1]).ToLower(); }
                        parameters.Add(new Tuple<string, object>(":p" + n++.ToString(), field.ToColumnType(i._Args[1])));

                        opbrk = clbrk = ""; conj = " AND "; not = false;
                        break;
                    case __QueryOperation.IN:
                        field = ent.GetFieldByName((string) i._Args[0]);

                        sql += clbrk + conj + opbrk;
                        sql += field.ColumnName + (not ? " NOT IN (" : " IN (");
                        for(int k = 1; k < i._Args.Length; k++)
                        {
                            if(k > 1) { sql += ", "; }
                            sql += (":p" + n.ToString());
                            parameters.Add(new Tuple<string, object>(":p" + n++.ToString(), field.ToColumnType(i._Args[k])));
                        }
                        sql += ")";

                        opbrk = clbrk = ""; conj = " AND "; not = false;
                        break;
                    case __QueryOperation.GT:
                    case __QueryOperation.LT:
                        field = ent.GetFieldByName((string) i._Args[0]);

                        if(i._Op == __QueryOperation.GT)
                        {
                            op = (not ? " <= " : " > ");
                        }
                        else { op = (not ? " >= " : " < "); }

                        sql += clbrk + conj + opbrk;
                        sql += (field.ColumnName + op + ":p" + n.ToString());

                        parameters.Add(new Tuple<string, object>(":p" + n++.ToString(), field.ToColumnType(i._Args[1])));

                        opbrk = clbrk = ""; conj = " AND "; not = false;
                        break;
                }
            }

            if(!conj.EndsWith("(")) { sql += ")"; }
            Mapper._FillList(t, _InternalValues, sql, parameters, localCache);
        }


        /// <summary>Gets the query values.</summary>
        private List<T> _Values
        {
            get
            {
                if(_InternalValues == null)
                {
                    _InternalValues = new List<T>();
                    
                    if(typeof(T).IsAbstract || typeof(T)._GetEntity().IsMaterial)
                    {
                        ICollection<object> localCache = null;
                        foreach(Type i in typeof(T)._GetChildTypes())
                        {
                            _Fill(i, localCache);
                        }
                    }
                    else { _Fill(typeof(T), null); }
                }

                return _InternalValues;
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private methods                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets the object operation and arguments and returns a new Query.</summary>
        /// <param name="op">Operation.</param>
        /// <param name="args">Arguments.</param>
        /// <returns>Query.</returns>
        private Query<T> _SetOp(__QueryOperation op, params object[] args)
        {
            _Op = op;
            _Args = args;

            return new Query<T>(this);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Adds a not operation to the query.</summary>
        /// <returns>Query.</returns>
        public Query<T> Not()
        {
            return _SetOp(__QueryOperation.NOT);
        }


        /// <summary>Adds an and operation to the query.</summary>
        /// <returns>Query.</returns>
        public Query<T> And()
        {
            return _SetOp(__QueryOperation.AND);
        }


        /// <summary>Adds an or operation to the query.</summary>
        /// <returns>Query.</returns>
        public Query<T> Or()
        {
            return _SetOp(__QueryOperation.OR);
        }


        /// <summary>Adds a begin group operation to the query.</summary>
        /// <returns>Query.</returns>
        public Query<T> BeginGroup()
        {
            return _SetOp(__QueryOperation.GRP);
        }


        /// <summary>Adds an end group operation to the query.</summary>
        /// <returns>Query.</returns>
        public Query<T> EndGroup()
        {
            return _SetOp(__QueryOperation.ENDGRP);
        }


        /// <summary>Adds an equals operation to the query.</summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value.</param>
        /// <param name="ignoreCase">String comparison ignore case flag.</param>
        /// <returns>Query.</returns>
        public Query<T> Equals(string field, object value, bool ignoreCase = false)
        {
            return _SetOp(__QueryOperation.EQUALS, field, value, ignoreCase);
        }


        /// <summary>Adds a like operation to the query.</summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value.</param>
        /// <param name="ignoreCase">String comparison ignore case flag.</param>
        /// <returns>Query.</returns>
        public Query<T> Like(string field, object value, bool ignoreCase = false)
        {
            return _SetOp(__QueryOperation.LIKE, field, value, ignoreCase);
        }


        /// <summary>Adds an in operation to the query.</summary>
        /// <param name="field">Field name.</param>
        /// <param name="values">Values.</param>
        /// <returns>Query.</returns>
        public Query<T> In(string field, params object[] values)
        {
            List<object> v = new List<object>(values);
            v.Insert(0, field);
            return _SetOp(__QueryOperation.LIKE, v.ToArray());
        }


        /// <summary>Adds a greater than operation to the query.</summary>
        /// <param name="field">Field name.</param>
        /// <param name="values">Values.</param>
        /// <returns>Query.</returns>
        public Query<T> GreaterThan(string field, object value)
        {
            return _SetOp(__QueryOperation.GT, field, value);
        }


        /// <summary>Adds a greater than operation to the query.</summary>
        /// <param name="field">Field name.</param>
        /// <param name="values">Values.</param>
        /// <returns>Query.</returns>
        public Query<T> LessThan(string field, object value)
        {
            return _SetOp(__QueryOperation.LT, field, value);
        }


        /// <summary>Returns a list for the query results.</summary>
        /// <returns>List.</returns>
        public List<T> ToList()
        {
            return new List<T>(_Values);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IEnumerable<T>                                                                                       //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Returns an enumerator for this object.</summary>
        /// <returns>Enumerator.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _Values.GetEnumerator();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] IEnumerable                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Returns an enumerator for this object.</summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Values.GetEnumerator();
        }
    }
}
