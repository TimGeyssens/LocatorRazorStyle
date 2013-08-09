<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Install.ascx.cs" Inherits="SeekAndDestroy.Umbraco.Installer.Install" %>
<%@ Register tagprefix="umb" assembly="controls" namespace="umbraco.uicontrols" %>
<%@ Register tagprefix="sak" assembly="SeekAndDestroy" namespace="SeekAndDestroy.Umbraco.Controls" %>

<div style="padding: 10px 10px 0;">
     
 <umb:feedback ID="Feedback1" runat="server" type="success" text="Seek and Destroy successfully installed!" Visible="false"/>
     <img src="/App_Plugins/SeekAndDestroy/SeekAndDestroyLogo.png" alt="Seek And Destroy" width="150" style="float:right"/>

     <h1 style="margin-top: 10px;">Seek And Destroy</h1>
    <umb:feedback id="feedback" runat="server" />
        <asp:panel id="pnlInstall" runat="server">
        <p>Now that you have <strong>Seek and destroy</strong> installed, you can also install the google maps datatype and geocode some of your content.</p>
    	<h2>Install google maps datatype</h2>
    	<p>To install the datatype, simply hit the button.</p>
        <asp:Button ID="btnGoogleMapsDataType" runat="server" Text="Install" OnClick="btnGoogleMapsDataType_Click" onclientclick="jQuery(this).hide(); jQuery('#installingMessage').show(); return true;"/>
        <p>
           
            <div style="display: none;" id="installingMessage">
                <umb:ProgressBar ID="ProgressBar1" runat="server" />
                <br />
                <em>&nbsp; &nbsp;Installing google maps datatype, please wait...</em><br />
            </div>
        </p>
            <h2>Geo code documents</h2>
            <h4>Type of documents to geocode</h4>
            <sak:DocumentMapperControl runat="server" ID="docmapper"></sak:DocumentMapperControl>
            <asp:Button ID="btnGeoCode" runat="server" Text="GeoCode" OnClick="btnGeoCode_Click" onclientclick="jQuery(this).hide(); jQuery('#installingMessage').show(); return true;"/>
    </asp:panel> 


    </div>

