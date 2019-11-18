using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace PHO_Dynamic_Tab_Web_Part.PHO_Dynamic_Tab_Web_Part
{
    [ToolboxItemAttribute(false)]
    public class PHO_Dynamic_Tab_Web_Part : System.Web.UI.WebControls.WebParts.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/PHO_Dynamic_Tab_Web_Part/PHO_Dynamic_Tab_Web_Part/PHO_Dynamic_Tab_Web_PartUserControl.ascx";
        string _docLibraryName = string.Empty;
        string _vName = string.Empty;       
        string _myEnum = string.Empty;
        string _siteURL = string.Empty;
        string _dValue = string.Empty;
        bool _Pdate = true;
        bool _newWindow = false;
      
        

      

        [WebBrowsable(true), Category("Tab Settings"), Personalizable(PersonalizationScope.Shared),
         WebDisplayName("Please type full site URL:")]
        public string GetSiteURL
        {
            get { return _siteURL; }
            set { _siteURL = value; }
        }

        [WebBrowsable(true), Category("Tab Settings"), Personalizable(PersonalizationScope.Shared),
            WebDisplayName("Set Document Library Name: ")]
        public string getLibrary
        {
            get { return _docLibraryName; }
            set { _docLibraryName = value; }
        }
        [WebBrowsable(true), Category("Tab Settings"), Personalizable(PersonalizationScope.Shared),
            WebDisplayName("Set View Name: ")]
        public string getViewName
        {
            get { return _vName; }
            set { _vName = value; }
        }

        [WebBrowsable(true), Category("Tab Settings"), Personalizable(PersonalizationScope.Shared),
           WebDisplayName("Set Default Value on 'Page Load Event'(Optional): ")]
        public string getDefaultValue
        {
            get { return _dValue; }
            set { _dValue = value; }
        }

     
        [WebBrowsable(true), Category("Tab Settings"), Personalizable(PersonalizationScope.Shared),
         WebDisplayName("Open File in new window: ")]
        public bool OpenFileinNewWindow
        {
            get { return _newWindow; }
            set { _newWindow = value; }
        }

        [WebBrowsable(true), Category("Tab Settings"), Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Check to Show Date Published: ")]
        public bool GetPublishedDate
        {
            get { return _Pdate; }
            set { _Pdate = value; }
        }



        //protected override void OnPreRender(EventArgs e)
        //{
            //base.OnPreRender(e);

           // EnsureChildControls();
           
        //}


        protected override void CreateChildControls()
        {         
            try
            {
                 Control control = Page.LoadControl(_ascxPath);
                 Controls.Add(control);
                
            }
            catch (Exception ex)
            {
                this.Controls.Clear();
                Controls.Add(new LiteralControl("<div><ul>" + ex.Message + "</ul><div/>"));
            }

        }       
    }
}
