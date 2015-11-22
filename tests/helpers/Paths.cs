using System;
using System.IO;

namespace TestHelpers
{
	internal static class TestPaths
	{
        internal static string TestFolderPath
		{
			get
			{
				if (_testfolderpath == null)
				{
                    string dir = AppDomain.CurrentDomain.BaseDirectory;
                    if (dir.Contains("bin\\Debug"))
                        _testfolderpath = dir.Substring(0, dir.IndexOf("bin\\Debug"));
                    else
                        _testfolderpath = dir.Substring(0, dir.IndexOf("bin\\Release"));
				}
				return _testfolderpath;
			}
		}

        internal static string TestDataFolderPath
        {
            get
            {
                if (_testdatafolderpath == null)
                {
                    _testdatafolderpath = Path.Combine(TestFolderPath, "data\\");
                }
                return _testdatafolderpath;
            }
        }

        internal static string SchemaFolderPath
        {
            get
            {
                if (_schemafolderpath == null)
                {
                    _schemafolderpath = Path.Combine(TestFolderPath, "..", "data", "schema");
                }
                return _schemafolderpath;
            }
        }

        private static string _testfolderpath = null;
        private static string _testdatafolderpath = null;
        private static string _schemafolderpath = null;
	}
}
