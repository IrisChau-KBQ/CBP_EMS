using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SharePoint.Client;

//Here is the once-per-application setup information
[assembly: log4net.Config.XmlConfigurator(Watch = true)]



namespace DataCleanup
{
    class DataCleanUps
    {

        static void Main(string[] args)
        {


            log4net.Config.BasicConfigurator.Configure();
            log4net.ILog log1 = log4net.LogManager.GetLogger(typeof(DataCleanUps));


            string server = ConfigurationSettings.AppSettings["server"];
            string database = ConfigurationSettings.AppSettings["database"];
            string username = ConfigurationSettings.AppSettings["username"];
            string password = ConfigurationSettings.AppSettings["password"];

            string spsite = ConfigurationSettings.AppSettings["site"];

            System.Data.SqlClient.SqlConnection sqlcon;

            string pgmintakesql = ""; // for TB_PROGRAMME_INTAKE
            string ccmfsql = ""; // for CCMF Table
            string cpipsql = ""; // for CPIP table


            // Application Variables
            // string HouseKeepMonth = ReadUserNumericinput();


            //Retive House Keep Month from Config settings
            string HouseKeepMonth = ConfigurationSettings.AppSettings["HouseKeepMonth"];

            log.Info("Program Execution Started");

            // Get Operation Type from console.
            //string operation = ReadUserOptions();

            // Get Operation Type from  Config Settings.
            string operation = ConfigurationSettings.AppSettings["OperationType"];
            string operationtype = "Unknown";

            if (operation == "1") { operationtype = "Retrive Data"; }
            else if (operation == "2")
            { operationtype = "Delete Data"; };

            log.Info("Selected Operation Type (" + operation + ") : " + operationtype);

            //setup connection string for the databse
            string connetionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + "; Password=" + password;
            sqlcon = new System.Data.SqlClient.SqlConnection(connetionString);




            // Retrive how many months 		
            //configsql = "select distinct top 1 value, Modified_date   from TB_SYSTEM_PARAMETER where config_code='HouseKeepMonth'  order by Modified_date desc";
            //DataTable dt_system_parameter = new DataTable();

            try
            {
                //Console.WriteLine("Retriving values from TB_SYSTEM_PARAMETER ");
                // dt_system_parameter = GetData(configsql, sqlcon);

                //if (dt_system_parameter.Rows.Count > 0)
                // {

                //string sHouseKeepMonth = dt_system_parameter.Rows[0]["value"].ToString();

                // string sHouseKeepMonth = HouseKeepMonth;


                //Console.WriteLine("House Keep Month value retrived as : " + sHouseKeepMonth);

                //if (StoreUserInput(HouseKeepMonth, sqlcon))
                {

                    ccmfsql = "select CCMF_ID as APPLICATION_ID, APPLICATION_NUMBER,Programme_ID from TB_CCMF_APPLICATION where Programme_ID in (select Programme_ID from TB_PROGRAMME_INTAKE where datediff(m, Application_Deadline, getdate()) > " + HouseKeepMonth + " and status = 'Completed')  and status <> 'Awarded'";

                    cpipsql = " select Incubation_ID as APPLICATION_ID, APPLICATION_NUMBER,Programme_ID from TB_INCUBATION_APPLICATION where Programme_ID in " +
                        " (select Programme_ID from TB_PROGRAMME_INTAKE where datediff(m, Application_Deadline , getdate()) >" + HouseKeepMonth + " and status = 'Completed') and status <> 'Awarded'";

                    pgmintakesql = ccmfsql + " Union ALL " + cpipsql;


                    DataTable dt_Applications = new DataTable();
                    try
                    {

                        //log.Info(pgmintakesql);
                        dt_Applications = GetData(pgmintakesql, sqlcon);
                        if (dt_Applications.Rows.Count > 0)
                        {

                            //Deletion of relational data starts here

                            foreach (DataRow dr in dt_Applications.Rows)
                            {

                                if (operation == "2")
                                {
                                    // Delete Relational data in child tables first
                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_FUNDING_STATUS_HISTORY", sqlcon);
                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_FUNDING_STATUS", sqlcon);

                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_COMPANY_CORE_MEMBER_HISTORY", sqlcon);
                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_COMPANY_CORE_MEMBER", sqlcon);


                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_CONTACT_DETAIL_HISTORY", sqlcon);
                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_CONTACT_DETAIL", sqlcon);

                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_ATTACHMENT_HISTORY", sqlcon);
                                    DeleteRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_ATTACHMENT", sqlcon);

                                    //Delete Sharepoint Related data
                                    DeleteSPRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "Application_List", spsite);
                                    DeleteSPRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "ApplicationAttachments", spsite);

                                    //Delete ALL Additional Relational Data HERE
                                    DeleteAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_COLLABORATOR", sqlcon);
                                    DeleteAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_SHORTLISTING", sqlcon);
                                    DeleteAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_SCREENING_CCMF_SCORE", sqlcon);
                                    DeleteAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_SCREENING_INCUBATION_SCORE", sqlcon);


                                    //Delete  data of Intakes here
                                    DeleteData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "Incubation_ID", "TB_INCUBATION_APPLICATION_HISTORY", sqlcon);
                                    DeleteData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "Incubation_ID", "TB_INCUBATION_APPLICATION", sqlcon);

                                    DeleteData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "CCMF_ID", "TB_CCMF_APPLICATION_HISTORY", sqlcon);
                                    DeleteData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "CCMF_ID", "TB_CCMF_APPLICATION", sqlcon);
                                }
                                else if (operation == "1")
                                {
                                    //Display Data
                                    SelectData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "CCMF_ID", "TB_CCMF_APPLICATION", sqlcon);
                                    SelectData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "CCMF_ID", "TB_CCMF_APPLICATION_HISTORY", sqlcon);

