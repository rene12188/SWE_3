using System;
using System.Collections;
using System.Collections.Generic;

namespace SWE3.ORM.MetaModel
{

     public sealed class Query<T>: IEnumerable<T>
    {
        private Query<T> _previous;
        private QueryOperationEnum _operation = QueryOperationEnum.Nop;
        private object[] _arguments = null;
        private List<T> _inteneralValueList = null;

        internal Query(Query<T> previous)
        {
            _previous = previous;
        }

        private void Fill(Type type, ICollection<object> localCacheCollection)
        {
            List<Query<T>> operationList = new List<Query<T>>();

            Query<T> query = this;
            while(query != null)
            {
                operationList.Insert(0, query);
                query = query._previous;
            }

            __Entity entity = type._GetEntity();
            string sql = entity.GetSql();
            List<Tuple<string, object>> parameterList = new List<Tuple<string, object>>();
            string conjunction = " WHERE ";
            bool not = false;
            string openbracket = "";
            string closebracket = "";
            int k = 0;
            string op;
            __Field field;

            foreach(var operation in operationList)
            {
                switch(operation._operation)
                {
                    case QueryOperationEnum.Or:
                        if (conjunction != " WHERE ")
                        {
                            conjunction = " OR ";
                        }
                        break;

                    case QueryOperationEnum.Not:
                        not = true; break;

                    case QueryOperationEnum.Grp:
                        openbracket += "("; break;

                    case QueryOperationEnum.Endgrp:
                        closebracket += ")"; break;

                    case QueryOperationEnum.Equals:
                    case QueryOperationEnum.Like:
                        field = entity.GetFieldByName((string) operation._arguments[0]);

                        if(operation._operation == QueryOperationEnum.Like)
                        {
                            op = not ? " NOT LIKE " : " LIKE ";
                        }
                        else
                        {
                            op = not ? " != " : " = ";
                        }

                        sql += closebracket + conjunction + openbracket;
                        sql += ((bool) operation._arguments[2] ? "Lower(" + field.ColumnName + ")" : field.ColumnName) + op +
                                ((bool) operation._arguments[2] ? "Lower(:p" + k.ToString() + ")" : ":p" + k.ToString());

                        if ((bool)operation._arguments[2])
                        {
                            operation._arguments[1] = ((string) operation._arguments[1]).ToLower();
                        }
                        parameterList.Add(new Tuple<string, object>(":p" + k++.ToString(), field.ToColumnType(operation._arguments[1])));

                        openbracket = closebracket = ""; conjunction = " AND "; not = false;
                        break;

                    case QueryOperationEnum.In:
                        field = entity.GetFieldByName((string) operation._arguments[0]);

                        sql += closebracket + conjunction + openbracket;
                        sql += field.ColumnName + (not ? " NOT IN (" : " IN (");
                        for(int l = 1; l < operation._arguments.Length; l++)
                        {
                            if (l > 1)
                            {
                                sql += ", ";
                            }
                            sql += ":p" + k.ToString();
                            parameterList.Add(new Tuple<string, object>(":p" + k++.ToString(), field.ToColumnType(operation._arguments[l])));
                        }
                        sql += ")";

                        openbracket = closebracket = ""; conjunction = " AND "; not = false;
                        break;

                    case QueryOperationEnum.Gt:
                    case QueryOperationEnum.Lt:
                        field = entity.GetFieldByName((string) operation._arguments[0]);

                        if(operation._operation == QueryOperationEnum.Gt)
                        {
                            op = not ? " <= " : " > ";
                        }
                        else
                        {
                            op = not ? " >= " : " < ";
                        }

                        sql += closebracket + conjunction + openbracket;
                        sql += (field.ColumnName + op + ":p" + k.ToString());

                        parameterList.Add(new Tuple<string, object>(":p" + k++.ToString(), field.ToColumnType(operation._arguments[1])));

                        openbracket = closebracket = ""; conjunction = " AND "; not = false;
                        break;
                }
            }

            Orm._FillList(type, _inteneralValueList, sql, parameterList);
        }

        private List<T> _values
        {
            get
            {
                if(_inteneralValueList == null)
                {
                    _inteneralValueList = new List<T>();
                    
                  
                     Fill(typeof(T), null); 
                }

                return _inteneralValueList;
            }
        }

        private Query<T> _SetOp(QueryOperationEnum operation, params object[] arguments)
        {
            _operation = operation;
            _arguments = arguments;

            return new Query<T>(this);
        }

        public Query<T> Not()
        {
            return _SetOp(QueryOperationEnum.Not);
        }

        public Query<T> And()
        {
            return _SetOp(QueryOperationEnum.And);
        }

        public Query<T> Or()
        {
            return _SetOp(QueryOperationEnum.Or);
        }

        public Query<T> BeginGroup()
        {
            return _SetOp(QueryOperationEnum.Grp);
        }

        public Query<T> EndGroup()
        {
            return _SetOp(QueryOperationEnum.Endgrp);
        }

        public Query<T> Equals(string field, object value, bool ignoreCase = false)
        {
            return _SetOp(QueryOperationEnum.Equals, field, value, ignoreCase);
        }

        public Query<T> Like(string field, object value, bool ignoreCase = false)
        {
            return _SetOp(QueryOperationEnum.Like, field, value, ignoreCase);
        }

        public Query<T> In(string field, params object[] values)
        {
            List<object> v = new List<object>(values);
            v.Insert(0, field);
            return _SetOp(QueryOperationEnum.Like, v.ToArray());
        }

        public Query<T> GreaterThan(string field, object value)
        {
            return _SetOp(QueryOperationEnum.Gt, field, value);
        }

        public Query<T> LessThan(string field, object value)
        {
            return _SetOp(QueryOperationEnum.Lt, field, value);
        }

        public List<T> ToList()
        {
            return new List<T>(_values);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }
    }
}


