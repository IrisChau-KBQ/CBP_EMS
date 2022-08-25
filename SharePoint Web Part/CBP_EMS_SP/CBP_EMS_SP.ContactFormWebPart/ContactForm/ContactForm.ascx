<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactForm.ascx.cs" Inherits="CBP_EMS_SP.ContactFormWebPart.ContactForm.ContactForm" %>
<asp:Panel ID="panelContactForm" runat="server">
    <asp:Literal ID="ltrContactForm" runat="server" />
    <table class="create-contact-form-table">
        <tr>
            <th><asp:Label ID="lblName" runat="server" Text="Your Name: "></asp:Label></th>
            <td>
                <asp:TextBox ID="txtName" runat="server" ToolTip=""></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfName" 
                   runat="server" 
                   ValidationGroup="vgContactForm"
                   ControlToValidate ="txtName" 
                   Display="Dynamic"
                   ErrorMessage="Please type your name.">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th><asp:Label ID="lblEmail" runat="server" Text="Your Email: "></asp:Label></th>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" ToolTip=""></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfEmail" 
                   runat="server" 
                   ValidationGroup="vgContactForm"
                   ControlToValidate ="txtEmail" 
                   Display="Dynamic"
                   ErrorMessage="Please type your email.">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th><asp:Label ID="lblPhone" runat="server" Text="Your Phone: "></asp:Label></th>
            <td><asp:TextBox ID="txtPhone" runat="server" ToolTip=""></asp:TextBox></td>
        </tr>
        <tr>
            <th><asp:Label ID="lblResponsibleSales" runat="server" Text="Responsible Sales (One-to-one): "></asp:Label></th>
            <td>
                <asp:DropDownList ID="ddlResponsibleSales" runat="server" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True" Value="">Please select...</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfResponsibleSales" 
                   runat="server" 
                   ValidationGroup="vgContactForm"
                   ControlToValidate ="ddlResponsibleSales"
                   Display="Dynamic" 
                   ErrorMessage="Please select responsible sales.">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th><asp:Label ID="lblInquiry" runat="server" Text="Inquiry: "></asp:Label></th>
            <td><asp:TextBox ID="txtInquiry" runat="server" TextMode="multiline" Columns="50" Rows="5"/></td>
        </tr>
        <tr>
            <th><asp:Label ID="lblProblemTags" runat="server" Text="Problem Tags (Many-to-many): "></asp:Label></th>
            <td>
                <asp:CheckBoxList ID="chkLstProblemTags" runat="server" CssClass="problem-tag-checkbox-list">
                    
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:ValidationSummary runat=server ValidationGroup="vgContactForm" DisplayMode="BulletList" HeaderText="Please fill in the followings:" ShowSummary="true"/>
            </td>
        </tr>
        <tr>
            <th></th>
            <td><asp:Button ID="btnSubmit" runat="server" Text="SEND" ValidationGroup="vgContactForm" CausesValidation="true" OnClick="ContactForm_SendBtn_Click"/></td>
        </tr>
        <tr>
            <td colspan="2">
            
                <asp:Repeater ID="RepeaterContactFormView" runat="server">
                    <HeaderTemplate>
                        <table class="contact-form-table">
                            <tr>
                                <th>Name</th>
                                <th>Email</th>
                                <th>Phone</th>
                                <th>Responsible Sales</th>
                                <th>Inquiry</th>
                                <th>Problem Tags</th>
                            </tr>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Name") %></td>
                            <td><%# Eval("Email") %></td>
                            <td><%# Eval("Phone") %></td>
                            <td><%# Eval("ContactFormResponsibleSales.Name") %></td>
                            <td><%# Eval("Inquiry") %></td>
                            <td>
                                <asp:ListView ID="ProblemTagList" runat="server" DataSource='<%# Eval("ContactFormProblemTags")%>' >
                                    <ItemTemplate>
                                        <%# Eval("ProblemTagName") %>,
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>

                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
</asp:Panel>