                                    SelectData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "Incubation_ID", "TB_INCUBATION_APPLICATION", sqlcon);
                                    SelectData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "Incubation_ID", "TB_INCUBATION_APPLICATION_HISTORY", sqlcon);


                                    //Display  Relational Data
                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_FUNDING_STATUS_HISTORY", sqlcon);
                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_FUNDING_STATUS", sqlcon);

                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_COMPANY_CORE_MEMBER_HISTORY", sqlcon);
                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_COMPANY_CORE_MEMBER", sqlcon);


                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_CONTACT_DETAIL_HISTORY", sqlcon);
                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_CONTACT_DETAIL", sqlcon);

                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_ATTACHMENT_HISTORY", sqlcon);
                                    SelectRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_ATTACHMENT", sqlcon);

                                    //Display Sharepoint Related data
                                    //SelectSPRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "Application_List", spsite);
                                    //SelectSPRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "ApplicationAttachments", spsite);

                                    //Display Additional Relational Data
                                    SelectAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_COLLABORATOR", sqlcon);
                                    SelectAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_APPLICATION_SHORTLISTING", sqlcon);
                                    SelectAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_SCREENING_CCMF_SCORE", sqlcon);
                                    SelectAdditionalRelationalData(dr["APPLICATION_ID"].ToString(), dr["APPLICATION_NUMBER"].ToString(), dr["Programme_ID"].ToString(), "TB_SCREENING_INCUBATION_SCORE", sqlcon);


                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Error Info While fetching Applications : " + ex);
                        log.Error("Error: While fetching Applications : " + ex);
                    }

                    //Console.WriteLine("****** Program Complete, Have a good day******");
                    log.Info("Program Execution Ends");
                    Console.WriteLine("<<<<< Press Any Key to Exit >>>>>");

                    Console.ReadKey();
                    // }

                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error Info While fetching Intakes : " + ex);
                log.Error("Error: While fetching Intakes : " + ex);
            }


        }


        private static DataTable GetData(string sql, System.Data.SqlClient.SqlConnection sqlconn)
        {


            DataTable dttable = new DataTable();

            //Console.WriteLine("connection created successfuly");

            using (var cmd = new SqlCommand(sql, sqlconn))
            {
                //Console.WriteLine("command created successfuly");

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);

                sqlconn.Open();
                //Console.WriteLine("connection opened successfuly");
                adapt.Fill(dttable);
                sqlconn.Close();
                //Console.WriteLine("connection closed successfuly");
            }
            return dttable;
        }

        private static bool DeleteRelationalData(string applicationid, string applicationno, string programid, string tablename, System.Data.SqlClient.SqlConnection conn)

        {

            string cmdString = "DELETE FROM " + tablename + " WHERE PROGRAMME_ID = @pgmid  AND APPLICATION_NUMBER=@applicationid";
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                //comm.Parameters.AddWithValue("@tablename", tablename);
                comm.Parameters.AddWithValue("@applicationid", applicationid);
                comm.Parameters.AddWithValue("@pgmid", programid);

                try
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                    //Console.WriteLine(" - Sucessfully Created New House keeping Month Record");
                    log.Info("Deleted  Application No. " + " / <***** " + applicationno + " *****> from table : / " + tablename + " /");
                    return true;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Delete Data Error Info" + e);
                    log.Error("DeleteRelationalData Error Info :" + e);
                    conn.Close();
                    return false;
                }
            }


        }


        private static bool DeleteAdditionalRelationalData(string applicationid, string applicationno, string programid, string tablename, System.Data.SqlClient.SqlConnection conn)

        {

            string cmdString = "DELETE FROM " + tablename + " WHERE PROGRAMME_ID = @pgmid  AND APPLICATION_NUMBER=@applicationid";
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                //comm.Parameters.AddWithValue("@tablename", tablename);
                comm.Parameters.AddWithValue("@applicationid", applicationno);
                comm.Parameters.AddWithValue("@pgmid", programid);

                try
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                    //Console.WriteLine(" - Sucessfully Created New House keeping Month Record");
                    log.Info("Deleted  Application No. " + " / <***** " + applicationno + " *****> from table : / " + tablename + " /");
                    return true;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Delete Data Error Info" + e);
                    log.Error("DeleteAdditionalRelationalData Error Info :" + e);
                    conn.Close();
                    return false;
                }
            }

        }

        private static bool DeleteData(string applicationid, string applicationno, string programid, string keycolumn, string tablename, System.Data.SqlClient.SqlConnection conn)

        {
            string cmdString = "DELETE FROM " + tablename + " WHERE @keycolumn = @applicationid AND PROGRAMME_ID=@pgmid";
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                //comm.Parameters.AddWithValue("@tablename", tablename);
                comm.Parameters.AddWithValue("@keycolumn", keycolumn);
                comm.Parameters.AddWithValue("@applicationid", applicationid);
                comm.Parameters.AddWithValue("@pgmid", programid);

                try
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                    //Console.WriteLine(" - Sucessfully Created New House keeping Month Record");
                    log.Info("Deleted  Application No. " + " / <***** " + applicationno + " *****> from table : / " + tablename + " /");
                    return true;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Delete Data Error Info" + e);
                    log.Error("Delete Data Error Info :" + e);
                    conn.Close();
                    return false;
                }

            }


        }

        private static string ReadUserNumericinput()
        {
            string _val = "";
            Console.WriteLine("Enter your value for House  Keep Month : ");
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace)
                {
                    double val = 0;
                    bool _x = double.TryParse(key.KeyChar.ToString(), out val);
                    if (_x)
                    {
                        _val += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);

            return _val; ;

        }


        private static string ReadUserOptions()
        {

            bool correctInput = false;

            Console.WriteLine("Enter your value for required Opperation ( 1-Retrive Values / 2- Delete Values according to house keeping month configuration )  : ");

            while (!correctInput)
            {
                correctInput = true;

                string decision = Console.ReadLine();
                int iDecision;
                if (int.TryParse(decision, out iDecision))
                    switch (iDecision)
                    {
                        case 1:
                            return iDecision.ToString();

                        case 2:
                            return iDecision.ToString();
                        default:
                            correctInput = false;
                            break;
                    }
                else
                    correctInput = false;

                if (!correctInput)
                    Console.WriteLine("input was bad.");
            }
            return "";
        }


        private static bool StoreUserInput(string HouseKeepMonth, System.Data.SqlClient.SqlConnection conn)
        {
            string cmdString = "INSERT INTO TB_SYSTEM_PARAMETER(Config_Code,Value,Created_By,Created_Date) VALUES (@val1, @val2, @val3,@val4)";
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                comm.Parameters.AddWithValue("@val1", "HouseKeepMonth");
                comm.Parameters.AddWithValue("@val2", HouseKeepMonth);
                comm.Parameters.AddWithValue("@val3", "Ansh Lulla");
                comm.Parameters.AddWithValue("@val4", DateTime.Now);

                try
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                    //Console.WriteLine(" - Sucessfully Created New House keeping Month Record");
                    log.Info("Sucessfully Created New House keeping Month Record : " + HouseKeepMonth);
                    return true;
                    
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error On Insert House Keeping Month " + e);
                    return false;
                }

            }
        }

        private static bool SelectData(string applicationid, string applicationno, string programid, string keycolumn, string tablename, System.Data.SqlClient.SqlConnection conn)

        {
            string cmdString = "select * FROM  " + tablename + "  WHERE @keycolumn = @applicationid AND PROGRAMME_ID=@pgmid";
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                //comm.Parameters.AddWithValue("@tablename", tablename);
                comm.Parameters.AddWithValue("@keycolumn", keycolumn);
                comm.Parameters.AddWithValue("@applicationid", applicationid);
                comm.Parameters.AddWithValue("@pgmid", programid);

               
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        log.Info(applicationid + " / <***** " + applicationno + " / <***** " + programid + " / <***** " + keycolumn + " / <***** " + tablename);
                        Console.WriteLine("Selected  Application No. " + " / <***** " + applicationno + " *****> from table : / " + tablename + " /");
                
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader.GetName(i).ToString() + " : " + reader.GetValue(i));
                            }
                            Console.WriteLine();
                        }
                    }
                    conn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Delete Data Error Info" + e);
                    log.Error("Select Data Error Info" + e);
                    conn.Close();
                    return false;
                }
            }

        }

        private static bool SelectRelationalData(string applicationid, string applicationno, string programid, string tablename, System.Data.SqlClient.SqlConnection conn)

        {
            string cmdString = "select * FROM " + tablename + " WHERE APPLICATION_ID = @applicationid AND PROGRAMME_ID=@pgmid";
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                //comm.Parameters.AddWithValue("@tablename", tablename);
                comm.Parameters.AddWithValue("@applicationid", applicationid);
                comm.Parameters.AddWithValue("@pgmid", programid);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        log.Info(applicationid + " / <***** " + applicationno + " / <***** " + programid + " / <***** " + tablename);
                        Console.WriteLine("Selected  Application No. " + " / <***** " + applicationno + " *****> from table : / " + tablename + " /");
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader.GetName(i).ToString() + " : " + reader.GetValue(i));
                            }
                            Console.WriteLine();
                        }
                    }
                    conn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    log.Error("Select Relational Data Error Info : " + e);
                    log.Info(applicationid + " / <***** " + applicationno + " / <***** " + programid  + " / <***** " + tablename);
                    conn.Close();
                    return false;
                }
            }

        }

        private static bool SelectAdditionalRelationalData(string applicationid, string applicationno, string programid, string tablename, System.Data.SqlClient.SqlConnection conn)

        {
            string cmdString = "select * FROM " + tablename + " WHERE APPLICATION_NUMBER = @applicationid AND PROGRAMME_ID=@pgmid";
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                //comm.Parameters.AddWithValue("@tablename", tablename);
                comm.Parameters.AddWithValue("@applicationid", applicationid);
                comm.Parameters.AddWithValue("@pgmid", programid);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        Console.WriteLine("Selected  Application No. " + " / <***** " + applicationno + " *****> from table : / " + tablename + " /");
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader.GetName(i).ToString() + " : " + reader.GetValue(i));
                            }
                            Console.WriteLine();
                        }
                    }
                    conn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    log.Error("Select Additional Relational Data Error Info" + e);
                    log.Info(applicationid + " / <***** " + applicationno + " / <***** " + programid + " / <***** " + tablename);
                    conn.Close();
                    return false;
                }
            }

        }
        
        private static bool SelectSPRelationalData(string applicationid, string applicationno, string programid, string listname, string spsite)

        {
            //bool result = true;

            try
            {
                ClientContext clientContext = new ClientContext(spsite);

                List list = clientContext.Web.Lists.GetByTitle(listname);

                //CamlQuery query = new CamlQuery();
                //query.ViewXml = "<View/>";

                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='APPLICATION_ID'/>" +
                    "<Value Type='string'>" + applicationid + "</Value></Eq></Where></Query></View>";


                ListItemCollection items = list.GetItems(camlQuery);

                clientContext.Load(list);
                clientContext.Load(items);

                clientContext.ExecuteQuery();

                foreach (ListItem item in items)
                {
                    if (item["PROGRAMME_ID"].ToString() == programid)
                    {
                        Console.WriteLine("ID: {0} \nTitle: {1} \nPROGRAMME_ID: {2}", item.Id, item["Title"], item["PROGRAMME_ID"].ToString());
                        log.Info(item.Id + " - " + item["PROGRAMME_ID"].ToString() + " - " + applicationno);

                    }
                }
                return true;
            }

            catch (Exception e)
            {
                log.Error("Select SharePoint List items failed : " + e);
                return false;
            }
        }

        private static bool DeleteSPRelationalData(string applicationid, string applicationno, string programid, string listname, string spsite)

        {
            //bool result = true;

            try
            {
                ClientContext clientContext = new ClientContext(spsite);

                List list = clientContext.Web.Lists.GetByTitle(listname);

                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='APPLICATION_ID'/>" +
                    "<Value Type='string'>" + applicationid + "</Value></Eq></Where></Query></View>";

                ListItemCollection items = list.GetItems(camlQuery);

                clientContext.Load(list);
                clientContext.Load(items);

                clientContext.ExecuteQuery();


                foreach (ListItem item in items)
                {
                    if (item["PROGRAMME_ID"].ToString() == programid)
                    {
                        item.DeleteObject();
                        Console.WriteLine("Deleting .... : ID: {0} \nTitle: {1} \nPROGRAMME_ID: {2}", item.Id, item["Title"], item["PROGRAMME_ID"].ToString());
                        log.Info("Deleted SharePoint Item : " + item.Id + " - " + item["PROGRAMME_ID"].ToString() + " - " + applicationno);
                        break;
                    }
                }
                clientContext.ExecuteQuery();

                return true;
            }

            catch (Exception e)
            {
                log.Error("Delete Action of SharePoint List items failed : " + e);
                return false;
            }


        }


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
