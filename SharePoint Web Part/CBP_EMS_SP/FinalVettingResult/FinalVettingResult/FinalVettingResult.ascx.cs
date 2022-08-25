using CBP_EMS_SP.Common;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Data.CustomModels;
using CBP_EMS_SP.Data.Models;
using Novacode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls.WebParts;

namespace FinalVettingResult.FinalVettingResult
{
    [ToolboxItemAttribute(false)]
    public partial class FinalVettingResult : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public FinalVettingResult()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Context.Request.UrlReferrer != null)
                {
                    btn_Cancel.PostBackUrl = Context.Request.UrlReferrer.ToString();
                }
                FillPrograms();
            }
        }
        List<PresentationVettingUser> objUserData = new List<PresentationVettingUser>();

        public List<PrsentationResultSummary> GetList()
        {
            List<PrsentationResultSummary> objPrsentationResultSummary = new List<PrsentationResultSummary>();
            if (!string.IsNullOrEmpty(Context.Request.QueryString["program_id"]))
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    var program_id = Context.Request.QueryString["program_id"];
                    int programId = Convert.ToInt32(program_id);

                    List<TB_EC_RESULT> objTB_VETTING_APPLICATION = dbContext.TB_EC_RESULT.Where(x => x.Programme_ID == programId).ToList();
                    List<TB_PROGRAMME_INTAKE> objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList();


                    string sql = "";

                    var connection = new SqlConnection(connStr);
                    connection.Open();
                    try
                    {
                        if (objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name.ToLower().Contains("incubation"))
                        {
                            sql = "select *, " +

" case when(data.Recommendedcount > data.NotRecommendedcount) then '1' else '0' end totalrecommended" +
" ,(data.Recommendedcount + data.NotRecommendedcount) totalvotes from(select inc.Application_Number as Application_Number, inc.Company_Name_Eng as Company_Program , vtmet.Vetting_Meeting_ID as Vetting_Meeting_ID," +
 " (select count(*) from TB_PRESENTATION_INCUBATION_SCORE scor where scor.Application_Number = inc.Application_Number and scor.Go = 1) as Recommendedcount," +

" (select count(*) from TB_PRESENTATION_INCUBATION_SCORE scor where scor.Application_Number = " +
" inc.Application_Number and scor.Go = 0) as NotRecommendedcount ," +
" case when(vtmet.Vetting_Meeting_ID is null) then null else vtmet.meeting_status end as Meeting_Status from TB_INCUBATION_APPLICATION inc left join TB_VETTING_MEETING vtmet on vtmet.Programme_ID = inc.Programme_ID " +
                                //left join TB_VETTING_APPLICATION vtApp on vtApp.Vetting_Meeting_ID = vtmet.Vetting_Meeting_ID and inc.Application_Number = vtApp.Application_Number 
" where inc.Programme_ID = " + programId + " and inc.Application_Parent_ID is null)" +
" data order by data.application_number";
                        }
                        else
                        {
                            sql = "select *, " +

" case when(data.Recommendedcount > data.NotRecommendedcount) then '1' else '0' end totalrecommended" +
" ,(data.Recommendedcount + data.NotRecommendedcount) totalvotes from(select inc.Application_Number as Application_Number, inc.Company_Name_Eng as Company_Program , vtmet.Vetting_Meeting_ID as Vetting_Meeting_ID," +
 " (select count(*) from TB_PRESENTATION_CCMF_SCORE scor where scor.Application_Number = inc.Application_Number and scor.Go = 1) as Recommendedcount," +

" (select count(*) from TB_PRESENTATION_CCMF_SCORE scor where scor.Application_Number = " +
" inc.Application_Number and scor.Go = 0) as NotRecommendedcount ," +
" case when(vtmet.Vetting_Meeting_ID is null) then null else vtmet.meeting_status end as Meeting_Status from TB_CCMF_APPLICATION inc left join TB_VETTING_MEETING vtmet on vtmet.Programme_ID = inc.Programme_ID" +
                                //left join TB_VETTING_APPLICATION vtApp on vtApp.Vetting_Meeting_ID = vtmet.Vetting_Meeting_ID and inc.Application_Number = vtApp.Application_Number
" where inc.Programme_ID =" + programId + " and inc.Application_Parent_ID is null)" +
" data order by data.application_number";
                        }


                        SqlCommand command = new SqlCommand(sql, connection);
                        command.CommandType = CommandType.Text;
                        DataTable dtResult = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtResult);

                        foreach (DataRow dr in dtResult.Rows)
                        {
                            objPrsentationResultSummary.Add(new PrsentationResultSummary
                            {
                                Application_Number = Convert.ToString(dr["Application_Number"]),
                                company_name = Convert.ToString(dr["Company_Program"]),
                                Recommendedcount = Convert.ToInt32(dr["Recommendedcount"]),
                                NotRecommendedcount = Convert.ToInt32(dr["NotRecommendedcount"]),
                                totalvotes = Convert.ToInt32(dr["totalvotes"]),
                                totalrecommended = Convert.ToInt32(dr["totalrecommended"]),

                                Vetting_Meeting_ID = !string.IsNullOrEmpty(Convert.ToString(dr["Vetting_Meeting_ID"])) ? Guid.Parse(Convert.ToString(dr["Vetting_Meeting_ID"])) : Guid.Empty,
                                Meeting_Status = Convert.ToString(dr["Meeting_Status"]),

                                //Score_of_vettingmember = IncubationContext.Get_programme_summary_ccmf(Guid.Parse(Convert.ToString(dr["Vetting_Meeting_ID"])), dbContext, objUserData);




                            });
                            foreach (PrsentationResultSummary objSummary in objPrsentationResultSummary)
                            {
                                objSummary.Score_of_vettingmember = new List<Presentation_Score>();
                                if (objSummary.Vetting_Meeting_ID != Guid.Empty)
                                {
                                    List<PresentationVettingUser> userdata = IncubationContext.GetUserList(dbContext, objSummary.Vetting_Meeting_ID);
                                    string VMID = objSummary.Application_Number;
                                    objSummary.Score_of_vettingmember = IncubationContext.Get_Finalresultbyapplicationnumber(VMID, dbContext, userdata, objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name).Score_of_vettingmember;
                                }


                                //objPrsentationResultSummary.Remove(objSummary);
                            }
                            //Guid Vetting_Meeting_ID = !string.IsNullOrEmpty(Convert.ToString(dr["Vetting_Meeting_ID"])) ? Guid.Parse(Convert.ToString(dr["Vetting_Meeting_ID"])) : Guid.Empty;
                            string Application_Number = Convert.ToString(dr["Application_Number"]);
                            List<PrsentationResultSummary> obj = (objPrsentationResultSummary.Where(x => x.Application_Number == Application_Number)).ToList();


                            if (obj.Count() > 1)
                            {
                                bool issaved = false;
                                foreach (PrsentationResultSummary item in obj)
                                {
                                    if (item.Meeting_Status.ToLower() != "email sended")
                                    {
                                        issaved = true;
                                    }
                                }
                                if (issaved)
                                {
                                    //objPrsentationResultSummary.Except(obj).ToList();
                                    //obj.Except(objPrsentationResultSummary).ToList();
                                    objPrsentationResultSummary = objPrsentationResultSummary.Except(obj).ToList();
                                }

                            }

                        }
                        command.Dispose();
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {

                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return objPrsentationResultSummary;
        }
        public void FillPrograms()
        {


            List<PrsentationResultSummary> objPrsentationResultSummary = GetList();

            lbltotalprojects.Text = Convert.ToString(objPrsentationResultSummary.Count());
            lblShort_Listed.Text = Convert.ToString(objPrsentationResultSummary.Where(x => x.Meeting_Status != null && x.Meeting_Status != "").Count());
            lblrecommended.Text = Convert.ToString(objPrsentationResultSummary.Where(x => x.totalrecommended == 1).Count());
            lblnotrecommended.Text = Convert.ToString(objPrsentationResultSummary.Where(x => x.totalrecommended == 0).Count());


            rptrprogrammesummary.DataSource = objPrsentationResultSummary;
            rptrprogrammesummary.DataBind();
            // reader.Dispose();


        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Context.Request.QueryString["program_id"]))
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    using (DocX doc = DocX.Create(string.Format("Report-{0}.doc", DateTime.Now.Ticks)))
                    {
                        var program_id = Context.Request.QueryString["program_id"];
                        int programId = Convert.ToInt32(program_id);

                        TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programId);
                        string fileNameTemplate = @"F:\New folder\DocXExample.docx";


                        //string outputFileName =
                        //string.Format(fileNameTemplate, DateTime.Now.ToString("MM-dd-yy"));


                        DocX letter = this.GetRejectionLetterTemplate(doc);

                        letter.ReplaceText("%ProgrammeName%", objTB_PROGRAMME_INTAKE.Programme_Name);
                        letter.ReplaceText("%Intakenumber%", objTB_PROGRAMME_INTAKE.Intake_Number.ToString());
                        letter.ReplaceText("%TotalApplicatio%", lbltotalprojects.Text);
                        letter.ReplaceText("%ShortlistedApplications%", lblShort_Listed.Text);
                        letter.ReplaceText("%Recommended%", lblrecommended.Text);
                        letter.ReplaceText("%NotRecommended%", lblnotrecommended.Text);
                        if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation"))
                        {
                            letter.ReplaceText("%shortprogramme%", "Incubation");
                        }
                        else if (!objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("university") && !objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("hong kong"))
                        {
                            letter.ReplaceText("%shortprogramme%", "CCMF – Cross Border");
                        }
                        else if (!objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("crossborder") && !objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("university"))
                        {
                            letter.ReplaceText("%shortprogramme%", "CCMF – Hong Kong");

                        }
                        else
                        {
                            letter.ReplaceText("%shortprogramme%", "CCMF – CUPP");

                        }
                        SPFunctions objfunction = new SPFunctions();
                        letter.ReplaceText("%Recievername%", objfunction.GetCurrentUser());
                        letter.ReplaceText("%Date%", objTB_PROGRAMME_INTAKE.Application_Start.ToShortDateString());
                        IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new CyberportEMS_EDM().TB_SYSTEM_PARAMETER;
                        letter.ReplaceText("%contactno%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Contact").Value);
                        letter.ReplaceText("%email%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Email").Value);

                        //letter.SaveAs(outputFileName);

                        MemoryStream ms = new MemoryStream();
                        letter.SaveAs(ms);



                        System.Web.HttpContext.Current.Response.Clear();
                        System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Final Vetting Result.docx");
                        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        ms.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
                        System.Web.HttpContext.Current.Response.End();
                    }


                }
            }
        }
        private DocX GetRejectionLetterTemplate(DocX doc)
        {

            // Adjust the path so suit your machine:
            string fileName = @"D:\Users\John\Documents\DocXExample.docx";

            // Set up our paragraph contents:
            string headerText = "Endorsement on Final Vetting Result for the %ProgrammeName% %Intakenumber% Recruitment";
            string letterBodyText = DateTime.Now.ToShortDateString() + Environment.NewLine + "%Recievername%" + Environment.NewLine;
            //+ "%Address1%"
            //+ Environment.NewLine + "%Address2%" + Environment.NewLine + "%Address3%";
            string paraOne = ""
                    + "Dear %Recievername%" + Environment.NewLine
                   + "Thank you for being the Vetting Team Leader for the Presentation Session "
                   + "of the %ProgrammeName% %Intakenumber%(%shortprogramme%) Recruitment on the %Date%." + Environment.NewLine
                    + " I am writing to seek for your endorsement on the Final Vetting Result of"
                    + "the Scheme.Please find below a summary of the result for your reference." + Environment.NewLine;
            string paraTwo = ""
              + "For details, please refer to the enclosed Final Vetting Result of"
              + " %shortprogramme%  %Intakenumber% Recruitment.Kindly endorse the result by signing and returning this letter."
               + Environment.NewLine +
               "Thank you very much for your gracious support.Should yopu require any assisstance,please do not hestitate to contact me at %contactno% or %email%";
            string paraThree = ""
             + "Summary of %shortprogramme%  %Intakenumber% Result ";
            string paraFour = ""
            + "Encl: Finalt Vetting Result of %shortprogramme%  %Intakenumber% Recruitment ";
            string paraFive = ""
                + " Finalt Vetting Result of %shortprogramme%  %Intakenumber% Recruitment ";


            // Title Formatting:
            var titleFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Arial");
            titleFormat.Size = 15D;
            titleFormat.Position = 12;
            titleFormat.Bold = true;

            // Body Formatting
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Arial");
            paraFormat.Size = 11D;
            titleFormat.Position = 12;
            var listformatting = new Formatting();
            listformatting.FontFamily = new System.Drawing.FontFamily("Arial");
            listformatting.Size = 11D;
            titleFormat.Position = 12;

            // Create the doculment in memory:
            // var doc = DocX.Create(fileName);


            doc.MarginLeft = 10f;
            doc.MarginRight = 10f;



            // Insert each prargraph, with appropriate spacing and alignment:
            doc.InsertParagraph(Environment.NewLine);
            Paragraph letterBody = doc.InsertParagraph(letterBodyText, false, paraFormat);
            letterBody.Alignment = Alignment.both;
            doc.InsertParagraph(Environment.NewLine);
            Paragraph title = doc.InsertParagraph(headerText, false, titleFormat);
            title.Alignment = Alignment.center;



            doc.InsertParagraph(Environment.NewLine);
            doc.InsertParagraph(paraOne, false, paraFormat);

            doc.InsertParagraph(paraThree, false, paraFormat);
            var list = doc.AddList(listType: ListItemType.Bulleted, startNumber: 1);

            doc.AddListItem(list, "Total no. of Application: %TotalApplicatio%", 0, listType: ListItemType.Bulleted);
            doc.AddListItem(list, "Sortlisted for Presentation:  %ShortlistedApplications%", 0, listType: ListItemType.Bulleted);
            doc.AddListItem(list, "Recommended:  %Recommended%", 0, listType: ListItemType.Bulleted);
            doc.AddListItem(list, "Not Recommended: %NotRecommended%", 0, listType: ListItemType.Bulleted);

            doc.InsertList(list);
            doc.InsertParagraph(Environment.NewLine);
            doc.InsertParagraph(paraTwo, false, paraFormat);
            doc.InsertParagraph(Environment.NewLine);

            Table t = doc.AddTable(9, 3); // rows and columns - 2 and 4
            t.Alignment = Alignment.center;
            Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 1, Color.White);
            t.SetBorder(TableBorderType.Bottom, b);
            t.SetBorder(TableBorderType.Left, b);
            t.SetBorder(TableBorderType.Right, b);
            t.SetBorder(TableBorderType.Top, b);
            t.SetBorder(TableBorderType.InsideH, b);
            t.SetBorder(TableBorderType.InsideV, b);
            t.Rows[0].Cells[0].Paragraphs.First().Append("Your Sincerley,");
            t.Rows[0].Cells[1].Paragraphs.First().Append("");
            t.Rows[0].Cells[2].Paragraphs.First().Append("SIGNED by");
            t.Rows[1].Cells[0].Paragraphs.First().Append("");
            t.Rows[1].Cells[1].Paragraphs.First().Append("");
            t.Rows[1].Cells[2].Paragraphs.First().Append("");
            t.Rows[2].Cells[0].Paragraphs.First().Append("");
            t.Rows[2].Cells[1].Paragraphs.First().Append("");
            t.Rows[2].Cells[2].Paragraphs.First().Append("");


            t.Rows[3].Cells[0].Paragraphs.First().Append("______________");
            t.Rows[3].Cells[1].Paragraphs.First().Append("");
            t.Rows[3].Cells[2].Paragraphs.First().Append("______________");
            t.Rows[4].Cells[0].Paragraphs.First().Append("Name");
            t.Rows[4].Cells[1].Paragraphs.First().Append("");
            t.Rows[4].Cells[2].Paragraphs.First().Append("Name");
            t.Rows[5].Cells[0].Paragraphs.First().Append("Title");
            t.Rows[5].Cells[1].Paragraphs.First().Append("");
            t.Rows[5].Cells[2].Paragraphs.First().Append("Title");
            t.Rows[6].Cells[0].Paragraphs.First().Append("Company");
            t.Rows[6].Cells[1].Paragraphs.First().Append("");
            t.Rows[7].Cells[0].Paragraphs.First().Append("Date");
            t.Rows[7].Cells[1].Paragraphs.First().Append("");
            doc.InsertTable(t);
            doc.InsertParagraph(Environment.NewLine);
            doc.InsertParagraph(paraFour, false, paraFormat);
            doc.InsertParagraph(Environment.NewLine);
            doc.InsertParagraph(paraFive, false, paraFormat);
            doc.InsertParagraph(Environment.NewLine);
            List<PrsentationResultSummary> objPrsentationResultSummary = GetList();

            Table tdata = doc.AddTable(objPrsentationResultSummary.Count + 1, 7);
            tdata.Alignment = Alignment.left;
            tdata.SetColumnWidth(0, 500);
            tdata.SetColumnWidth(1, 2200);
            tdata.SetColumnWidth(2, 2200);
            tdata.SetColumnWidth(3, 800);
            tdata.SetColumnWidth(4, 1000);
            tdata.SetColumnWidth(5, 2000);
            tdata.SetColumnWidth(6, 3000);
            tdata.SetTableCellMargin(TableCellMarginType.left, 100);
            tdata.SetTableCellMargin(TableCellMarginType.right, 100);


            tdata.SetBorder(TableBorderType.InsideH, new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));


            tdata.Rows[0].Cells[0].Paragraphs.First().Append("No.");
            tdata.Rows[0].Cells[1].Paragraphs.First().Append("Application No.");
            tdata.Rows[0].Cells[2].Paragraphs.First().Append("Project Name");

            tdata.Rows[0].Cells[3].Paragraphs.First().Append("Go");

            tdata.Rows[0].Cells[4].Paragraphs.First().Append("Not Go");

            tdata.Rows[0].Cells[5].Paragraphs.First().Append("Total No. of Votes");
            tdata.Rows[0].Cells[6].Paragraphs.First().Append("Remarks");
            for (int i = 0; i < objPrsentationResultSummary.Count; i++)
            {

                tdata.Rows[i + 1].Cells[0].Paragraphs.First().Append((i + 1).ToString());
                tdata.Rows[i + 1].Cells[1].Paragraphs.First().Append(objPrsentationResultSummary[i].Application_Number.ToString());
                tdata.Rows[i + 1].Cells[2].Paragraphs.First().Append(objPrsentationResultSummary[i].company_name.ToString());
                tdata.Rows[i + 1].Cells[3].Paragraphs.First().Append(objPrsentationResultSummary[i].Recommendedcount.ToString());

                tdata.Rows[i + 1].Cells[4].Paragraphs.First().Append(objPrsentationResultSummary[i].NotRecommendedcount.ToString());
                tdata.Rows[i + 1].Cells[5].Paragraphs.First().Append(objPrsentationResultSummary[i].totalvotes.ToString());

                for (int j = 0; j < objPrsentationResultSummary[i].Score_of_vettingmember.Count; j++)
                {
                    if (objPrsentationResultSummary[i].Score_of_vettingmember[j].Remarks != "")
                    {
                        if (j == 0)
                        {

                            tdata.Rows[i + 1].Cells[6].Paragraphs.First().Append("0" + (j + 1) + ":" + objPrsentationResultSummary[i].Score_of_vettingmember[j].Remarks.ToString());
                        }
                        else
                        {
                            tdata.Rows[i + 1].Cells[6].Paragraphs.First().AppendLine("0" + (j + 1) + ":" + objPrsentationResultSummary[i].Score_of_vettingmember[j].Remarks.ToString());
                        }
                    }
                }


            }
            doc.InsertTable(tdata);
            return doc;
        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            PopupBConfirm.Visible = true;
            //comented in 2018
            //  pnlsubmissionpopup.Visible = true;
        }
        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlsubmissionpopup.Visible = false;

            btn_Confirm.Visible = false;
            btn_export.Visible = true;


        }
        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            PopupBConfirm.Visible = false;
        }

        protected void btn_submitFinal_Click(object sender, EventArgs e)
        {
            pnlsubmissionpopup.Visible = true;
            PopupBConfirm.Visible = false;
        }
    }

    //protected void btnexport_Click(object sender, EventArgs e)
    //{

    //}
}





