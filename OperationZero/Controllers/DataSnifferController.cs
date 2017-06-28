using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationZero.Models;
using System.Data;
using System.Data.SqlClient;

namespace OperationZero.Controllers
{
    public class DataSnifferController : Controller
    {
        Connections connection = new Connections();

        [HttpGet]
        public ActionResult DataSniffer()
        {
            DataSnifferObject dataSnifferObject = new DataSnifferObject();
            ConnectionObject connectionObject = new ConnectionObject();
            connectionObject.isConnected = false;

            if (Session["ConnectionObject"] != null)
            {
                connectionObject = Session["ConnectionObject"] as ConnectionObject;

            }
            dataSnifferObject.connectionObject = connectionObject;

            return View(dataSnifferObject);
        }

        [HttpPost]
        public ActionResult DataSniffer(DataSnifferObject dataSnifferObject)
        {

            List<string> parameterList = new List<string>();
            parameterList.Add(dataSnifferObject.connectionObject.DatabaseSelectedName);
            parameterList.Add(dataSnifferObject.parameter2.ToString());

            if (dataSnifferObject.runProcedure == true)
            {
                dataSnifferObject.connectionObject = connection.executeStoredProcedure(dataSnifferObject.connectionObject
                    , "Library_model_ccc.dbo.DataSniffer_2"
                    , parameterList
                    , false);
            }

            string connectionString = connection.GetConnectionString(dataSnifferObject.connectionObject);
                string sql =
                " SELECT DISTINCT TableName, TableCriticality," +
                " SUM(CASE WHEN DataElementCriticality = 'Mandatory' and DataPresentInFile = 0 AND COALESCE(Override,0) <> 1 THEN 1 ELSE 0 END) as MandatoryFieldMissingCount, " +
                " SUM(CASE WHEN COALESCE(ResolvedInPostLoad,0) <> 1 THEN CAST(DataPresentInFile as int) ELSE 0 END ) as DataPresentInFileCount, " +
                " SUM(CASE WHEN IssuePresent = 1 AND DataPresentInFile > 0 AND COALESCE(ResolvedInPostLoad,0) <> 1 THEN 1 ELSE 0 END) as IssuePresentInFileCount," +
                " COUNT(*) as TotalFieldCount" +
                " FROM  (SELECT DISTINCT TableName, TableCriticality, DataElementCriticality, DataPresentInFile, DataElement, IssuePresent , Override " +
                " FROM reference.DataSniffer WHERE IsActive = 1 AND COALESCE(ResolvedInPostLoad,0) <> 1  ) x GROUP BY TableName, TableCriticality ";

                using (SqlConnection conn2 = new SqlConnection(connectionString))
                {
                    SqlCommand cmd2 = new SqlCommand(sql, conn2);
                    conn2.Open();
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    List<DataSnifferParent> model2 = new List<DataSnifferParent>();
                    while (rdr2.Read())
                    {
                        var record2 = new DataSnifferParent();
                        record2.TableName = rdr2["TableName"].ToString();
                        record2.TableCriticality = rdr2["TableCriticality"].ToString();
                        record2.MandatoryFieldMissingCount = Convert.ToInt32(rdr2["MandatoryFieldMissingCount"]);
                        record2.DataPresentInFileCount = Convert.ToInt32(rdr2["DataPresentInFileCount"]);
                        record2.IssuePresentInFileCount = Convert.ToInt32(rdr2["IssuePresentInFileCount"]);
                        record2.TotalFieldCount = Convert.ToInt32(rdr2["TotalFieldCount"]);

                        if (record2.DataPresentInFileCount > 0)
                        {
                            record2.CriticalPercent = Decimal.Round(100 * Decimal.Divide(record2.MandatoryFieldMissingCount, (record2.TotalFieldCount)));
                            record2.CautionaryPercent = Decimal.Round(100 * Decimal.Divide(record2.IssuePresentInFileCount,  (record2.TotalFieldCount)));
                            record2.AcceptablePercent = Decimal.Round((100 - record2.CautionaryPercent - record2.CriticalPercent));
                        }
                        else 
                        {
                            record2.CriticalPercent = 0;
                            record2.CautionaryPercent = 0;
                            record2.AcceptablePercent = 0;
                        }

                        model2.Add(record2);
                        
                    }
                    conn2.Close();
                    dataSnifferObject.dataSnifferParents = model2;
                }


                return View(dataSnifferObject);
            }

