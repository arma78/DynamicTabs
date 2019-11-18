<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PHO_Dynamic_Tab_Web_PartUserControl.ascx.cs" Inherits="PHO_Dynamic_Tab_Web_Part.PHO_Dynamic_Tab_Web_Part.PHO_Dynamic_Tab_Web_PartUserControl" %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
     
     <ContentTemplate>
         <asp:Panel ID="ButtonPanel" runat="server"></asp:Panel>         
              
         <div class="tabcontainer">
               
         <asp:GridView ID="GridView1" CssClass="GridView1" DataKeyNames="FileLeafRef" runat="server"  
                 AutoGenerateColumns="False" EnableModelValidation="True" 
                 onrowdatabound="GridView1_RowDataBound" 
                 ondatabinding="GridView1_DataBinding" ondatabound="GridView1_DataBound" 
                >         
        <Columns>
             <asp:TemplateField HeaderText="">
             <ItemTemplate>
             <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("Title") %>'></asp:HyperLink>
             </ItemTemplate>
                 <ControlStyle BorderStyle="None" />
                 <HeaderStyle BorderStyle="None" />
                 <ItemStyle BorderStyle="None" />
             </asp:TemplateField>           
             <asp:BoundField  DataField="DateOfPublication"  DataFormatString="{0:d}">           
             <ControlStyle BorderStyle="None" />
             <FooterStyle Wrap="False" />
             <HeaderStyle BorderStyle="None" />
             <ItemStyle Wrap="False" BorderStyle="None" />
             </asp:BoundField>
             <asp:BoundField DataField="FileLeafRef" ReadOnly="True" >
             <ControlStyle BorderStyle="None" />
             <HeaderStyle BorderStyle="None" />
             <ItemStyle BorderStyle="None" />
             </asp:BoundField>
        </Columns>    
        </asp:GridView>     
     <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label> 
     </div> 
  </ContentTemplate> 
   <Triggers>
 
</Triggers> 
</asp:UpdatePanel>