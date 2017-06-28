using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient; 
using System.Data;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OperationZero.Models
{
    
    public class Server
    {
        public int ServerId { get; set; }
        public string ServerAddress { get; set; }
    }

    public class Database
    {
        public string DatabaseId { get; set; }
        public string DatabaseName { get; set; }
    }

    public class ConnectionObject
    {
        public int ServerSelected { get; set; }
        public string ServerSelectedName { get; set; }
        public IEnumerable<Server> Servers { get; set; }
        public string DatabaseSelected { get; set; }
        public string DatabaseSelectedName { get; set; }
        public IEnumerable<Database> Databases { get; set; }
        public string error { get; set; }
        public bool isConnected { get; set; }
        public bool clearSession { get; set; }
        public SqlConnection sqlConnection { get; set; }
        public DataTable dataTable { get; set; }
    }
    
    public class CoreMeasuresObject 
    {
        public ConnectionObject connectionObject { get; set; }
       
        [Required(ErrorMessage = "Database Name is Required")]
        [DisplayName("Database")]
        public string parameter1 { get; set; }

        [DisplayName("To Email")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string parameter2 { get; set; }

        [DisplayName("Load Instance (Optional)")]
        public string parameter3 { get; set; }

        [DisplayName("Exclusion List (Optional)")]
        public string parameter4 { get; set; }        
    }
    
    public class Connections
    {
        string connection;
        static string userName = "XXXXXX";
        static string password = "XXXXXX";         
       
        public ConnectionObject AddServers(ConnectionObject connectionObject)
        {
            var servers = new List<Server>
                        {
                            new Server { ServerId = 1, ServerAddress = "192.168.17.xx" },
                            new Server { ServerId = 2, ServerAddress = "192.168.17.xx" },
                            new Server { ServerId = 3, ServerAddress = "192.168.17.xx" },
                            new Server { ServerId = 4, ServerAddress = "192.168.17.xx" }
                        };

            var localConnectionObject = new ConnectionObject
           {
               Servers = servers.Select(x => new Server
               {
                   ServerId = x.ServerId,
                   ServerAddress = x.ServerAddress
               })
           };
            connectionObject.Servers = localConnectionObject.Servers;
            return connectionObject;
        }
      
        public string GetConnectionString(ConnectionObject currentConnection)
        {
            string connectionString = null;

            if (currentConnection.DatabaseSelectedName != null)
            {
                connectionString = "Data Source= " + currentConnection.ServerSelectedName + "; Database=" + currentConnection.DatabaseSelectedName + "; User ID=" + userName + ";Password=" + password + ";";
            }
            return connectionString;
        }    

        public ConnectionObject getDatabases(ConnectionObject currentConnection)
        {

            Server server = (from Server s in currentConnection.Servers
                             where s.ServerId == currentConnection.ServerSelected
                             select s).Single();
            connection = "Data Source= " + server.ServerAddress + " ; User ID=" + userName + ";Password=" + password + ";";
            currentConnection.ServerSelectedName = server.ServerAddress;

            using (var con = new SqlConnection(connection))
            {


                var databasesList = new List<Database>();

                try
                {
                    con.Open();
                    DataTable databases = con.GetSchema("Databases");
                    
                    for (int i = 0; i <= databases.Rows.Count-1; i++)
                    {
                        string databaseName = databases.Rows[i].Field<String>("database_name");

                        databasesList.Add(new Database { DatabaseId = i.ToString(), DatabaseName = databaseName });
                    }

                    //now sort the list for a friendly UX
                    databasesList = databasesList.OrderBy(x => x.DatabaseName).ToList();

                    var connectionObject = new ConnectionObject
                    {
                        Databases = databasesList.Select(x => new Database
                        {
                            DatabaseId = x.DatabaseId,
                            DatabaseName = x.DatabaseName
                        })
                    };
                    
                    currentConnection.Databases = connectionObject.Databases;

                }
                catch (Exception ex)
                {
                    // do something with this later
                    string exception = ex.Message;
                   
                }

                return currentConnection;
            }
        }

        public ConnectionObject connectToDatabase(ConnectionObject currentConnection)
        {
            if(currentConnection.DatabaseSelected != null)
            {

                Database database = (from Database d in currentConnection.Databases
                                     where d.DatabaseId == currentConnection.DatabaseSelected
                                     select d).Single();

                currentConnection.DatabaseSelectedName = database.DatabaseName;
                connection = "Data Source= " + currentConnection.ServerSelectedName + "; Database=" + currentConnection.DatabaseSelectedName + "; User ID=" + userName + ";Password=" + password + ";";
                SqlConnection databaseConnection = new SqlConnection(connection);


                using (databaseConnection)
                {

                    try
                    {
                        databaseConnection.Open();
                        databaseConnection.Close();
                        currentConnection.isConnected = true;

                    }
                    catch (Exception ex)
                    {
                        currentConnection.isConnected = false;
                        currentConnection.error = ex.Message;

                    }
                }
            }
            else
            {
                currentConnection.error = "No Database selected";
                currentConnection.isConnected = false;
            }

                return currentConnection;
            
        }

        public ConnectionObject executeStoredProcedure(ConnectionObject currentConnection, string fullProcedureName, List<string> parameterList, bool hasResult )
        {
           
            try
                {

                    connection = "Data Source= " + currentConnection.ServerSelectedName + "; Database=" + currentConnection.DatabaseSelectedName + "; User ID=" + userName + ";Password=" + password + ";";
                    SqlConnection databaseConnection = new SqlConnection(connection);
                    
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = fullProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = databaseConnection;
                    cmd.CommandTimeout = 20000;

                    databaseConnection.Open();

                    SqlCommandBuilder.DeriveParameters(cmd);
             
                    if (parameterList.Count() > 1)
                    {
                        for (var i = 0; i < parameterList.Count; i++)
                        {
                            cmd.Parameters[i + 1].Value = parameterList[i];
                        }
                    }
                    if (hasResult == true)
                    {
                        SqlDataReader result = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(result);
                        currentConnection.dataTable = dt;
                    }
                    else
                    {
                        int result = cmd.ExecuteNonQuery();
                    }

                    

                    databaseConnection.Close();
                    currentConnection.isConnected = true;
                    

                }
                catch (Exception ex)
                {
                    //currentConnection.isConnected = false;
                    currentConnection.error = ex.Message;

                }




                return currentConnection;
            }
        



    }


}
