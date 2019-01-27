using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.ProducerConsumer;
using MySql.Data.MySqlClient;
using SQLite;
using static KorfbalStatistics.CustomExtensions.Attributes;

namespace KorfbalStatistics.RemoteDb
{
   public class BaseRemoteDbManager
    {
        public BaseRemoteDbManager(Consumer consumer, MySqlConnection connection, SQLiteConnection localConnection)
        {
            RemoteConsumer = consumer;
            RemoteConnection = connection;
            LocalConnection = localConnection;

            try
            {
                if (RemoteConnection.State == System.Data.ConnectionState.Open)
                    return;
                RemoteConnection.Open();
                bool test = RemoteConnection.Ping();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected MySqlConnection RemoteConnection;
        protected Consumer RemoteConsumer { get; }
        public SQLiteConnection LocalConnection { get; }

        protected void ExecuteSingleQuery(string query)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = RemoteConnection;
            try
            {
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected T ExecuteSingleDataQuery<T>(MySqlCommand command)
        {
            while (RemoteConnection.State == ConnectionState.Fetching)
            {
                Thread.Sleep(10);
            }
            command.Connection = RemoteConnection;

            MySqlDataReader reader = command.ExecuteReader();

            return ReadData<T>(reader);
        }

        protected T[] ExecuteMultiDataQuery<T>(MySqlCommand command)
        {
            using (var conn = new MySqlConnection("Server=lexpunter.hopto.org; database=StatisticKorfballApp; Uid=root; Pwd=2964Lpsql; Convert Zero Datetime=True"))
            {
                conn.Open();
                command.Connection = conn;

                MySqlDataReader reader = command.ExecuteReader();

                return ReadDataList<T>(reader);
            }
        }
        protected string CreateInsertStatement<T>(T insertClass, string tableName)
        {
            string sql = $"INSERT INTO {tableName}(";
            string values = "Values(";
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "IsSynchronised")
                    continue;
                if (property.GetValue(insertClass) != null)
                {

                    var remoteName = property.GetCustomAttribute<RemoteDbNameAttribute>();
                    sql += (remoteName == null ? property.Name : remoteName.Name) + ", ";

                    bool isBool = property.PropertyType == typeof(bool);
                    var value = property.GetValue(insertClass);
                    if (isBool)
                        values += $"'{((bool)value ? 1 : 0)}', ";
                    else if (property.PropertyType == typeof(Guid))
                    {
                        if ((Guid)value == Guid.Empty)
                            values += "null, ";
                        else
                            values += $"'{property.GetValue(insertClass)}', ";
                    }
                    else
                        values += $"'{property.GetValue(insertClass)}', ";
                }
            }
            sql = sql.Substring(0, sql.Length - 2);
            values = values.Substring(0, values.Length - 2);
            sql += ")";
            values += ")";
            return sql + values;
        }

        protected string CreateInsertStatement<T>(List<T> insertList, string tableName)
        {
            string sql = $"INSERT INTO {tableName}(";
            string values = "Values";
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "IsSynchronised")
                    continue;
                var remoteName = property.GetCustomAttribute<RemoteDbNameAttribute>();
                sql += (remoteName == null ? property.Name : remoteName.Name) + ", ";
            }

            insertList.ForEach(item =>
            {
                values += "(";
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == "IsSynchronised")
                        continue;
                    if (property.GetValue(item) != null)
                    {
                        bool isBool = property.PropertyType == typeof(bool);
                        var value = property.GetValue(item);
                        if (isBool)
                            values += $"'{((bool)value ? 1 : 0)}', ";
                        else if (property.PropertyType == typeof(Guid))
                        {
                            if ((Guid)value == Guid.Empty)
                                values += "null, ";
                            else
                                values += $"'{property.GetValue(item)}', ";
                        }
                        else
                            values += $"'{property.GetValue(item)}', ";
                    }
                    else
                    {
                        values += "null, ";
                    }
                }
                values = values.Substring(0, values.Length - 2);
                values += $"),";
            });

            sql = sql.Substring(0, sql.Length - 2);
            sql += ")";
            values = values.Substring(0, values.Length - 1);
            return sql + values;
        }

        protected T[] ReadDataList<T>(MySqlDataReader reader)
        { 
            List<T> dataList = new List<T>();
            try {

                T item = ReadData<T>(reader, false);
                while (item != null)
                {
                    dataList.Add(item);
                    if (reader.IsClosed)
                        item = default(T);
                    else 
                        item = ReadData<T>(reader, false);
                }
                reader.Close();
            } catch (Exception e)
            {
                reader.Close();
            }
            
            return dataList.ToArray();
        }

        protected T ReadData<T>(MySqlDataReader reader, bool closeReader = true)
        {
            try
            {
                if (reader.Read())
                {
                    T newClass = (T)Activator.CreateInstance(typeof(T));
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        var remoteName = property.GetCustomAttribute<RemoteDbNameAttribute>();
                        string colName = remoteName == null ? property.Name : remoteName.Name;
                        if (property.Name == "IsSynchronised")
                        {
                            property.SetValue(newClass, true);
                            continue;
                        }
                        if (property.PropertyType == typeof(Guid))
                        {
                            string guidString = reader.GetValue(reader.GetOrdinal(colName)) as string;
                            if (guidString == null)
                                continue;
                            property.SetValue(newClass, Guid.Parse(guidString));

                        }
                        else if (property.PropertyType == typeof(Guid?))
                        {
                            string guidString = reader.GetValue(reader.GetOrdinal(colName)) as string;
                            if (guidString == null)
                                continue;
                            property.SetValue(newClass, Guid.Parse(guidString));
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            int boolInt = reader.GetSByte(colName);
                            property.SetValue(newClass, boolInt == 1 ? true : false);
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            DateTime date = reader.GetDateTime(colName);
                            property.SetValue(newClass, date);
                        }
                        else
                        {
                            var value = reader.GetValue(reader.GetOrdinal(colName));
                            if (Convert.IsDBNull(value))
                                continue;

                            property.SetValue(newClass, value);
                        }
                    }
                    return newClass;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                closeReader = true;
                throw new Exception(e.Message);
            }
            finally
            {
                if (closeReader)
                    reader.Close();
            }
            return default(T);
        }
    }
}