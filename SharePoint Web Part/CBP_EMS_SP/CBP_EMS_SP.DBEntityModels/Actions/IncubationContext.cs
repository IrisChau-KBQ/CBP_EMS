using CBP_EMS_SP.Data.CustomModels;
using CBP_EMS_SP.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBP_EMS_SP.Data
{
    public class IncubationContext
    {
        public IncubationContext() { }

        public static List<TB_APPLICATION_FUNDING_STATUS> APPLICATION_FUNDING_STATUS_GET(Guid ApplicationID)
        {
            using (var DBContext = new CyberportEMS_EDM())
            {

                return DBContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == ApplicationID).ToList();
            }

        }
        public static List<TB_INCUBATION_APPLICATION> GET_INCUBATION_PROGRAMS()
        {
            using (var DBContext = new CyberportEMS_EDM())
            {

                return DBContext.TB_INCUBATION_APPLICATION.ToList();
            }

        }
        public static List<TB_CCMF_APPLICATION> GET_CCMF_PROGRAMS(string Programmename)
        {
            using (var DBContext = new CyberportEMS_EDM())
            {

                return DBContext.TB_CCMF_APPLICATION.Where(x => x.Programme_Type.ToLower() == Programmename.ToLower()).ToList();
            }

        }

        public static void APPLICATION_FUNDING_STATUS_ADDUPDATE(CyberportEMS_EDM DBContext, List<TB_APPLICATION_FUNDING_STATUS> objFundingList, Guid ApplicationId)
        {

            List<TB_APPLICATION_FUNDING_STATUS> objCurrentList = DBContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == ApplicationId).ToList();

            List<TB_APPLICATION_FUNDING_STATUS> objCurrentListNotExists = objCurrentList.Where(p => !objFundingList.Any(p2 => p2.Funding_ID == p.Funding_ID && p2.Application_ID == ApplicationId)).ToList();
            DBContext.TB_APPLICATION_FUNDING_STATUS.RemoveRange(objCurrentListNotExists.Where(x => x.Funding_ID != 0));

            DBContext.SaveChanges();

            foreach (TB_APPLICATION_FUNDING_STATUS objFunds in objFundingList)
            {
                TB_APPLICATION_FUNDING_STATUS objFundsDb = DBContext.TB_APPLICATION_FUNDING_STATUS.FirstOrDefault(x => x.Funding_ID == objFunds.Funding_ID && x.Application_ID == ApplicationId);
                if (objFundsDb == null)
                {
                    DBContext.TB_APPLICATION_FUNDING_STATUS.Add(objFunds);
                }
                else
                {
                    objFundsDb.Amount_Received = objFunds.Amount_Received;
                    objFundsDb.Application_ID = objFunds.Application_ID;
                    objFundsDb.Application_Status = objFunds.Application_Status;
                    objFundsDb.Currency = objFunds.Currency;
                    objFundsDb.Date = objFunds.Date;
                    objFundsDb.Expenditure_Nature = objFunds.Expenditure_Nature;
                    objFundsDb.Funding_Status = objFunds.Funding_Status;
                    objFundsDb.Maximum_Amount = objFunds.Maximum_Amount;
                    objFundsDb.Programme_ID = objFunds.Programme_ID;
                    objFundsDb.Programme_Name = objFunds.Programme_Name;
                }
            }
        }

    
        public static List<TB_APPLICATION_COMPANY_CORE_MEMBER> APPLICATION_COMPANY_CORE_MEMBER_GET(Guid ApplicationID)
        {
            using (var DBContext = new CyberportEMS_EDM())
            {

                return DBContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == ApplicationID).ToList();
            }

        }
        public static List<TB_PROGRAMME_INTAKE> Get_programme_name()
        {
            using (var DBContext = new CyberportEMS_EDM())
            {

                return DBContext.TB_PROGRAMME_INTAKE.Where(x => x.Active == true).ToList();
            }

        }
        public static List<PrsentationResultSummary> Get_Final_vetting_Result(int program_id, CyberportEMS_EDM dbcontext)
        {
            List<TB_EC_RESULT> objTB_VETTING_APPLICATION = new List<TB_EC_RESULT>();
            List<PrsentationResultSummary> listPrsentationResultSummary = new List<PrsentationResultSummary>();


            objTB_VETTING_APPLICATION = dbcontext.TB_EC_RESULT.Where(x => x.Programme_ID == program_id).ToList();

            List<Presentation_Score> objTB_PRESENTATION_CCMF_SCORE1 = new List<Presentation_Score>();
            List<TB_PRESENTATION_CCMF_SCORE> objTB_PRESENTATION_CCMF_SCORE = new List<TB_PRESENTATION_CCMF_SCORE>();
            if (objTB_VETTING_APPLICATION.Count > 0)
            {



                foreach (TB_EC_RESULT item in objTB_VETTING_APPLICATION)
                {
                    if (dbcontext.TB_PRESENTATION_INCUBATION_SCORE.Where(x => x.Application_Number == item.Application_Number).Count() > 0)
                    {
                        PrsentationResultSummary objPrsentationResultSummary = new PrsentationResultSummary();
                        objTB_PRESENTATION_CCMF_SCORE.AddRange(dbcontext.TB_PRESENTATION_CCMF_SCORE.Where(x => x.Application_Number == item.Application_Number).ToList()); objPrsentationResultSummary.Application_Number = item.Application_Number;
                        objPrsentationResultSummary.company_name = item.Company_Program;
                        objPrsentationResultSummary.Score_of_vettingmember = objTB_PRESENTATION_CCMF_SCORE.Where(x => x.Application_Number == item.Application_Number).Select(x => new Presentation_Score()
                        {

                            Remarks = x.Remarks,


                        }).ToList();
                        objPrsentationResultSummary.Totalscore = item.Total_votes;

                        objPrsentationResultSummary.isRecommended = (Convert.ToInt16(item.Recommendedcount));
                        objPrsentationResultSummary.isNotRecommended = (Convert.ToInt16(item.NotRecommendedcount));
                        objPrsentationResultSummary.NotRecommendedcount = listPrsentationResultSummary.Select(x => x.isNotRecommended).Count();
                        objPrsentationResultSummary.Recommendedcount = listPrsentationResultSummary.Select(x => x.isRecommended).Count();

                        listPrsentationResultSummary.Add(objPrsentationResultSummary);





                    }

                }








            }





            return listPrsentationResultSummary;

        }
        public static List<PresentationVettingUser> GetUserList(CyberportEMS_EDM DbContext, Guid vetting_metting_id)
        {

            return (from Vm in DbContext.TB_VETTING_MEMBER
                    join usr in DbContext.TB_VETTING_MEMBER_INFO on
                    Vm.Vetting_Member_ID equals usr.Vetting_Member_ID
                    where Vm.Vetting_Meeting_ID == vetting_metting_id
                    orderby usr.Email ascending
                    select new PresentationVettingUser()
                    {
                        VettingMember = Vm,
                        UserData = usr
                    }).ToList();
        }
        public static List<PrsentationResultSummary> Get_programme_summary_ccmf(Guid vettingid, CyberportEMS_EDM dbcontext, List<PresentationVettingUser> objUserData)
        {
            List<PrsentationResultSummary> listPrsentationResultSummary = new List<PrsentationResultSummary>();
            List<TB_VETTING_APPLICATION> objTB_VETTING_APPLICATION = dbcontext.TB_VETTING_APPLICATION.Where(x => x.Vetting_Meeting_ID == vettingid && x.Go == true).ToList();
            List<TB_VETTING_APPLICATION> ObjApp = objTB_VETTING_APPLICATION.Where(x => x.Application_Number.ToLower() == "time break").ToList();
            objTB_VETTING_APPLICATION = objTB_VETTING_APPLICATION.Except(ObjApp).ToList();
            //List<TB_PRESENTATION_CCMF_SCORE> objTB_PRESENTATION_CCMF_SCORE = new List<TB_PRESENTATION_CCMF_SCORE>();
            if (objTB_VETTING_APPLICATION.Count > 0)
            {
                if (objTB_VETTING_APPLICATION.FirstOrDefault().Application_Number.ToLower().Contains("cpip"))
                {


                    foreach (TB_VETTING_APPLICATION item in objTB_VETTING_APPLICATION)
                    {
                        PrsentationResultSummary objPrsentationResultSummary = new PrsentationResultSummary();
                        objPrsentationResultSummary.Vetting_Application_ID = item.Vetting_Application_ID;
                        objPrsentationResultSummary.Application_Number = item.Application_Number;
                        objPrsentationResultSummary.PresentationTime = item.Presentation_From;
                        TB_PRESENTATION_APPLICATION_REMARKS objPresentatinRemarks = dbcontext.TB_PRESENTATION_APPLICATION_REMARKS.FirstOrDefault(k => k.Vetting_Appilcation_ID == item.Vetting_Application_ID);
                        if (objPresentatinRemarks != null)
                        {
                            objPrsentationResultSummary.Withdraw = objPresentatinRemarks.Withdraw;
                            objPrsentationResultSummary.Remark = objPresentatinRemarks.Remark;
                        }
                        else
                        {
                            objPrsentationResultSummary.Withdraw = false;
                            objPrsentationResultSummary.Remark = "";

                        }
                        TB_INCUBATION_APPLICATION objProgram = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item.Application_Number);
                        if (objProgram != null)
                        {
                            objPrsentationResultSummary.company_name = objProgram.Company_Name_Eng;
                            objPrsentationResultSummary.ProgramId = objProgram.Programme_ID;
                            objPrsentationResultSummary.Cluster = objProgram.Business_Area;
                            objPrsentationResultSummary.Programme_Type = "";
                            objPrsentationResultSummary.Application_Type = "";
                            objPrsentationResultSummary.Preferred_track = objProgram.Preferred_Track;

                        }

                        if (dbcontext.TB_PRESENTATION_INCUBATION_SCORE.Where(x => x.Application_Number == item.Application_Number).Count() > 0)
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {
                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                TB_PRESENTATION_INCUBATION_SCORE objTbScore = dbcontext.TB_PRESENTATION_INCUBATION_SCORE.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Member_Email.ToLower() == objPresUser.UserData.Email.ToLower());
                                if (objTbScore != null)
                                {
                                    objScoreNew.Go = objTbScore.Go;
                                    objScoreNew.Member_Email = objTbScore.Member_Email;
                                    objScoreNew.Remarks = objTbScore.Remarks;
                                    objScoreNew.Total_Score = (Math.Round(Convert.ToDecimal(objTbScore.Total_Score), 3) * 1.0M);


                                }

                                //TB_VETTING_DECLARATION objTB_VETTING_DECLARATION = dbcontext.TB_VETTING_DECLARATION.FirstOrDefault(x => x.Member_Email == objScoreNew.Member_Email && x.Vetting_Meeting_ID == item.Vetting_Meeting_ID);
                                //if (objTB_VETTING_DECLARATION != null)
                                //{
                                //    TB_DECLARATION_APPLICATION objTB_DECLARATION_APPLICATION = dbcontext.TB_DECLARATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Vetting_Delclaration_ID == objTB_VETTING_DECLARATION.Vetting_Delclaration_ID);


                                //    if (objTB_DECLARATION_APPLICATION != null)
                                //    {
                                int intrstofconflict = Getconflictofinterst(item.Application_Number, item.Vetting_Meeting_ID, objScoreNew.Member_Email);
                                if (intrstofconflict == 1)
                                {

                                    {
                                        objScoreNew.Total_Score = null;
                                        objScoreNew.Go = null;
                                    }

                                    //}
                                }


                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }

                        }
                        else
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {

                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }
                        }
                        objPrsentationResultSummary.Totalscore = Math.Round(Convert.ToDecimal(objPrsentationResultSummary.Score_of_vettingmember.Sum(x => x.Total_Score)), 3);
                        objPrsentationResultSummary.Averagescore = Math.Round(Convert.ToDecimal(objPrsentationResultSummary.Score_of_vettingmember.Average(x => x.Total_Score)), 3);
                        objPrsentationResultSummary.isRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == true).Select(x => x.Go).Count();
                        objPrsentationResultSummary.isNotRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == false).Select(x => x.Go).Count();
                        objPrsentationResultSummary.totalvotes = objPrsentationResultSummary.isRecommended + objPrsentationResultSummary.isNotRecommended;
                        string result = string.Empty;
                        if (objPrsentationResultSummary.Withdraw == true)
                        {
                            result = "NA";
                        }
                        else
                        {
                            if (objPrsentationResultSummary.isRecommended > objPrsentationResultSummary.isNotRecommended)
                            {
                                result = "Recommended";
                            }
                            else if (objPrsentationResultSummary.isRecommended < objPrsentationResultSummary.isNotRecommended)
                            {
                                result = "Not Recommended";
                            }
                            else if (objPrsentationResultSummary.totalvotes == 0)
                            {
                                result = "NA";
                            }
                            else if (objPrsentationResultSummary.isRecommended == objPrsentationResultSummary.isNotRecommended)
                            {
                                result = "TBC";
                            }
                            else
                            {
                                result = "";
                            }
                        }
                        objPrsentationResultSummary.Result = result;

                        listPrsentationResultSummary.Add(objPrsentationResultSummary);
                    }
                }
                else if (objTB_VETTING_APPLICATION.FirstOrDefault().Application_Number.ToLower().Contains("ccmf"))
                {
                    foreach (TB_VETTING_APPLICATION item in objTB_VETTING_APPLICATION)
                    {
                        PrsentationResultSummary objPrsentationResultSummary = new PrsentationResultSummary();
                        objPrsentationResultSummary.Vetting_Application_ID = item.Vetting_Application_ID;
                        objPrsentationResultSummary.Application_Number = item.Application_Number;
                        objPrsentationResultSummary.PresentationTime = item.Presentation_From;

                        TB_PRESENTATION_APPLICATION_REMARKS objPresentatinRemarks = dbcontext.TB_PRESENTATION_APPLICATION_REMARKS.FirstOrDefault(k => k.Vetting_Appilcation_ID == item.Vetting_Application_ID);
                        if (objPresentatinRemarks != null)
                        {
                            objPrsentationResultSummary.Withdraw = objPresentatinRemarks.Withdraw;
                            objPrsentationResultSummary.Remark = objPresentatinRemarks.Remark;
                        }
                        else
                        {
                            objPrsentationResultSummary.Withdraw = false;
                            objPrsentationResultSummary.Remark = "";

                        }



                        //Guid Vetting_declaration_id = dbcontext.TB_VETTING_DECISION.FirstOrDefault(x => x.Application_Number == item.Application_Number).Vetting_Delclaration_ID;
                        //TB_VETTING_DECLARATION objTB_VETTING_DECLARATION = dbcontext.TB_VETTING_DECLARATION.FirstOrDefault(x => x.Vetting_Delclaration_ID == Vetting_declaration_id);
                        TB_CCMF_APPLICATION objProgram = dbcontext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Application_Number == item.Application_Number);
                        if (objProgram != null)
                        {
                            objPrsentationResultSummary.company_name = objProgram.Project_Name_Eng;
                            objPrsentationResultSummary.ProgramId = objProgram.Programme_ID;
                            objPrsentationResultSummary.Cluster = objProgram.Business_Area;
                            objPrsentationResultSummary.Programme_Type = objProgram.Programme_Type;
                            objPrsentationResultSummary.Application_Type = objProgram.CCMF_Application_Type;

                        }

                        if (dbcontext.TB_PRESENTATION_CCMF_SCORE.Where(x => x.Application_Number == item.Application_Number).Count() > 0)
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {
                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                TB_PRESENTATION_CCMF_SCORE objTbScore = dbcontext.TB_PRESENTATION_CCMF_SCORE.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Member_Email.ToLower() == objPresUser.UserData.Email.ToLower());
                                if (objTbScore != null)
                                {
                                    objScoreNew.Go = objTbScore.Go;
                                    objScoreNew.Member_Email = objTbScore.Member_Email;
                                    objScoreNew.Remarks = objTbScore.Remarks;
                                    objScoreNew.Total_Score = (Math.Round(Convert.ToDecimal(objTbScore.Total_Score), 3) * 1.0M);


                                }
                                //TB_VETTING_DECLARATION objTB_VETTING_DECLARATION = dbcontext.TB_VETTING_DECLARATION.FirstOrDefault(x => x.Member_Email == objScoreNew.Member_Email && x.Vetting_Meeting_ID == item.Vetting_Meeting_ID);
                                //if (objTB_VETTING_DECLARATION != null)
                                //{
                                //    TB_DECLARATION_APPLICATION objTB_DECLARATION_APPLICATION = new TB_DECLARATION_APPLICATION();
                                //    objTB_DECLARATION_APPLICATION = dbcontext.TB_DECLARATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Vetting_Delclaration_ID == objTB_VETTING_DECLARATION.Vetting_Delclaration_ID);


                                //    if (objTB_DECLARATION_APPLICATION != null)
                                //    {
                                int intrstofconflict = Getconflictofinterst(item.Application_Number, item.Vetting_Meeting_ID, objScoreNew.Member_Email);
                                if (intrstofconflict == 1)
                                {
                                    objScoreNew.Total_Score = null;
                                    objScoreNew.Go = null;
                                }

                                //    }
                                //}

                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }

                        }
                        else
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {

                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }
                        }

                        objPrsentationResultSummary.Totalscore = Math.Round(Convert.ToDecimal(objPrsentationResultSummary.Score_of_vettingmember.Sum(x => x.Total_Score)), 3);
                        objPrsentationResultSummary.Averagescore = Math.Round(Convert.ToDecimal(objPrsentationResultSummary.Score_of_vettingmember.Average(x => x.Total_Score)), 3);
                        objPrsentationResultSummary.isRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == true).Select(x => x.Go).Count();

                        objPrsentationResultSummary.isNotRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == false).Select(x => x.Go).Count();
                        objPrsentationResultSummary.totalrecommended = (objPrsentationResultSummary.isRecommended > objPrsentationResultSummary.isNotRecommended) ? 1 : 0;
                        objPrsentationResultSummary.totalvotes = objPrsentationResultSummary.isRecommended + objPrsentationResultSummary.isNotRecommended;

                        string result = string.Empty;
                        if (objPrsentationResultSummary.Withdraw == true)
                        {
                            result = "NA";
                        }
                        else
                        {
                            if (objPrsentationResultSummary.isRecommended > objPrsentationResultSummary.isNotRecommended)
                            {
                                result = "Recommended";
                            }
                            else if (objPrsentationResultSummary.isRecommended < objPrsentationResultSummary.isNotRecommended)
                            {
                                result = "Not Recommended";
                            }
                            else if (objPrsentationResultSummary.totalvotes == 0)
                            {
                                result = "NA";
                            }
                            else if (objPrsentationResultSummary.isRecommended == objPrsentationResultSummary.isNotRecommended)
                            {
                                result = "TBC";
                            }
                            else
                            {
                                result = "";
                            }
                        }
                        objPrsentationResultSummary.Result = result;
                        listPrsentationResultSummary.Add(objPrsentationResultSummary);
                    }




                }
            }





            return listPrsentationResultSummary.OrderBy(k => k.PresentationTime).ToList();

        }

        public static int Getconflictofinterst(string Applicanno, Guid Vettindid, string membermail)
        {
            int Conflict = 0;
            using (var dbContext = new CyberportEMS_EDM())
            {
                string sql = "";

                var connection = new SqlConnection(dbContext.Database.Connection.ConnectionString);
                connection.Open();

                try
                {
                    sql = "select top 1 oapp.Conflict_Of_Interest from TB_DECLARATION_APPLICATION " +
                       " oapp where  oapp.Vetting_Delclaration_ID = (select odapp.Vetting_Delclaration_ID from TB_VETTING_DECLARATION odapp" +
                       " where odapp.Member_Email='" + membermail + "' and odapp.Vetting_Meeting_ID='" + Vettindid + "' ) and oapp.Application_Number='" + Applicanno + "'";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandType = CommandType.Text;
                    var resultConflict = command.ExecuteScalar();

                    Conflict = Convert.ToInt32(resultConflict);
                }

                catch (Exception e)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return Conflict;
        }
        public static PrsentationResultSummary Get_Finalresultbyapplicationnumber(string Applicationumber, CyberportEMS_EDM dbcontext, List<PresentationVettingUser> objUserData, string ProgramName)
        {





            PrsentationResultSummary objPrsentationResultSummary = new PrsentationResultSummary();
            objPrsentationResultSummary.Application_Number = Applicationumber;
            if (ProgramName.ToLower().Contains("incubation"))
            {
                TB_INCUBATION_APPLICATION objProgram = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == Applicationumber);
                if (objProgram != null)
                {
                    objPrsentationResultSummary.company_name = objProgram.Company_Name_Eng;
                    objPrsentationResultSummary.ProgramId = objProgram.Programme_ID;
                    objPrsentationResultSummary.Cluster = objProgram.Business_Area;
                    objPrsentationResultSummary.Programme_Type = "";
                    objPrsentationResultSummary.Application_Type = "";

                }

                if (dbcontext.TB_PRESENTATION_INCUBATION_SCORE.Where(x => x.Application_Number == Applicationumber).Count() > 0)
                {
                    objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                    foreach (PresentationVettingUser objPresUser in objUserData)
                    {
                        Presentation_Score objScoreNew = new Presentation_Score()
                        {
                            Go = null,
                            Member_Email = string.Empty,
                            Remarks = string.Empty,
                            Total_Score = null
                        };
                        TB_PRESENTATION_INCUBATION_SCORE objTbScore = dbcontext.TB_PRESENTATION_INCUBATION_SCORE.FirstOrDefault(x => x.Application_Number == Applicationumber && x.Member_Email.ToLower() == objPresUser.UserData.Email.ToLower());
                        if (objTbScore != null)
                        {
                            objScoreNew.Go = objTbScore.Go;
                            objScoreNew.Member_Email = objTbScore.Member_Email;
                            objScoreNew.Remarks = objTbScore.Remarks;
                            objScoreNew.Total_Score = objTbScore.Total_Score;


                        }
                        objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                    }

                }
                else
                {
                    objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                    foreach (PresentationVettingUser objPresUser in objUserData)
                    {
                        Presentation_Score objScoreNew = new Presentation_Score()
                        {

                            Go = null,
                            Member_Email = string.Empty,
                            Remarks = string.Empty,
                            Total_Score = null
                        };
                        objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                    }
                }
                objPrsentationResultSummary.Totalscore = objPrsentationResultSummary.Score_of_vettingmember.Sum(x => x.Total_Score);
                objPrsentationResultSummary.Averagescore = objPrsentationResultSummary.Score_of_vettingmember.Average(x => x.Total_Score);
                objPrsentationResultSummary.isRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == true).Select(x => x.Go).Count();
                objPrsentationResultSummary.isNotRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == false).Select(x => x.Go).Count();
                objPrsentationResultSummary.totalvotes = objPrsentationResultSummary.isRecommended + objPrsentationResultSummary.isNotRecommended;

            }
            else
            {
                TB_CCMF_APPLICATION objProgram = dbcontext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Application_Number == Applicationumber);
                if (objProgram != null)
                {
                    objPrsentationResultSummary.company_name = objProgram.Project_Name_Eng;
                    objPrsentationResultSummary.ProgramId = objProgram.Programme_ID;
                    objPrsentationResultSummary.Cluster = objProgram.Business_Area;
                    objPrsentationResultSummary.Programme_Type = "";
                    objPrsentationResultSummary.Application_Type = "";

                }

                if (dbcontext.TB_PRESENTATION_CCMF_SCORE.Where(x => x.Application_Number == Applicationumber).Count() > 0)
                {
                    objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                    foreach (PresentationVettingUser objPresUser in objUserData)
                    {
                        Presentation_Score objScoreNew = new Presentation_Score()
                        {
                            Go = null,
                            Member_Email = string.Empty,
                            Remarks = string.Empty,
                            Total_Score = null
                        };
                        TB_PRESENTATION_CCMF_SCORE objTbScore = dbcontext.TB_PRESENTATION_CCMF_SCORE.FirstOrDefault(x => x.Application_Number == Applicationumber && x.Member_Email.ToLower() == objPresUser.UserData.Email.ToLower());
                        if (objTbScore != null)
                        {
                            objScoreNew.Go = objTbScore.Go;
                            objScoreNew.Member_Email = objTbScore.Member_Email;
                            objScoreNew.Remarks = objTbScore.Remarks;
                            objScoreNew.Total_Score = objTbScore.Total_Score;


                        }
                        objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                    }

                }
                else
                {
                    objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                    foreach (PresentationVettingUser objPresUser in objUserData)
                    {
                        Presentation_Score objScoreNew = new Presentation_Score()
                        {

                            Go = null,
                            Member_Email = string.Empty,
                            Remarks = string.Empty,
                            Total_Score = null
                        };
                        objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                    }
                }
                objPrsentationResultSummary.Totalscore = objPrsentationResultSummary.Score_of_vettingmember.Sum(x => x.Total_Score);
                objPrsentationResultSummary.Averagescore = objPrsentationResultSummary.Score_of_vettingmember.Average(x => x.Total_Score);
                objPrsentationResultSummary.isRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == true).Select(x => x.Go).Count();
                objPrsentationResultSummary.isNotRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == false).Select(x => x.Go).Count();
                objPrsentationResultSummary.totalvotes = objPrsentationResultSummary.isRecommended + objPrsentationResultSummary.isNotRecommended;
            }









            return objPrsentationResultSummary;

        }
        public static List<TB_APPLICATION_CONTACT_DETAIL> APPLICATION_CONTACT_DETAIL_GET(Guid ApplicationID)
        {
            using (var DBContext = new CyberportEMS_EDM())
            {

                return DBContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == ApplicationID).ToList();
            }

        }
        public static void APPLICATION_COMPANY_CORE_MEMBER_ADDUPDATE(CyberportEMS_EDM DBContext, List<TB_APPLICATION_COMPANY_CORE_MEMBER> objList, Guid ApplicationId)
        {
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCurrentList = DBContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == ApplicationId).ToList();

            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCurrentListNotExists = objCurrentList.Where(p => !objList.Any(p2 => p2.Core_Member_ID == p.Core_Member_ID && p2.Application_ID == ApplicationId)).ToList();
            DBContext.TB_APPLICATION_COMPANY_CORE_MEMBER.RemoveRange(objCurrentListNotExists.Where(x => x.Core_Member_ID != 0));
            foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objFunds in objList)
            {
                TB_APPLICATION_COMPANY_CORE_MEMBER objFundsDb = DBContext.TB_APPLICATION_COMPANY_CORE_MEMBER.FirstOrDefault(x => x.Core_Member_ID == objFunds.Core_Member_ID && x.Application_ID == ApplicationId);
                if (objFundsDb == null)
                {
                    DBContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Add(objFunds);
                }
                else
                {
                    objFundsDb.Application_ID = objFunds.Application_ID;
                    objFundsDb.Background_Information = objFunds.Background_Information;
                    objFundsDb.Bootcamp_Eligible_Number = objFunds.Bootcamp_Eligible_Number;
                    objFundsDb.HKID = objFunds.HKID;
                    objFundsDb.Masked_HKID = objFunds.Masked_HKID;
                    objFundsDb.Name = objFunds.Name;
                    objFundsDb.Position = objFunds.Position;
                    objFundsDb.Professional_Qualifications = objFunds.Professional_Qualifications;
                    objFundsDb.Programme_ID = objFunds.Programme_ID;
                    objFundsDb.Special_Achievements = objFunds.Special_Achievements;
                    objFundsDb.Working_Experiences = objFunds.Working_Experiences;
                    objFundsDb.CoreMember_Profile = objFunds.CoreMember_Profile;
                }
            }
        }
        public static TB_APPLICATION_ATTACHMENT TB_APPLICATION_ATTACHMENTGGet(Guid ApplicationId, int ProgramId)
        {
            using (var DBContext = new CyberportEMS_EDM())
            {
                return DBContext.TB_APPLICATION_ATTACHMENT.FirstOrDefault(x => x.Application_ID == ApplicationId && x.Programme_ID == ProgramId);
            }


        }
        public static List<TB_APPLICATION_ATTACHMENT> ListofTB_APPLICATION_ATTACHMENTGGet(Guid ApplicationId, int ProgramId)
        {
            using (var DBContext = new CyberportEMS_EDM())
            {
                return DBContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == ApplicationId && x.Programme_ID == ProgramId).ToList();
            }


        }
        public static void TB_APPLICATION_ATTACHMENTADDUPDATE(CyberportEMS_EDM DBContext, TB_APPLICATION_ATTACHMENT objAttachment, bool AllowMultiple = false)
        {
            TB_APPLICATION_ATTACHMENT objDataExists = DBContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objAttachment.Application_ID && x.Programme_ID == objAttachment.Programme_ID && x.Attachment_Type.ToLower() == objAttachment.Attachment_Type.ToLower()).OrderByDescending(x => x.Modified_Date).FirstOrDefault();
            if (objDataExists == null || objAttachment.Attachment_Type.ToLower().Contains("other_attachment") || objAttachment.Attachment_Type.ToLower().Contains("hk_id") || objAttachment.Attachment_Type.ToLower().Contains("student_id") || AllowMultiple == true)
            {
                DBContext.TB_APPLICATION_ATTACHMENT.Add(objAttachment);
            }
            else
            {
                objDataExists.Attachment_Path = objAttachment.Attachment_Path;
                objDataExists.Attachment_Type = objAttachment.Attachment_Type;
                objDataExists.Modified_By = objAttachment.Modified_By;
                objDataExists.Modified_Date = objAttachment.Modified_Date;
                objDataExists.Programme_ID = objAttachment.Programme_ID;
            }

        }
        public static void TB_APPLICATION_CONTACTDETAILSADDUPDATE(CyberportEMS_EDM DBContext, List<TB_APPLICATION_CONTACT_DETAIL> objList, Guid ApplicationId)
        {
            List<TB_APPLICATION_CONTACT_DETAIL> objCurrentList = DBContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == ApplicationId).ToList();

            List<TB_APPLICATION_CONTACT_DETAIL> objCurrentListNotExists = objCurrentList.Where(p => !objList.Any(p2 => p2.CONTACT_DETAILS_ID == p.CONTACT_DETAILS_ID && p2.Application_ID == ApplicationId)).ToList();
            DBContext.TB_APPLICATION_CONTACT_DETAIL.RemoveRange(objCurrentListNotExists.Where(x => x.CONTACT_DETAILS_ID != 0));

            foreach (TB_APPLICATION_CONTACT_DETAIL objFunds in objList)
            {
                TB_APPLICATION_CONTACT_DETAIL objFundsDb = DBContext.TB_APPLICATION_CONTACT_DETAIL.FirstOrDefault(x => x.CONTACT_DETAILS_ID == objFunds.CONTACT_DETAILS_ID && x.Application_ID == ApplicationId);
                if (objFundsDb == null)
                {
                    DBContext.TB_APPLICATION_CONTACT_DETAIL.Add(objFunds);
                }
                else
                {
                    objFundsDb.Application_ID = objFunds.Application_ID;
                    objFundsDb.Contact_No_Home = objFunds.Contact_No_Home;
                    objFundsDb.Contact_No_Mobile = objFunds.Contact_No_Mobile;
                    objFundsDb.Contact_No_Office = objFunds.Contact_No_Office;
                    objFundsDb.Last_Name_Eng = objFunds.Last_Name_Eng;
                    objFundsDb.First_Name_Eng = objFunds.First_Name_Eng;
                    objFundsDb.Position = objFunds.Position;
                    objFundsDb.Fax = objFunds.Fax;
                    objFundsDb.Email = objFunds.Email;
                    objFundsDb.Nationality = objFunds.Nationality;
                    objFundsDb.Mailing_Address = objFunds.Mailing_Address;
                    objFundsDb.Contact_No = objFunds.Contact_No;
                    objFundsDb.First_Name_Chi = objFunds.First_Name_Chi;
                    objFundsDb.Last_Name_Chi = objFunds.Last_Name_Chi;
                    objFundsDb.ID_Number = objFunds.ID_Number;
                    objFundsDb.Student_ID_Number = objFunds.Student_ID_Number;
                    objFundsDb.First_Name_Eng = objFunds.First_Name_Eng;
                    objFundsDb.Organisation_Name = objFunds.Organisation_Name;
                    objFundsDb.Programme_Enrolled_Eng = objFunds.Programme_Enrolled_Eng;
                    objFundsDb.Education_Institution_Eng = objFunds.Education_Institution_Eng;
                    objFundsDb.Mailing_Address = objFunds.Mailing_Address;
                    objFundsDb.Graduation_Month = objFunds.Graduation_Month;
                    objFundsDb.Graduation_Year = objFunds.Graduation_Year;
                    objFundsDb.Salutation = objFunds.Salutation;
                    objFundsDb.Nationality = objFunds.Nationality;
                    objFundsDb.Area = objFunds.Area;
                    
                }
            }
        }


        public static bool UserIsCollaborator(string ApplicationNumber, int ProgramId, string UserEmail)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                TB_APPLICATION_COLLABORATOR objTB_APPLICATION_COLLABORATOR = dbContext.TB_APPLICATION_COLLABORATOR.FirstOrDefault(x => x.Application_Number == ApplicationNumber && x.Programme_ID == ProgramId);
                if (objTB_APPLICATION_COLLABORATOR != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public static List<CASP_CompanyList> GetCompanyForUser(string strCurrentUser)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<CASP_CompanyListPrograms> objUserProg = new List<CASP_CompanyListPrograms>();
                List<TB_INCUBATION_APPLICATION> objIncubation = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Created_By == strCurrentUser && x.Awarded == true).ToList();
                objIncubation.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                {
                    ApplicationId = x.Incubation_ID,
                    ApplicationNumber = x.Application_Number,
                    ApplicationType = "CPIP"
                }));

                List<TB_CCMF_APPLICATION> objCCMF = dbContext.TB_CCMF_APPLICATION.Where(x => x.Created_By == strCurrentUser && x.Awarded == true).ToList();
                objCCMF.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                {
                    ApplicationId = x.CCMF_ID,
                    ApplicationNumber = x.Application_Number,
                    ApplicationType = "CCMF"
                }));
                List<Guid> Applications = objUserProg.Select(x => x.ApplicationId).ToList();

                List<TB_COMPANY_APPLICATION_MAP> objCompanies = dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => Applications.AsEnumerable().Contains(x.Application_ID)).ToList();

                List<CASP_CompanyList> objCASP_CompanyList = new List<CASP_CompanyList>();
                objCompanies.ForEach(x => objCASP_CompanyList.Add(
                    new CASP_CompanyList()
                    {
                        CompanyIdNumber = Convert.ToString(x.Company_Profile_ID) + ":" + x.Application_No,
                        CompanyName = x.TB_COMPANY_PROFILE_BASIC.Company_Name
                    }
                    ));

                // as user dosent have any EC Applications
                return objCASP_CompanyList;
            }
        }
        public static List<CASP_CompanyList> GetCompanyProjectNameForUser(string strCurrentUser)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<CASP_CompanyList> objCASP_CompanyList = new List<CASP_CompanyList>();
                List<TB_INCUBATION_APPLICATION> objIncubation = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Created_By == strCurrentUser && x.Awarded == true).ToList();
                objIncubation.ForEach(x => objCASP_CompanyList.Add(new CASP_CompanyList()
                {
                    CompanyIdNumber = x.Company_Name_Eng + ":" + x.Application_Number,
                    CompanyName = x.Company_Name_Eng
                }));

                List<TB_CCMF_APPLICATION> objCCMF = dbContext.TB_CCMF_APPLICATION.Where(x => x.Created_By == strCurrentUser && x.Awarded == true).ToList();
                objCCMF.ForEach(x => objCASP_CompanyList.Add(new CASP_CompanyList()
                {
                    CompanyIdNumber = x.Project_Name_Eng + ":" + x.Application_Number,
                    CompanyName = x.Project_Name_Eng
                }));
                // as user dosent have any EC Applications
                return objCASP_CompanyList;
            }
        }

        public static List<CASP_CompanyList> GetCompanyForUserCASP(string strCurrentUser)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<CASP_CompanyListPrograms> objUserProg = new List<CASP_CompanyListPrograms>();
                List<TB_INCUBATION_APPLICATION> objIncubation = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Created_By == strCurrentUser && x.Awarded == true).ToList();
                objIncubation.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                {
                    ApplicationId = x.Incubation_ID,
                    ApplicationNumber = x.Application_Number,
                    ApplicationType = "CPIP"
                }));

                List<TB_CCMF_APPLICATION> objCCMF = dbContext.TB_CCMF_APPLICATION.Where(x => x.Created_By == strCurrentUser && x.Awarded == true).ToList();
                objCCMF.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                {
                    ApplicationId = x.CCMF_ID,
                    ApplicationNumber = x.Application_Number,
                    ApplicationType = "CCMF"
                }));

                List<TB_CASP_APPLICATION> objCASP = dbContext.TB_CASP_APPLICATION.Where(x => x.Created_By == strCurrentUser && x.Status == "Completed").ToList();
                objCASP.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                {
                    ApplicationId = x.CASP_ID,
                    ApplicationNumber = x.Application_No,
                    ApplicationType = "CASP"
                }));
                List<Guid> Applications = objUserProg.Select(x => x.ApplicationId).ToList();

                List<TB_COMPANY_APPLICATION_MAP> objCompanies = dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => Applications.AsEnumerable().Contains(x.Application_ID)).ToList();

                List<CASP_CompanyList> objCASP_CompanyList = new List<CASP_CompanyList>();
                objCompanies.ForEach(x => objCASP_CompanyList.Add(
                    new CASP_CompanyList()
                    {
                        CompanyIdNumber = Convert.ToString(x.Company_Profile_ID),
                        CompanyName = x.TB_COMPANY_PROFILE_BASIC.Company_Name
                    }
                    ));

                // as user dosent have any EC Applications
                return objCASP_CompanyList;
            }
        }

        public static void REIMBURSEMENT_ITEM_ADDUPDATE(CyberportEMS_EDM DBContext, List<TB_FA_REIMBURSEMENT_ITEM> objFaList, Guid ApplicationId)
        {

            List<TB_FA_REIMBURSEMENT_ITEM> objCurrentList = DBContext.TB_FA_REIMBURSEMENT_ITEM.Where(x => x.FA_Application_ID == ApplicationId).ToList();

            List<TB_FA_REIMBURSEMENT_ITEM> objCurrentListNotExists = objCurrentList.Where(p => !objFaList.Any(p2 => p2.Item_ID == p.Item_ID && p2.FA_Application_ID == ApplicationId)).ToList();
            DBContext.TB_FA_REIMBURSEMENT_ITEM.RemoveRange(objCurrentListNotExists.Where(x => x.Item_ID != default(Guid)));

            DBContext.SaveChanges();

            foreach (TB_FA_REIMBURSEMENT_ITEM objFunds in objFaList)
            {
                TB_FA_REIMBURSEMENT_ITEM objFundsDb = DBContext.TB_FA_REIMBURSEMENT_ITEM.FirstOrDefault(x => x.Item_ID == objFunds.Item_ID && x.FA_Application_ID == ApplicationId);
                if (objFundsDb == null)
                {
                    objFunds.Item_ID = Guid.NewGuid();
                    DBContext.TB_FA_REIMBURSEMENT_ITEM.Add(objFunds);
                }
                else
                {
                    objFundsDb.Advertisement = objFunds.Advertisement;
                    objFundsDb.Amount = objFunds.Amount;
                    objFundsDb.Created_By = objFunds.Created_By;
                    objFundsDb.Created_Date = objFunds.Created_Date;
                    objFundsDb.Date = objFunds.Date;
                    objFundsDb.Description = objFunds.Description;
                    objFundsDb.Modified_By = objFunds.Modified_By;
                    objFundsDb.Modified_Date = objFunds.Modified_Date;
                }
            }
        }

        public static void REIMBURSEMENT_SALARY_ADDUPDATE(CyberportEMS_EDM DBContext, List<TB_FA_REIMBURSEMENT_SALARY> objFaList, Guid ApplicationId)
        {

            List<TB_FA_REIMBURSEMENT_SALARY> objCurrentList = DBContext.TB_FA_REIMBURSEMENT_SALARY.Where(x => x.FA_Application_ID == ApplicationId).ToList();

            List<TB_FA_REIMBURSEMENT_SALARY> objCurrentListNotExists = objCurrentList.Where(p => !objFaList.Any(p2 => p2.Salary_ID == p.Salary_ID && p2.FA_Application_ID == ApplicationId)).ToList();
            DBContext.TB_FA_REIMBURSEMENT_SALARY.RemoveRange(objCurrentListNotExists.Where(x => x.Salary_ID != default(Guid)));

            DBContext.SaveChanges();

            foreach (TB_FA_REIMBURSEMENT_SALARY objFunds in objFaList)
            {
                TB_FA_REIMBURSEMENT_SALARY objFundsDb = DBContext.TB_FA_REIMBURSEMENT_SALARY.FirstOrDefault(x => x.Salary_ID == objFunds.Salary_ID && x.FA_Application_ID == ApplicationId);
                if (objFundsDb == null)
                {
                    objFunds.Salary_ID = Guid.NewGuid();
                    DBContext.TB_FA_REIMBURSEMENT_SALARY.Add(objFunds);
                }
                else
                {
                    objFundsDb.Amount = objFunds.Amount;
                    objFundsDb.Intern_Name = objFunds.Intern_Name;
                    objFundsDb.MPF = objFunds.MPF;
                    objFundsDb.Period_From = objFunds.Period_From;
                    objFundsDb.Period_To = objFunds.Period_To;
                    objFundsDb.Tax = objFunds.Tax;
                    objFundsDb.Created_By = objFunds.Created_By;
                    objFundsDb.Created_Date = objFunds.Created_Date;
                    objFundsDb.Modified_By = objFunds.Modified_By;
                    objFundsDb.Modified_Date = objFunds.Modified_Date;
                }
            }
        }
    }
}
