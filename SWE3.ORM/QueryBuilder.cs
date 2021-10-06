﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE3.ORM.MetaModel;
using Npgsql;

namespace SWE3.ORM
{
    internal static class QueryBuilder
    {
        public static string InsertBuilder(__Entity ent)
        {
            int fieldnumber = ent.Fields.Length;

            string fields = "(";

            string values = "(";

            for ( int i = 0 ; i < ent.Fields.Length; i++)
            {
                fields += "col" + i;

                values += "val" + i;

                if (i == ent.Fields.Length - 1)
                {
                    break;
                }
                fields += ",";
                values += ",";
            }

            fields += ")";
            values += ")";


            string cmd = $"INSERT INTO {ent.TableName} {fields} Values {values}";



            return cmd;


        }
    }
}