        [HttpPost]
        public ActionResult DataSnifferChildren(string serverSelectedName, string databaseSelectedName, string tableName, string alertType)
        {
            ConnectionObject connectionObject = new ConnectionObject();
            connectionObject.ServerSelectedName = serverSelectedName;
            connectionObject.DatabaseSelectedName = databaseSelectedName;

            string connectionString = connection.GetConnectionString(connectionObject);
            var model = new List<DataSnifferRow>();
            String sql = "SELECT DISTINCT coalesce(Override,'0'), TableName, TableCriticality, TableCriticalityWeight, DataElement, DataElementDescription, DataElementCriticality," +
             "DataElementIsNullable, DataElementDataType, DataElementMaxLength, DataElementOrdinalPosition, DataElementMemberColumnName, DataPresentInFile, IssuePresent," +
             "RowsAffected, TotalRows, PercentAffected, IssueGroup, IssueDescription, QueryforDetails, MemberCommunication, ResolvedInPostLoad, PotentialSolution, EffectOfMissingDataElement," +
             "Comments, Status, IsActive, ColumnDefault  FROM  reference.DataSniffer WHERE IsActive = 1 "+
             "AND TableName = '"+tableName+"'";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    var record = new DataSnifferRow();
                    record.hiddenOverride = Convert.ToBoolean(Convert.ToInt32(rdr["Override"]));
                    record.TableName = rdr["TableName"].ToString();
                    record.TableCriticality = rdr["TableCriticality"].ToString();
                    record.TableCriticalityWeight = rdr["TableCriticalityWeight"].ToString();
                    record.DataElement = rdr["DataElement"].ToString();
                    record.DataElementDescription = rdr["DataElementDescription"].ToString();
                    record.DataElementCriticality = rdr["DataElementCriticality"].ToString();
                    record.DataElementIsNullable = rdr["DataElementIsNullable"].ToString();
                    record.DataElementDataType = rdr["DataElementDataType"].ToString();
                    record.DataElementMaxLength = rdr["DataElementMaxLength"].ToString();
                    record.DataElementOrdinalPosition = rdr["DataElementOrdinalPosition"].ToString();
                    record.DataElementMemberColumnName = rdr["DataElementMemberColumnName"].ToString();
                    record.DataPresentInFile = rdr["DataPresentInFile"].ToString();
                    record.IssuePresent = rdr["IssuePresent"].ToString();
                    record.RowsAffected = rdr["RowsAffected"].ToString();
                    record.TotalRows = rdr["TotalRows"].ToString();
                    record.PercentAffected = rdr["PercentAffected"].ToString();
                    record.IssueGroup = rdr["IssueGroup"].ToString();
                    record.IssueDescription = rdr["IssueDescription"].ToString();
                    record.QueryforDetails = rdr["QueryforDetails"].ToString();
                    record.MemberCommunication = rdr["MemberCommunication"].ToString();
                    record.ResolvedInPostLoad = rdr["ResolvedInPostLoad"].ToString();
                    record.PotentialSolution = rdr["PotentialSolution"].ToString();
                    record.EffectOfMissingDataElement = rdr["EffectOfMissingDataElement"].ToString();
                    record.Comments = rdr["Comments"].ToString();
                    record.IsActive = rdr["IsActive"].ToString();
                    record.ColumnDefault = rdr["ColumnDefault"].ToString();

                    //build css class
                    if (record.DataElementCriticality == "Mandatory" && record.DataPresentInFile == "0" && record.hiddenOverride == false)
                    {
                        record.CSSClass = "danger";
                        record.CSSIcon = "glyphicon-remove-sign";
                        record.CSSOrder = 1;
                        record.IssueGroup = "Field Missing";
                        record.IssueDescription = record.DataElement + " is mandatory but missing";
                        record.PercentAffected = "100.00%";
                        record.RowsAffected = record.TotalRows;
                    }
                    else if (record.IssuePresent == "True" && record.DataPresentInFile == "1" && record.hiddenOverride == false)
                    {
                        record.CSSClass = "warning";
                        record.CSSIcon = "glyphicon-minus-sign";
                        record.CSSOrder = 2;
                    }

                    else if (record.DataPresentInFile == "1")
                    {
                        record.CSSClass = "success";
                        record.CSSIcon = "glyphicon-ok-sign";
                        record.CSSOrder = 3;
                    }
                   else if (record.hiddenOverride == true)
                    {
                        record.CSSClass = "muted";
                        record.CSSIcon = "glyphicon-minus-sign";
                        record.CSSOrder = 4;
                    }
                    else
                    {
                        record.CSSClass = "muted";
                        record.CSSIcon = "glyphicon-question-sign";
                        record.CSSOrder = 5;
                    }

                    model.Add(record);

                }
                conn.Close();               
            }
            // filter list for only alert type
            if (alertType != null)
            {
                model = model.Where(x => x.CSSClass == alertType).ToList();
            }
                     

            model = model.OrderBy(x => x.CSSOrder).ThenBy(x => x.DataElement).ToList();

            return PartialView("DataSnifferChildren", model);
        }
      
    }
}

