#region Namespaces
using System;
using System.Data;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;
using System.Net;
using System.Data.SqlClient;
#endregion

namespace SAP2RRHH
{

	[Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute]
	public partial class ScriptMain : Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase
	{
		public void Main()
		{
            string EndpointUser = (string)Dts.Variables["$Project::EndpointUser"].Value;
            string EndopintPass = (string)Dts.Variables["$Project::EndopintPass"].Value;
            string sqlConnectionString = (string)Dts.Variables["$Project::SqlDestinationString"].Value;

            string targetTable = (string)Dts.Variables["User::TargetTable"].Value;
            string entity = (string)Dts.Variables["User::Entity"].Value;
            string executionDate = (string)Dts.Variables["User::executionDate"].Value;
            bool modeDelta = (bool)Dts.Variables["User::modeDelta"].Value;

            string webServiceUrl = "https://your-endpoint-provided-link";

            bool skip = true;
            try
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webServiceUrl);
                string encodedCredentials = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(EndpointUser + ":" + EndopintPass));
                
                request.Headers.Add("Authorization", "Basic " + encodedCredentials);
                request.Headers.Add("entity", entity);
                request.Headers.Add("modeDelta", modeDelta.ToString());
                if (modeDelta) { request.Headers.Add("executionDate", executionDate); }
                
                request.Method = "POST";

                Dts.Events.FireInformation(0, "WebService " + DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss\.fff")
                    , "Executing POST...", string.Empty, 0, ref skip);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (IDataReader reader = CreateModelReader(entity, responseStream)) //Function is defined below
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        Dts.Events.FireInformation(0, "Bulk " + DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss\.fff")
                            , "Executing...", string.Empty, 0, ref skip);
                        bulkCopy.DestinationTableName = targetTable;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.BatchSize = (int)(2097152 / reader.FieldCount);
                        bulkCopy.NotifyAfter = 250000;
                        bulkCopy.SqlRowsCopied += (sender, e) =>
                        {
                            Dts.Events.FireInformation(0, "Blulk " + DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss\.fff")
                                , $"{e.RowsCopied} rows copied.", string.Empty, 0, ref skip);
                        };
                        bulkCopy.WriteToServer(reader);
                    }
                }

                Dts.TaskResult = (int)ScriptResults.Success;
            }
            catch (WebException webEx)
            {
                using (WebResponse resp = webEx.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)resp;
                    using (Stream data = httpResponse.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Dts.Events.FireError(0, "HTTP Error", $"Status code: {httpResponse.StatusCode}, Response: {text}", String.Empty, 0);
                    }
                }
                Dts.TaskResult = (int)ScriptResults.Failure;
            }
            catch (Exception ex)
            {
                Dts.Events.FireError(0, "Message", ex.Message, String.Empty, 0);
                Dts.Events.FireError(0, "Source", ex.Source, String.Empty, 0);
                Dts.Events.FireError(0, "StackTrace", ex.StackTrace, String.Empty, 0);
                Dts.TaskResult = (int)ScriptResults.Failure;
            }

        }
        
        public IDataReader CreateModelReader(string entity, Stream responseStream)
        {
            IDataReader reader = null;
            switch (entity)
            {//Generic examples, reduced to 5.
                case "contracts":
                    reader = new contractsReader(responseStream);
                    break;
                case "employees":
                    reader = new employeeReader(responseStream);
                    break;
                case "departments":
                    reader = new departmentsReader(responseStream);
                    break;
                case "locations":
                    reader = new locationsReader(responseStream);
                    break;
                case "positions":
                    reader = new positionsReader(responseStream);
                    break;
                default:
                    Dts.TaskResult = (int)ScriptResults.Failure;
                    Dts.Events.FireError(0, "Entity", "Header value not contemplated in code.", String.Empty, 0);
                    break;  
            }
            return reader;
        }
        enum ScriptResults
        {
            Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success,
            Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
        };
    }
}