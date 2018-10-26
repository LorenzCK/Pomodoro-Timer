using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        /// <summary>
        /// Initializes the database.
        /// </summary>
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
        /// Registers a new run and returns its row ID.
        /// </summary>
        public static Task<long> NewRun() {
            return Task.Run(() => {
                return PerformInsert("INSERT INTO Runs (`Start`) VALUES (datetime('now'))");
            });
        }

        public static Task CompleteRun(long runId) {
            return Task.Run(() => {
                var start = PerformScalarDateTime($"SELECT `Start` FROM Runs WHERE rowid = {runId}");

                var length = (int)((DateTimeOffset.UtcNow - start).TotalSeconds);

                PerformAction($"UPDATE Runs SET `End` = datetime('now'), `Seconds` = {length} WHERE rowid = {runId}");
            });
        }

        /// <summary>
        /// Performs a wrapped SQL query with a prepared statement.
        /// </summary>
        /// <typeparam name="T">Query return type.</typeparam>
        /// <param name="sql">SQL query.</param>
        /// <param name="f">Statement processing function.</param>
        private static T PerformSql<T>(string sql, Func<SQLitePCL.sqlite3_stmt, T> f) {
            if (Sql.sqlite3_prepare_v2(_db, sql, out SQLitePCL.sqlite3_stmt stmt) != Sql.SQLITE_OK) {
                throw new ArgumentException($"Cannot prepare SQL statement '{sql}'");
            }

            T ret = f(stmt);

            if (Sql.sqlite3_finalize(stmt) != Sql.SQLITE_OK) {
                throw new ArgumentException("Failed to finalize SQL statement");
            }

            return ret;
        }

        /// <summary>
        /// Performs a SQL query without results.
        /// </summary>
        private static void PerformAction(string sql) {
            PerformSql(sql, stmt => {
                if (Sql.sqlite3_step(stmt) != Sql.SQLITE_DONE) {
                    throw new ArgumentException("SQL statement not done");
                }

                return true;
            });
        }

        /// <summary>
        /// Performs an insert SQL query and returns new row ID.
        /// </summary>
        private static long PerformInsert(string sql) {
            return PerformSql(sql, stmt => {
                if (Sql.sqlite3_step(stmt) != Sql.SQLITE_DONE) {
                    throw new ArgumentException("SQL statement not done");
                }

                return Sql.sqlite3_last_insert_rowid(_db);
            });
        }

        private static long PerformScalarInt64(string sql) {
            return PerformSql(sql, stmt => {
                if (Sql.sqlite3_step(stmt) != Sql.SQLITE_ROW) {
                    throw new ArgumentException("Scalar SQL query did not return a result");
                }

                long ret = Sql.sqlite3_column_int64(stmt, 0);

                int resultLastStep = Sql.sqlite3_step(stmt);
                if (resultLastStep == Sql.SQLITE_ROW) {
                    throw new ArgumentException("Scalar SQL query did return more than one result");
                }
                else if(resultLastStep != Sql.SQLITE_DONE) {
                    throw new ArgumentException("SQL statement not done");
                }

                return ret;
            });
        }

        private static DateTimeOffset PerformScalarDateTime(string sql) {
            return PerformSql(sql, stmt => {
                if (Sql.sqlite3_step(stmt) != Sql.SQLITE_ROW) {
                    throw new ArgumentException("Scalar SQL query did not return a result");
                }

                string dt = Sql.sqlite3_column_text(stmt, 0);
                DateTimeOffset ret = DateTimeOffset.Parse(dt, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

                int resultLastStep = Sql.sqlite3_step(stmt);
                if (resultLastStep == Sql.SQLITE_ROW) {
                    throw new ArgumentException("Scalar SQL query did return more than one result");
                }
                else if (resultLastStep != Sql.SQLITE_DONE) {
                    throw new ArgumentException("SQL statement not done");
                }

                return ret;
            });
        }

    }

}
