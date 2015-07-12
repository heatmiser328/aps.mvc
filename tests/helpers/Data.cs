using System;
using System.Data;
using System.IO;

using ica.aps.data.interfaces;

namespace TestHelpers
{
    internal static class TestData
    {

        internal static void ResetBlank(IDatabase db)
        {
			/*
            File.Copy(Path.Combine(TestHelpers.TestPaths.TestFolderPath, "src\\App.Config"),
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ica.aps.services.dll.config"), true);
			*/
            if (db.IsSqlServerCeProvider)
            {
                File.Copy(Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, "aps_backup.sdf"),
                                    Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, "aps.sdf"), true);
            }
            else if (db.IsSqlServerProvider)
            {
                using (IDbConnection conn = db.Create())
                {
                    conn.Open();

                    string sql = string.Format(@"RESTORE DATABASE {0} FROM DISK = '{1}' WITH REPLACE",
                                                conn.Database,
                                                Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, "aps.bak"));
                    // change to master
                    conn.ChangeDatabase("master");
                    // perform restore
		            using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static void Reset(IDatabase db)
        {
            Reset(db, ".sql");
        }

        internal static void Reset(IDatabase db, string sqlscript)
        {
            ResetBlank(db);

            if (!string.IsNullOrEmpty(sqlscript))
                ExecuteScript(db, Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, sqlscript));
        }

        internal static void ExecuteScript(IDatabase db, string scriptfile)
        {
            using (StreamReader reader = new StreamReader(scriptfile))
            {
                string sqlscript = reader.ReadToEnd();
                string[] commands = sqlscript.Split(new string[] { "GO", "Go", "gO", "go", ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (commands != null && commands.Length > 0)
                {
                    using (IDbConnection conn = db.Create())
                    {
                        conn.Open();
                        foreach (string command in commands)
                        {
                            if (!string.IsNullOrEmpty(command.Replace(System.Environment.NewLine, "")) && 
                                 command != ";" && 
                                 string.Compare(command, "go", true) != 0)
                            {
                                using (IDbCommand cmd = conn.CreateCommand())
                                {
                                    try
                                    {
                                        //System.Diagnostics.Trace.Write(command);
                                        cmd.CommandText = command;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        while (ex != null)
                                        {
                                            System.Diagnostics.Trace.Write(ex.Message);
                                            ex = ex.InnerException;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }            
        }

        internal static void ExecuteSQL(IDatabase db, string sql)
        {
            using (IDbConnection conn = db.Create())
            {
                conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    //System.Diagnostics.Trace.Write(sql);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal static void ExecuteSQL(IDatabase db, string[] sqlcmds)
        {
            using (IDbConnection conn = db.Create())
            {
                conn.Open();
                IDbTransaction trans = conn.BeginTransaction();
                foreach (string sql in sqlcmds)
                {
                    //System.Diagnostics.Trace.Write(sql);
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = trans;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
                trans.Commit();
            }
        }

        internal static object ExecuteSQLScalar(IDatabase db, string sql)
        {
            using (IDbConnection conn = db.Create())
            {
                conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    //System.Diagnostics.Trace.Write(sql);
                    cmd.CommandText = sql;
                    return cmd.ExecuteScalar();                    
                }
            }
        }
    }
}
