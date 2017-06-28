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
   
        public class DataSnifferObject
        {
            public List<DataSnifferRow> dataSnifferChildren { get; set; }
            public List<DataSnifferParent> dataSnifferParents { get; set; }  
            public ConnectionObject connectionObject { get; set; }
            [Required(ErrorMessage = "Database Name is Required")]
            [DisplayName("Database")]
            public string parameter1 { get; set; }

            [DisplayName("WeRunAPRDRG")]
            public bool parameter2 { get; set; }
            public bool runProcedure { get; set; }       
         
        }

        public class DataSnifferParent
        {
            public string TableName { get; set; }
            public string TableCriticality { get; set; }
            public int MandatoryFieldMissingCount { get; set; }
            public int DataPresentInFileCount { get; set; }
            public int IssuePresentInFileCount { get; set; }
            public int TotalFieldCount { get; set; }
            public decimal AcceptablePercent { get; set; }
            public decimal CriticalPercent { get; set; }
            public decimal CautionaryPercent { get; set; }  

        }

       
        public class DataSnifferRow 
        {
            public string RecordKey { get; set; }
            public string TableName { get; set; }
            public string TableCriticality { get; set; }
            public string TableCriticalityWeight { get; set; }
            public string DataElement { get; set; }
            public string DataElementDescription { get; set; }
            public string DataElementCriticality { get; set; }
            public string DataElementIsNullable { get; set; }
            public string DataElementDataType { get; set; }
            public string DataElementMaxLength { get; set; }
            public string DataElementOrdinalPosition { get; set; }
            public string DataElementMemberColumnName { get; set; }
            public string DataPresentInFile { get; set; }
            public string IssuePresent { get; set; }
            public string RowsAffected { get; set; }
            public string TotalRows { get; set; }
            public string PercentAffected { get; set; }
            public string IssueGroup { get; set; }
            public string IssueDescription { get; set; }
            public string QueryforDetails { get; set; }
            public string MemberCommunication { get; set; }
            public string ResolvedInPostLoad { get; set; }
            public string PotentialSolution { get; set; }
            public string EffectOfMissingDataElement { get; set; }
            public string Comments { get; set; }
            public string Status { get; set; }
            public string IsActive { get; set; }
            public string ColumnDefault { get; set; }
            public string CSSClass { get; set; }
            public int CSSOrder { get; set; }
            public string CSSIcon { get; set; }
            public bool hiddenOverride { get; set; }
      
        }





   

     


}