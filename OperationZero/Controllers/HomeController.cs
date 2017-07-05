using OperationZero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationZero.Controllers
{
    public class HomeController : Controller
    {
        Connections connection = new Connections();        

        [HttpGet]
        public ActionResult Index()
        {
            ConnectionObject connectionObject = new ConnectionObject();

            if (Session["ConnectionObject"] != null)
            {
                ConnectionObject tempConnectionObject = new ConnectionObject();
                tempConnectionObject = Session["ConnectionObject"] as ConnectionObject;
                if (tempConnectionObject.isConnected == true && tempConnectionObject.clearSession == false)
                {
                    connectionObject = tempConnectionObject;
                }
                else
                {
                    connectionObject = connection.AddServers(connectionObject);
                    Session["ConnectionObject"] = null;
                }
            
            }
            else
            {
                connectionObject = connection.AddServers(connectionObject);
            }


            return View(connectionObject);
        }

        [HttpPost]
        public ActionResult Index(ConnectionObject connectionObject)
        {
            if (Session["ConnectionObject"] != null )
            {
                ConnectionObject tempConnectionObject = new ConnectionObject();
                tempConnectionObject = Session["ConnectionObject"] as ConnectionObject;
                Session["ConnectionObject"] = null;
             }
            
            connectionObject = connection.AddServers(connectionObject);

            if (connectionObject.ServerSelected != 0)
            {
                //get database list
               connectionObject = connection.getDatabases(connectionObject);

                // prob a more secure way to store this?
               Session["ConnectionObject"] = connectionObject;

            }

            if (connectionObject.DatabaseSelected != null)
            {
                // connect to database
                connectionObject = connection.connectToDatabase(connectionObject);

                // prob a more secure way to store this?
                Session["ConnectionObject"] = connectionObject;
            }

                     
            return View(connectionObject);
        }
        
        [HttpGet]
        public ActionResult CoreMeasures()
        {
            CoreMeasuresObject coreMeasuresObject = new CoreMeasuresObject();
            ConnectionObject connectionObject = new ConnectionObject();
            connectionObject.isConnected = false;

            if (Session["ConnectionObject"] != null)
            {
                connectionObject = Session["ConnectionObject"] as ConnectionObject;
                
            }
            coreMeasuresObject.connectionObject = connectionObject;

            return View(coreMeasuresObject);
        }

        [HttpPost]
        public ActionResult CoreMeasures(CoreMeasuresObject coreMeasuresObject)
        {
            
                List<string> parameterList = new List<string>();
                parameterList.Add(coreMeasuresObject.connectionObject.DatabaseSelectedName);
                parameterList.Add(coreMeasuresObject.parameter2);
                parameterList.Add(coreMeasuresObject.parameter3);
                parameterList.Add(coreMeasuresObject.parameter4);

                coreMeasuresObject.connectionObject = connection.executeStoredProcedure(coreMeasuresObject.connectionObject
                    , "Library_model_ccc.report.CoreMeasuresImplementation"
                    , parameterList
                    , false);
            
            return View(coreMeasuresObject);
        }


      
    }
}
