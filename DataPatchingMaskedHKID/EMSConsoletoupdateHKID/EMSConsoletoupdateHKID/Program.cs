using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMSConsoletoupdateHKID
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection con;
            con = new SqlConnection(Properties.Settings.Default.ConnectionStr);
            con.Open();
            try
            {
                
                var sql = "";
                var sql1 = "";

                sql = "select core.Core_Member_ID, core.HKID from TB_APPLICATION_COMPANY_CORE_MEMBER core";
                    SqlCommand command = new SqlCommand(sql, con);
                    command.CommandType = CommandType.Text;

                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtResult);

                if (dtResult.Rows.Count > 0)
                {

                    foreach (DataRow dr in dtResult.Rows)
                    {
                        string HKID = Convert.ToString(dr["HKID"]);
                        int CorememberId = Convert.ToInt32(dr["Core_Member_ID"]);


                        string encrptedHKID = MD5Encryption.DecryptData(HKID);
                        if (encrptedHKID.Length > 0)
                        {
                            string masked = string.Empty;
                            string MaskedHKID = encrptedHKID;

                            if (encrptedHKID.Length > 4)
                            {
                                masked = new string('*', encrptedHKID.Length - 4);
                                MaskedHKID = encrptedHKID.Substring(0, 4) + masked;
                            }                            

                            sql1 = "update TB_APPLICATION_COMPANY_CORE_MEMBER set Masked_HKID='" + MaskedHKID + "' where Core_Member_ID='" + CorememberId + "'";
                            SqlCommand command1 = new SqlCommand(sql1, con);

                            command1.ExecuteNonQuery();

                            Console.WriteLine(CorememberId + ",[" + MaskedHKID + "]");
                        }
                    }
                    Console.Read();
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + "::" + e.StackTrace);
                Console.Read();
            }
            finally
            {
                con.Close();
            }
        }
    }
}
