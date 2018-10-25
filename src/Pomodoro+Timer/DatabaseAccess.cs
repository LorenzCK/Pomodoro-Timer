using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Sql = SQLitePCL.raw;

namespace PomodoroTimer {

    public static class DatabaseAccess {

        private const string DatabaseFilename = "store.db";
        private const int CurrentDataVersion = 1;

        private static SQLitePCL.sqlite3 _db;

        public static async Task Initialize() {
            Sql.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            if (Sql.sqlite3_initialize() != Sql.SQLITE_OK) {
                throw new InvalidOperationException("Cannot initialize SQLite");
            }

            var filePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseFilename);
            Debug.WriteLine("Opening database at " + filePath);

            if (Sql.sqlite3_open_v2(filePath, out _db, Sql.SQLITE_OPEN_READWRITE | Sql.SQLITE_OPEN_CREATE, null) != Sql.SQLITE_OK) {
                throw new Exception("Cannot open or create SQLite database");
            }

            await ApplicationData.Current.SetVersionAsync(CurrentDataVersion, UpdateDataVersion);
        }

        private static void UpdateDataVersion(SetVersionRequest vr) {
            var deferral = vr.GetDeferral();

            Debug.WriteLine("Updating data from " + vr.CurrentVersion + " to " + vr.DesiredVersion);

            if(vr.CurrentVersion < 1 && vr.DesiredVersion >= 1) {
                Debug.WriteLine("Upgrading data to v1");

                PerformAction("CREATE TABLE Runs (`Start` TEXT NOT NULL, `End` TEXT, `Seconds` INTEGER)");
            }

            deferral.Complete();
        }

        /// <summary>
        /// Performs a SQL query without results.
        /// </summary>
        private static void PerformAction(string sql) {
            if (Sql.sqlite3_prepare_v2(_db, sql, out SQLitePCL.sqlite3_stmt stmt) != Sql.SQLITE_OK) {
                throw new ArgumentException($"Cannot prepare SQL statement '{sql}'");
            }
            if (Sql.sqlite3_step(stmt) != Sql.SQLITE_DONE) {
                throw new ArgumentException("SQL statement did not step to done");
            }
            if (Sql.sqlite3_finalize(stmt) != Sql.SQLITE_OK) {
                throw new ArgumentException("Cannot finalize SQL statement");
            }
        }

        /// <summary>
        /// Performs an insert SQL query and returns row ID.
        /// </summary>
        private static long PerformInsert(string sql) {
            if (Sql.sqlite3_prepare_v2(_db, sql, out SQLitePCL.sqlite3_stmt stmt) != Sql.SQLITE_OK) {
                throw new ArgumentException($"Cannot prepare SQL statement '{sql}'");
            }
            if (Sql.sqlite3_step(stmt) != Sql.SQLITE_DONE) {
                throw new ArgumentException("SQL statement did not step to done");
            }
            if (Sql.sqlite3_finalize(stmt) != Sql.SQLITE_OK) {
                throw new ArgumentException("Cannot finalize SQL statement");
            }

            return Sql.sqlite3_last_insert_rowid(_db);
        }

    }

}
