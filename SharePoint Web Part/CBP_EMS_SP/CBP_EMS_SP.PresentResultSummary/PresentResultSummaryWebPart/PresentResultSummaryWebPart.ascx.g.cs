﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBP_EMS_SP.PresentResultSummary.PresentResultSummaryWebPart {
    using System.Web.UI.WebControls.Expressions;
    using System.Web.UI.HtmlControls;
    using System.Collections;
    using System.Text;
    using System.Web.UI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.SharePoint.WebPartPages;
    using System.Web.SessionState;
    using System.Configuration;
    using Microsoft.SharePoint;
    using System.Web;
    using System.Web.DynamicData;
    using System.Web.Caching;
    using System.Web.Profile;
    using System.ComponentModel.DataAnnotations;
    using System.Web.UI.WebControls;
    using System.Web.Security;
    using System;
    using Microsoft.SharePoint.Utilities;
    using System.Text.RegularExpressions;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint.WebControls;
    using System.CodeDom.Compiler;
    
    
    public partial class PresentResultSummaryWebPart {
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.GridView GridViewPresentation;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Button btnConfirm;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Button btnCancel;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebPartCodeGenerator", "12.0.0.0")]
        public static implicit operator global::System.Web.UI.TemplateControl(PresentResultSummaryWebPart target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control3() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "Sequence";
            @__ctrl.HeaderText = "Sequence";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control4() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "ApplicationNo";
            @__ctrl.HeaderText = "Application No.";
            @__ctrl.ItemStyle.CssClass = "textGreenColor";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control5() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "CompanyNameProjectName";
            @__ctrl.HeaderText = "CompanyName / Project Name";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control6() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "Scoreofeachvettingmember";
            @__ctrl.HeaderText = "<div>Score of each<br>vetting member</div><div class=\'hmember\'>01 02 03 04</div>";
            @__ctrl.HtmlEncode = false;
            @__ctrl.HeaderStyle.Wrap = false;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control7() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "TotalScore";
            @__ctrl.HeaderText = "Total Score";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control8() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "AverageScore";
            @__ctrl.HeaderText = "Average Score";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control9() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "GoNotGo";
            @__ctrl.HeaderText = "<div>Go / Not Go<br>choice of each<br>vetting member</div><div class=\'hmemberGoNo" +
                "tGo\'>01 02 03 04</div>";
            @__ctrl.HtmlEncode = false;
            @__ctrl.HeaderStyle.Wrap = false;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control10() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "Recommend";
            @__ctrl.HeaderText = "Recommend";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control11() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "NotRecommend";
            @__ctrl.HeaderText = "Not Recommend";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control12() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "NoofVote";
            @__ctrl.HeaderText = "No. of Vote";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control13() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "Remarks";
            @__ctrl.HeaderText = "Remarks";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControl__control2(System.Web.UI.WebControls.DataControlFieldCollection @__ctrl) {
            global::System.Web.UI.WebControls.BoundField @__ctrl1;
            @__ctrl1 = this.@__BuildControl__control3();
            @__ctrl.Add(@__ctrl1);
            global::System.Web.UI.WebControls.BoundField @__ctrl2;
            @__ctrl2 = this.@__BuildControl__control4();
            @__ctrl.Add(@__ctrl2);
            global::System.Web.UI.WebControls.BoundField @__ctrl3;
            @__ctrl3 = this.@__BuildControl__control5();
            @__ctrl.Add(@__ctrl3);
            global::System.Web.UI.WebControls.BoundField @__ctrl4;
            @__ctrl4 = this.@__BuildControl__control6();
            @__ctrl.Add(@__ctrl4);
            global::System.Web.UI.WebControls.BoundField @__ctrl5;
            @__ctrl5 = this.@__BuildControl__control7();
            @__ctrl.Add(@__ctrl5);
            global::System.Web.UI.WebControls.BoundField @__ctrl6;
            @__ctrl6 = this.@__BuildControl__control8();
            @__ctrl.Add(@__ctrl6);
            global::System.Web.UI.WebControls.BoundField @__ctrl7;
            @__ctrl7 = this.@__BuildControl__control9();
            @__ctrl.Add(@__ctrl7);
            global::System.Web.UI.WebControls.BoundField @__ctrl8;
            @__ctrl8 = this.@__BuildControl__control10();
            @__ctrl.Add(@__ctrl8);
            global::System.Web.UI.WebControls.BoundField @__ctrl9;
            @__ctrl9 = this.@__BuildControl__control11();
            @__ctrl.Add(@__ctrl9);
            global::System.Web.UI.WebControls.BoundField @__ctrl10;
            @__ctrl10 = this.@__BuildControl__control12();
            @__ctrl.Add(@__ctrl10);
            global::System.Web.UI.WebControls.BoundField @__ctrl11;
            @__ctrl11 = this.@__BuildControl__control13();
            @__ctrl.Add(@__ctrl11);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.GridView @__BuildControlGridViewPresentation() {
            global::System.Web.UI.WebControls.GridView @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.GridView();
            this.GridViewPresentation = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "GridViewPresentation";
            @__ctrl.GridLines = global::System.Web.UI.WebControls.GridLines.None;
            @__ctrl.HeaderStyle.CssClass = "textColor";
            @__ctrl.RowStyle.CssClass = "textColorBlack";
            @__ctrl.AutoGenerateColumns = false;
            @__ctrl.ShowHeaderWhenEmpty = true;
            @__ctrl.CssClass = "griidview";
            this.@__BuildControl__control2(@__ctrl.Columns);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnConfirm() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnConfirm = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnConfirm";
            @__ctrl.Text = "Confirm";
            @__ctrl.CssClass = "apply-btn blue button_width160";
            @__ctrl.ValidationGroup = "PLOA";
            @__ctrl.Click -= new System.EventHandler(this.btnConfirm_Click);
            @__ctrl.Click += new System.EventHandler(this.btnConfirm_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnCancel() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnCancel = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnCancel";
            @__ctrl.Text = "Cancel";
            @__ctrl.CssClass = "apply-btn greentheme button_width160";
            @__ctrl.ValidationGroup = "1";
            @__ctrl.Click -= new System.EventHandler(this.btnCancel_Click);
            @__ctrl.Click += new System.EventHandler(this.btnCancel_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControlTree(global::CBP_EMS_SP.PresentResultSummary.PresentResultSummaryWebPart.PresentResultSummaryWebPart @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n\r\n\r\n\r\n\r\n<style>\r\n    .outsideboard {\r\n            min-width:150px;\r\n         " +
                        "   max-width:100%;\r\n            width:auto;\r\n            padding:15px 30px 15px " +
                        "15px;            \r\n            vertical-align: middle;\r\n        }\r\n        .insi" +
                        "deboard {            \r\n            width:300px;\r\n            border:1px solid bl" +
                        "ack;\r\n        }\r\n\r\n.center {\r\n    text-align: center;    }\r\n\r\n.rTable {\r\n  \tdisp" +
                        "lay: table;\r\n  \twidth: 100%;\r\n}\r\n.rTableRow {\r\n  \tdisplay: table-row;\r\n}\r\n\r\n.rTa" +
                        "bleCell{\r\n  \tdisplay: table-cell;\r\n    padding: 5px 10px;\r\n}\r\n\r\n.ddplist {\r\n    " +
                        "min-width: 279px;\r\n}\r\n\r\n.divother{  \t    \r\n    padding: 5px 10px;\r\n}\r\n\r\n.divothe" +
                        "r-rigth {\r\n    padding: 5px 10px;\r\n}\r\n\r\n.right {\r\n    position: absolute;\r\n    r" +
                        "ight: 0px;\r\n    width: 300px;\r\n    background-color: #b0e0e6;\r\n}\r\n\r\n.txtarea {\r\n" +
                        "    max-width:100%;\r\n    min-width:80%;\r\n    max-height:250px;\r\n    min-height:1" +
                        "50px;\r\n    height:150px !important;\r\n}\r\n.griidview th, .griidview td {\r\n    padd" +
                        "ing: 10px;\r\n    text-align: left;\r\n    \r\n}\r\n.griidview{\r\n    width:100%;\r\n    bo" +
                        "rder: 1px solid #e0dede;\r\n}\r\n.temptext {\r\n    color:white;\r\n}\r\n\r\n.btntext {\r\n   " +
                        " color:black;\r\n}\r\n.divMargin {\r\n    margin-top: 24px;\r\n    margin-bottom: 22px;\r" +
                        "\n}\r\n.radio{\r\n    position: relative;\r\n    top: 10px;\r\n}\r\n.radio td{\r\n     paddin" +
                        "g-right: 25px;\r\n}\r\n.radio td input{\r\n    margin-top: 10px;\r\n}\r\n\r\n.blue{\r\n    bac" +
                        "kground-color:#58a1e4 !important;\r\n    cursor:pointer;\r\n}\r\n.table1 .rTableCell{\r" +
                        "\n    padding: 6px 95px 6px 10px;\r\n}\r\n.table2 .rTableCell{\r\n    padding-right: 15" +
                        "1px;\r\n}\r\n.blue1{\r\n    color:#0072C6;\r\n}\r\n.margindiv{\r\n    margin-bottom:5px;\r\n}\r" +
                        "\n.Attenddiv{\r\n     margin-bottom: -2px;\r\n     padding-left: 7px;\r\n}\r\n.FileUpload" +
                        "Info{\r\n    padding-right: 209px;\r\n    position: relative;\r\n    top: 21px;\r\n}\r\n.l" +
                        "eft{\r\n    text-align:left;\r\n}\r\n.textColor{\n\tcolor:#145DAA;\n    font-weight: 600;" +
                        "\n}\r\n.textColorBlack{\n\tcolor: #66666b;\n    font-weight: 500;\n}\r\n.textGreenColor{\n" +
                        "\tcolor:#80C343;\n    font-weight: 600;\n}\r\ninput.btnDeepBlue,input.btnDeepBlue:hov" +
                        "er{\r\n    margin-left: 0px;\r\n    margin-right: 15px;\r\n    background-color: #0072" +
                        "c6;\r\n    color: white;\r\n    padding: 8px 15px;\r\n    cursor: pointer;\r\n}\r\ninput.g" +
                        "reentheme{\r\n    cursor:pointer;\r\n}\r\n.hmember{\r\n    position: relative;\r\n    top:" +
                        " 25px;\r\n}\r\n.hmemberGoNotGo{\r\n    position: relative;\r\n    top: 20px;\r\n}\r\n</style" +
                        ">\r\n<div class=\"outsideboard card-theme page_main_block\" style=\"width: 100%\">\r\n  " +
                        "  <div class=\"divother\" ><h2 class=\"left textColor\">Presentation Result Summary<" +
                        "/h2></div><br />\r\n\r\n    <div class=\"divother\">\r\n        "));
            global::System.Web.UI.WebControls.GridView @__ctrl1;
            @__ctrl1 = this.@__BuildControlGridViewPresentation();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    </div>\r\n    \r\n    <div style=\"text-align: left;margin-top: 50px;\">\r\n       " +
                        " "));
            global::System.Web.UI.WebControls.Button @__ctrl2;
            @__ctrl2 = this.@__BuildControlbtnConfirm();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        "));
            global::System.Web.UI.WebControls.Button @__ctrl3;
            @__ctrl3 = this.@__BuildControlbtnCancel();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    </div> \r\n</div>"));
        }
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new global::System.EventHandler(this.Page_Load);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual object Eval(string expression) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual string Eval(string expression, string format) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}
