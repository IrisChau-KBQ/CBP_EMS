<%@ Assembly Name="CBP_EMS_SP.UserPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=593a5946c1ef0b7b" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserHome.aspx.cs" Inherits="CBP_EMS_SP.UserPages.Layouts.UserHome" MasterPageFile="../EMS-Public.master" %>



<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderLeft" runat="server">
    <div class="table-col" style="vertical-align: top; width: 20%;" id="divUserLeftBar" runat="server">
    <ul class="left-navigation">
        <li class="left-nav-el"><a class="active" href="#homesection">Home</a></li>
        <li class="left-nav-el"><a href="#applicationsection">My Application</a></li>
        <li class="left-nav-el"><a href="#tempsection">Temp</a></li>
    </ul>
</div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

</asp:Content>
