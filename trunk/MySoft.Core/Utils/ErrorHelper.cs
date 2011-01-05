using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace MySoft.Core
{
    /// <summary>
    /// 错误处理
    /// </summary>
    public class ErrorHelper
    {
        public static string GetErrorWithoutHtml(Exception ex)
        {
            string error = "System Error\r\n";

            // Populate Error Information Collection
            NameValueCollection error_info = new NameValueCollection();
            error_info.Add("Message：", ex.Message);
            error_info.Add("Source：", ex.Source);
            error_info.Add("TargetSite：", ex.TargetSite == null ? null : ex.TargetSite.ToString());
            error_info.Add("StackTrace：", ex.StackTrace);
            error += CollectionToString(error_info);

            return error;
        }

        /// <summary>Returns HTML an formatted error message.</summary>
        public static string GetHtmlError(Exception ex)
        {
            // Heading Template
            const string heading = "<TABLE BORDER=\"0\" WIDTH=\"100%\" CELLPADDING=\"1\" CELLSPACING=\"0\"><TR><TD bgcolor=\"black\" COLSPAN=\"2\"><FONT face=\"Arial\" color=\"white\"><B>&nbsp;<!--HEADER--></B></FONT></TD></TR></TABLE>";

            // Error Message Header
            string html = "<FONT face=\"Arial\" size=\"5\" color=\"red\">Error - " + ex.Message + "</FONT><BR><BR>";

            // Populate Error Information Collection
            NameValueCollection error_info = new NameValueCollection();
            error_info.Add("Message", CleanHTML(ex.Message));
            error_info.Add("Source", CleanHTML(ex.Source));
            error_info.Add("TargetSite", CleanHTML(ex.TargetSite == null ? null : ex.TargetSite.ToString()));
            error_info.Add("StackTrace", CleanHTML(ex.StackTrace));

            // Error Information
            html += heading.Replace("<!--HEADER-->", "Error Information");
            html += CollectionToHtmlTable(error_info);

            if (ex.InnerException != null) html += GetHtmlError(ex.InnerException);

            return html;
        }

        private static string CollectionToHtmlTable(NameValueCollection collection)
        {
            // <TD>...</TD> Template
            const string TD = "<TD><FONT face=\"Arial\" size=\"2\"><!--VALUE--></FONT></TD>";

            // Table Header
            string html = "\n<TABLE width=\"100%\">\n"
                + "  <TR bgcolor=\"#C0C0C0\">" + TD.Replace("<!--VALUE-->", "&nbsp;<B>Name</B>")
                + "  " + TD.Replace("<!--VALUE-->", "&nbsp;<B>Value</B>") + "</TR>\n";

            // No Body? -> N/A
            if (collection != null)
            {
                if (collection.Count == 0)
                {
                    collection = new NameValueCollection();
                    collection.Add("N/A", "");
                }

                // Table Body
                for (int i = 0; i < collection.Count; i++)
                {
                    html += "<TR valign=\"top\" bgcolor=\"" + ((i % 2 == 0) ? "white" : "#EEEEEE") + "\">"
                        + TD.Replace("<!--VALUE-->", collection.Keys[i]) + "\n"
                        + TD.Replace("<!--VALUE-->", collection[i]) + "</TR>\n";
                }
            }
            else
            {
                collection = new NameValueCollection();
                collection.Add("N/A", "");
            }

            // Table Footer
            return html + "</TABLE>";
        }

        private static string CollectionToString(NameValueCollection collection)
        {
            string text = "";

            if (collection != null)
            {
                if (collection.Count == 0)
                {
                    collection = new NameValueCollection();
                    collection.Add("N/A", "");
                }

                for (int i = 0; i < collection.Count; i++)
                {
                    text += collection.Keys[i];
                    text += collection[i] + "\r\n";
                }
            }
            else
            {
                collection = new NameValueCollection();
                collection.Add("N/A", "");
            }

            return text;
        }

        private static string CleanHTML(string html)
        {
            if (html == null) return html;
            // Cleans the string for HTML friendly display
            return (html.Length == 0) ? "" : html.Replace("<", "&lt;").Replace("\r\n", "<BR>").Replace("&", "&amp;").Replace(" ", "&nbsp;");
        }
    }
}
