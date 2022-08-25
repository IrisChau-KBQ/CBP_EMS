using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Data;
using CBP_EMS_SP.Model;

namespace CBP_EMS_SP.ContactFormWebPart.ContactForm
{
    [ToolboxItemAttribute(false)]
    public partial class ContactForm : WebPart
    {
        public SPWeb mWeb;
        public SPSite mSite;
        public string mConnectionString;
        
        public ContactForm()
        {
            mWeb = SPContext.Current.Web;
            mSite = SPContext.Current.Site;
            mConnectionString = mWeb.AllProperties["ConnectionString"].ToString();
        }

        private void Refresh()
        {
            LoadContactFormView();
        }

        private void LoadContactFormResponsibleSalesDDL()
        {
            using (var db = new CBP_EMS_SP.Model.CyberportEMS_EDM())
            {
                var query = from cfrs in db.ContactFormResponsibleSales
                            orderby cfrs.ContactFormResponsibleSalesId
                            select cfrs;
                //var query = db.ContactForms.Single(c => c.id == 1).ContactFormResponsibleSales;

                ddlResponsibleSales.DataSource = query.ToList();
                ddlResponsibleSales.DataValueField = "ContactFormResponsibleSalesId"; 
                ddlResponsibleSales.DataTextField = "Name";
                ddlResponsibleSales.DataBind();
            }
        }

        private void LoadContactFormProblemTagChkBox()
        {
            using (var db = new CBP_EMS_SP.Model.CyberportEMS_EDM())
            {
                var query = from cfpt in db.ContactFormProblemTags
                            orderby cfpt.ProblemTagName
                            select cfpt;

                chkLstProblemTags.DataSource = query.ToList();
                chkLstProblemTags.DataValueField = "ContactFormProblemTagId";
                chkLstProblemTags.DataTextField = "ProblemTagName";
                chkLstProblemTags.DataBind();
            }
        }

        private void LoadContactFormView()
        {
            using (var db = new CBP_EMS_SP.Model.CyberportEMS_EDM())
            {
                var query = from cf in db.ContactForms
                            join cfrs in db.ContactFormResponsibleSales on cf.ContactFormResponsibleSalesId equals cfrs.ContactFormResponsibleSalesId
                            orderby cf.ContactFormId
                            select cf;

                RepeaterContactFormView.DataSource = query.ToList();
                RepeaterContactFormView.DataBind();
            }
        }

        public void ContactForm_SendBtn_Click(Object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                using (var db = new CBP_EMS_SP.Model.CyberportEMS_EDM())
                {

                    var newCf = new CBP_EMS_SP.Model.ContactForm
                    {
                        Name = txtName.Text,
                        Email = txtEmail.Text,
                        Phone = txtPhone.Text,
                        ContactFormResponsibleSalesId = Int32.Parse(ddlResponsibleSales.SelectedValue),
                        Inquiry = txtInquiry.Text
                    };
                    for (int i = 0; i < chkLstProblemTags.Items.Count; i++)
                    {
                        ltrContactForm.Text += chkLstProblemTags.Items[i].Selected.ToString();
                        if (chkLstProblemTags.Items[i].Selected == true)
                        {
                            var problemTag = new ContactFormProblemTag { ContactFormProblemTagId = Convert.ToInt32(chkLstProblemTags.Items[i].Value) };
                            db.ContactFormProblemTags.Attach(problemTag);

                            newCf.ContactFormProblemTags.Add(problemTag);
                        }
                    }
                    
                    db.ContactForms.Add(newCf);
                    db.SaveChanges();

                    Refresh();
                }
            }
            else
            {

            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadContactFormResponsibleSalesDDL();
                LoadContactFormProblemTagChkBox();
                LoadContactFormView();
            }
        }
    }
}
