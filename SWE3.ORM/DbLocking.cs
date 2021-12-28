using System;
using System.Data;
using SWE3.ORM.MetaModel;

namespace SWE3.ORM
{
    /// <summary>This class implements a database-based locking mechanism.</summary>
    public class DbLocking: ILocking
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        public DbLocking()
        {
            SessionKey = Guid.NewGuid().ToString();

            try
            {
                IDbCommand cmd = Mapper.__conn.CreateCommand();
                cmd.CommandText = "CREATE TABLE LOCKS (JCLASS VARCHAR(48) NOT NULL, JOBJECT VARCHAR(48) NOT NULL, JTIME TIMESTAMP NOT NULL, JOWNER VARCHAR(48) NOT NULL)";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd = Mapper.__conn.CreateCommand();
                cmd.CommandText = "CREATE UNIQUE INDEX UX_LOCKS ON LOCKS(JCLASS, JOBJECT)";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch(Exception) {}
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private methods                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets class and object key for an object.</summary>
        /// <param name="obj">Object.</param>
        /// <returns>Returns a tuple containing class and object key.</returns>
        private (string ClassKey, string ObjectKey) _GetKeys(object obj)
        {
            __Entity ent = obj._GetEntity();
            return (ent.TableName, ent.PrimaryKey.ToColumnType(ent.PrimaryKey.GetValue(obj)).ToString());
        }


        /// <summary>Gets the current lock owner for an object.</summary>
        /// <param name="obj">Object.</param>
        /// <returns>Owner key.</returns>
        private string _GetLock(object obj)
        {
            var keys = _GetKeys(obj);
            string rval = null;

            IDbCommand cmd = Mapper.__conn.CreateCommand();
            cmd.CommandText = "SELECT JOWNER FROM LOCKS WHERE JCLASS = :c AND JOBJECT = :o";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":c";
            p.Value = keys.ClassKey;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":o";
            p.Value = keys.ObjectKey;
            cmd.Parameters.Add(p);

            IDataReader re = cmd.ExecuteReader();
            if(re.Read())
            {
                rval = re.GetString(0);
            }
            re.Close();
            re.Dispose();
            cmd.Dispose();

            return rval;
        }


        /// <summary>Creates a lock on an object.</summary>
        /// <param name="obj">Object.</param>
        private void _CreateLock(object obj)
        {
            var keys = _GetKeys(obj);
            
            IDbCommand cmd = Mapper.__conn.CreateCommand();
            cmd.CommandText = "INSERT INTO LOCKS(JCLASS, JOBJECT, JTIME, JOWNER) VALUES (:c, :o, Current_Timestamp, :s)";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":c";
            p.Value = keys.ClassKey;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":o";
            p.Value = keys.ObjectKey;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":s";
            p.Value = SessionKey;
            cmd.Parameters.Add(p);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception) {}

            cmd.Dispose();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets this session's key.</summary>
        public string SessionKey
        {
            get; private set;
        }


        /// <summary>Gets or sets the locking timeout.</summary>
        public int Timeout
        {
            get; set;
        } = 180;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Purges timed out locks.</summary>
        public void Purge()
        {
            IDbCommand cmd = Mapper.__conn.CreateCommand();
            cmd.CommandText = "DELETE FROM LOCKS WHERE ((JulianDay(Current_Timestamp) - JulianDay(JTIME)) * 86400) > :t";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":t";
            p.Value = Timeout;
            cmd.Parameters.Add(p);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // [interface] ILocking                                                                                             //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Locks an object.</summary>
        /// <param name="obj">Object.</param>
        /// <exception cref="ObjectLockedException">Thrown when the object is already locked by another instance.</exception>
        public virtual void Lock(object obj)
        {
            string owner = _GetLock(obj);
            
            if(owner == SessionKey) return;
            if(owner == null) 
            {
                _CreateLock(obj);
                owner = _GetLock(obj);
            }

            if(owner != SessionKey) { throw new Exception(); }
        }


        /// <summary>Releases a lock on an object.</summary>
        /// <param name="obj">Object.</param>
        public virtual void Release(object obj)
        {
            var keys = _GetKeys(obj);

            IDbCommand cmd = Mapper.__conn.CreateCommand();
            cmd.CommandText = "DELETE FROM LOCKS WHERE JCLASS = :c AND JOBJECT = :o AND JOWNER = :s";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":c";
            p.Value = keys.ClassKey;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":o";
            p.Value = keys.ObjectKey;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":s";
            p.Value = SessionKey;
            cmd.Parameters.Add(p);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
