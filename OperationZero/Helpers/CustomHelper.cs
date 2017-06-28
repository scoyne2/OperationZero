using OperationZero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;

namespace OperationZero.Helpers
{
    public class CustomHelper
    {
        public static HtmlString AlertBox(string id, string text, string alertClass, string icon)
        {
            string alertBox = String.Format(
                "<div class='alert alert-{2}' role='alert' id='{0}'>" +
                "<span class='glyphicon glyphicon-{3}' aria-hidden='true'></span> {1} </div>"
                , id, text, alertClass, icon);

            return new HtmlString(alertBox);
        }
        
        public static HtmlString AlertBoxLink( string id, string href, string text, string alertClass, string icon)
        {
            string alertBox = String.Format(
                "<div class='alert alert-{3}' role='alert' id='{0}'>" +
                "<span class='glyphicon glyphicon-{4}' aria-hidden='true'></span>  "+
                "<a class='alert-link' href='{1}' style='color:white;'> {2} </a></div>  "
                ,id, href, text,alertClass, icon);

            return new HtmlString(alertBox);
        }

        public static HtmlString FileStatus(decimal acceptablePercent, int issuePresentInFileCount, int mandatoryFieldMissingCount, decimal cautionaryPercent, decimal criticalPercent, int dataPresentInFileCount)
        {
           
            string  icon = null;
            string  iconClass = null;
            string  title = null;

              if (acceptablePercent == 100 && issuePresentInFileCount == 0 && mandatoryFieldMissingCount == 0 )
                {
                    icon = "ok-sign" ;
                    iconClass  = "text-success";
                    title = "File has no issues" ;                                           
                }
              if (issuePresentInFileCount >= 1 && (criticalPercent < 1 || mandatoryFieldMissingCount < 1 && dataPresentInFileCount < 1))
                {
                    icon = "minus-sign" ;
                    iconClass  = "text-warning";
                    title = "File has some issues" ;  
                }
              if (criticalPercent >= 1 || mandatoryFieldMissingCount >= 1 && dataPresentInFileCount >= 1)
                {
                    icon = "remove-sign" ;
                    iconClass  = "text-danger";
                    title = "File has critical issues" ;  
                }
              if (acceptablePercent == 0 && cautionaryPercent == 0 && criticalPercent == 0)
                {
                    icon = "question-sign" ;
                    iconClass = "text-muted";
                    title = "File not sent" ;                 
                }

              string result = String.Format("<i class='glyphicon glyphicon-{0} {1}' style='font-size: 20px;' aria-hidden='true' title='{2}'></i>", icon, iconClass, title);
            
            return new HtmlString(result);
        }

        public static HtmlString ProgressBar(decimal acceptablePercent, decimal cautionaryPercent, decimal criticalPercent)
        {
              string result = String.Format(
                            "<div class='progress' style='background: #b4bcc2; margin-bottom: .5%;margin-top: .5%; height:18px'>" +
                            "<div class='progress-bar progress-bar-success'  style ='width: {0}%;'  title='{0}% Acceptable'>"+
                            "<span class='sr-only'></span>"+
                            "</div>"+
                            "<div class='progress-bar progress-bar-warning' style ='width: {1}%;'title='{1}% Cautionary'>" +
                            "<span class='sr-only'></span>"+
                            "</div>"+
                            "<div class='progress-bar progress-bar-danger' style ='width: {2}%;'title='{2}% Critical'>" +
                            "<span class='sr-only'></span>" +  
                            "</div>",    acceptablePercent, cautionaryPercent, criticalPercent);

            return new HtmlString(result);
        }
           


     

    }
}