using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelImport
{
    public partial class FrnDataUpload : Form
    {
        string insertQuery = @"INSERT INTO [dbo].[TB_PAST_APPLICATION]
           ([Application_Number]
           ,[ID_Number]
           ,[Name]
           ,[Member_Name]
           ,[Masked_HKID]
           ,[Programme_Type]
            ,[Programme_Stream]
            ,[Application_Type]
            ,[Description]
            ,[Reference_Number]
)
     VALUES";

        string insertQueryValues = @" ( '{0}','{1}','{2}',N'{3}','{4}','{5}','{6}','{7}','{8}','{9}'),";
        public FrnDataUpload()
        {
            InitializeComponent();
        }

        private void btn_FileUploader_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel Document(*.xls)|*.xlsx";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    string filepath = openFileDialog1.FileName;
            //}

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            label2.Text = openFileDialog1.FileName;
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            SqlCommand objCmd = new SqlCommand();

            try
            {
                if (!string.IsNullOrEmpty(openFileDialog1.FileName))
                {

                    string[] strContats = File.ReadAllLines(openFileDialog1.FileName);
                    FileInfo objFile = new FileInfo(openFileDialog1.FileName);
                    openFileDialog1.FileName = "";

                    using (ExcelPackage xlPackage = new ExcelPackage(objFile))
                    {
                        // get the first worksheet in the workbook
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];

                        Dictionary<int, string> ListHeaders = new Dictionary<int, string>();

                        for (int iColumn = 1; iColumn > 0; iColumn++)
                        {
                            string ColumnKey = worksheet.Cell(1, iColumn).Value;
                            if (string.IsNullOrEmpty(ColumnKey))
                            {
                                break;
                            }
                            else
                                ListHeaders.Add(iColumn, ColumnKey);
                        }
                        List<DataList> objDataList = new List<DataList>();
                        int iRow = 2;
                        while (iRow > 0)
                        {
                            DataList objData = new DataList();
                            string ApplicationNumber = worksheet.Cell(iRow, 1).Value;
                            string CompanyName = worksheet.Cell(iRow, 2).Value;


                            if (string.IsNullOrEmpty(ApplicationNumber))
                            {
                                break;
                            }
                            else {
                                objData.Application_Number = ApplicationNumber;
                                objData.Name = CompanyName;
                                objData.Programme_Type = worksheet.Cell(iRow, 3).Value;
                                objData.Programme_Type = !string.IsNullOrEmpty(objData.Programme_Type) ? objData.Programme_Type : null;

                                objData.Programme_Stream = worksheet.Cell(iRow, 4).Value;

                                if (!string.IsNullOrEmpty(objData.Programme_Stream))
                                {
                                    if (objData.Programme_Stream.ToLower() == "pro" || objData.Programme_Stream.ToLower().StartsWith("professional"))
                                    {
                                        objData.Programme_Stream = "PRO";
                                    }
                                    else if (objData.Programme_Stream.ToLower() == "yep" || objData.Programme_Stream.ToLower().Contains("young"))
                                    {
                                        objData.Programme_Stream = "YEP";
                                    }
                                    else
                                    {
                                        objData.Programme_Stream = null;
                                    }

                                }
                                else
                                    objData.Programme_Stream = null;


                                objData.Application_Type = worksheet.Cell(iRow, 5).Value;
                                if (!string.IsNullOrEmpty(objData.Application_Type) && (objData.Application_Type.ToLower().StartsWith("individual") || objData.Application_Type.ToLower().StartsWith("company")))
                                {
                                    objData.Application_Type = objData.Application_Type;
                                }
                                else objData.Application_Type = null;

                                objData.Description = string.Empty; //worksheet.Cell(iRow, 6).Value;

                                if (string.IsNullOrEmpty(objData.Description))
                                    objData.Description = null;
                                objData.Reference_Number = worksheet.Cell(iRow, 7).Value;

                                if (string.IsNullOrEmpty(objData.Reference_Number))
                                    objData.Reference_Number = null;


                                objData.Members = new List<MembersList>();
                                int iColumn = 8;
                                while (iColumn <= ListHeaders.Count() + 1)
                                {
                                    MembersList objMember = new MembersList();
                                    string Member_Name = worksheet.Cell(iRow, iColumn).Value;
                                    iColumn += 1;
                                    string Member_ID = worksheet.Cell(iRow, iColumn).Value;
                                    iColumn += 1;
                                    if (!string.IsNullOrEmpty(Member_ID) && !string.IsNullOrEmpty(Member_Name))
                                    {
                                        objMember.ID_Masked = (Member_ID.Length > 4) ? Member_ID.Remove(4) + "******" : Member_ID + "******";
                                        objMember.ID_Encrypted = Encryption.EncryptData(Member_ID);
                                        objMember.Name = Member_Name;
                                        objData.Members.Add(objMember);
                                    }else {

                                        //if (!string.IsNullOrEmpty(Member_ID)) {
                                        //    Console.WriteLine(string.Format("Member_ID: {0}", Member_ID));
                                        //}

                                        //if (!string.IsNullOrEmpty(Member_Name)) {
                                        //    Console.WriteLine(string.Format("Member_Name: {0}", Member_Name));
                                        //}
                                    }
                                }
                            }
                            objDataList.Add(objData);
                            iRow++;
                        }

                        if (objDataList.Count > 0)
                        {
                            int i = 0;
                            string Query = "";
                            foreach (DataList objData in objDataList)
                            {
                                if (objData.Members.Count() > 0)
                                {
                                    if (i == 0)
                                        Query += insertQuery;

                                    foreach (MembersList objMember in objData.Members)
                                    {
                                        Query += string.Format(insertQueryValues, objData.Application_Number.Trim(),
                                            objMember.ID_Encrypted.Trim(), objData.Name.Trim(), objMember.Name.Trim(), objMember.ID_Masked.Trim(),
                                            objData.Programme_Type, objData.Programme_Stream, objData.Application_Type, objData.Description, objData.Reference_Number);
                                        i += 1;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(Query))
                            {
                                Query = Query.Remove(Query.LastIndexOf(","));




                                objCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConString"]);
                                objCmd.Connection.Open();
                                objCmd.CommandText = Query;
                                objCmd.CommandType = CommandType.Text;
                                objCmd.ExecuteNonQuery();
                                objCmd.Connection.Close();

                            }
                        }
                        int TotalRows = 0;
                        objDataList.ForEach(x => TotalRows += x.Members.Count());
                        MessageBox.Show(objDataList.Count() + " excel row with " + TotalRows + " users inserted successfully.");
                    }

                    label2.Text = "";
                    openFileDialog1.FileName = "";

                }
                else
                    MessageBox.Show("Please upload file.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (objCmd.Connection != null) {
                    objCmd.Connection.Close();
                }

                this.Cursor = Cursors.Default;
            }
        }
    }

    public class DataList
    {
        public string Application_Number { get; set; }
        public string Name { get; set; }
        public string Programme_Type { get; set; }
        public string Programme_Stream { get; set; }
        public string Application_Type { get; set; }
        public string Description { get; set; }
        public string Reference_Number { get; set; }
        public List<MembersList> Members { get; set; }
    }
    public class MembersList
    {
        public string Name { get; set; }
        public string ID_Masked { get; set; }
        public string ID_Encrypted { get; set; }
    }
}
