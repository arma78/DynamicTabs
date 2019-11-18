using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Xml;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Linq;

namespace PHO_Dynamic_Tab_Web_Part.PHO_Dynamic_Tab_Web_Part
{
    // Developer: Armin Razic
    // Company: (OAHPP) Public Health Ontario
    // Year 2013
    
    
    public partial class PHO_Dynamic_Tab_Web_PartUserControl : UserControl
    {
        static string _TabFilter;
        public static string getTab
        {
            get
            {
                return _TabFilter;
            }
            set
            {
                _TabFilter = value;
            }
        }

        static uint _bID;
        public static uint getID
        {
            get
            {
                return _bID;
            }
            set
            {
                _bID = value;
            }
        }

        public void getReport()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            lblError.Visible = false;

            PHO_Dynamic_Tab_Web_Part ptw = (PHO_Dynamic_Tab_Web_Part)this.Parent;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite oSiteCollection = new SPSite(ptw.GetSiteURL))
                    {
                        using (SPWeb oWebsite = oSiteCollection.OpenWeb())
                        {                           
                            try
                            {                                
                                SPList list = oWebsite.Lists[ptw.getLibrary];
                                SPView oView = list.Views[ptw.getViewName]; 
                                SPQuery squery = new SPQuery(oView);

                                    XmlDocument groupDoc = new XmlDocument();
                                    groupDoc.LoadXml(string.Format("<Query>{0}</Query>", oView.Query));
                                    XmlElement groupBy = (XmlElement)groupDoc.SelectSingleNode("//GroupBy");   
                                    groupBy.SetAttribute("Collapse", "FALSE");                                
                                    string groupedbyFieldName = groupBy.InnerXml;   //<FieldRef Name="Color" />  trim  
                                      bool found = groupedbyFieldName.IndexOf("FALSE") != -1;
                                      if (found == true)
                                      {
                                          groupedbyFieldName = groupedbyFieldName.Remove(0, 16);
                                          groupedbyFieldName = groupedbyFieldName.Substring(0, groupedbyFieldName.Length - 22);
                                      }
                                      else
                                      {
                                          groupedbyFieldName = groupedbyFieldName.Remove(0, 16);
                                          groupedbyFieldName = groupedbyFieldName.Substring(0, groupedbyFieldName.Length - 4);
                                      }

                                     
                                
                                      var listItems = list.GetItems();
                                            
                                        // Filter the items in the results to only retain distinct items in an 2D array
                                        var k = (from SPListItem xitem in listItems select xitem[groupedbyFieldName]).Distinct().ToArray();
                                        var distinctItems = k.Where(s => s != null).ToArray();                                          

                                      

                                        SPField field = list.Fields.GetField(groupedbyFieldName);                                       
                                        SPFieldType fieldType = field.Type;                                   
                                        uint numberValues = Convert.ToUInt16(distinctItems.Count());
                                        _bID = numberValues;

                                        //TRIM GUID FROM THE MANAGED META DATA VALUES
                                        if (distinctItems.First().ToString().IndexOf("|", StringComparison.OrdinalIgnoreCase) >= 0)
                                        {
                                            for (int i = 0; i < distinctItems.Length; i++)
                                            {
                                                int index = distinctItems[i].ToString().IndexOf('|');
                                                if (index > 0)
                                                {
                                                    distinctItems[i] = distinctItems[i].ToString().Remove(index);
                                                }                                               
                                            } 
                                        }                                            
                                        
                                        if (found == true)
                                        {
                                           distinctItems = (from a in distinctItems orderby a descending select a).ToArray();
                                        }
                                        else if (found == false)
                                        {
                                          distinctItems = (from a in distinctItems orderby a ascending select a).ToArray();
                                        }


                                      if (this.ButtonPanel.Controls.Count == 0)
                                      {
                                         
                                         
                                          for (int i = 0; i <= numberValues - 1; i++)
                                          {                                          
                                              string uniqValue = SPEncode.HtmlEncode(distinctItems.GetValue(i).ToString());
                                              if (distinctItems.First().ToString().IndexOf("#", StringComparison.OrdinalIgnoreCase) >= 0)
                                              {
                                                  int index = uniqValue.IndexOf('#');
                                                  if (index > 0)
                                                  {
                                                      uniqValue = uniqValue.Remove(0, index + 1);
                                                  }
                                              }
                                              else if (distinctItems.First().ToString().IndexOf("|", StringComparison.OrdinalIgnoreCase) >= 0)
                                              {
                                                  int index = uniqValue.IndexOf('|');
                                                  if (index > 0)
                                                  {
                                                      uniqValue = uniqValue.Remove(0, index + 1);
                                                  }
                                              }
                                              Button btn = new Button();
                                              btn.ID = "Button" + i.ToString();
                                              btn.Text = uniqValue;
                                              btn.CssClass = "buttonNav";
                                              ButtonPanel.Controls.Add(btn);                                              
                                              btn.Click += new EventHandler(btn_Click);
                                              //Programmaticaly add button triggers after the button creation
                                              //<asp:AsyncPostBackTrigger runat="server" ControlID="Button1" EventName="Click"/>

                                                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                                                                     trigger.ControlID = "Button" + i.ToString();
                                                                     trigger.EventName = "Click";
                                                                this.UpdatePanel1.Triggers.Add(trigger);

                                          }

                                      }
                                     
                                      SPQuery tquery = new SPQuery();
                                      string tmp = oView.Query;

                                          if (!IsPostBack) //very first load//  
                                          {
                                              if (ptw.getDefaultValue == "")
                                              {
                                                  for (int i = 0; i < 1; i++)
                                                  {
                                                      Button btn = (Button)ButtonPanel.FindControl("Button" + i.ToString());
                                                      btn.CssClass = "buttonNav2";
                                                  }

                                                  string f = distinctItems.First().ToString();
                                                  int index = f.IndexOf('#');
                                                  if (index > 0)
                                                  {
                                                      f = f.Remove(0, index + 1);
                                                  }
                                                  tmp = "<Where><Eq><FieldRef Name='" + groupedbyFieldName + "' /><Value Type='" + fieldType + "'>" + f + "</Value></Eq></Where>" + tmp;
                                                                                                 
                                              }
                                              else
                                              {
                                                   //Change css for the selected tab
                                                  int t = Convert.ToInt16(_bID);
                                                  
                                                  for (int i = 0; i < t; i++)
                                                  { 
                                                      string r = SPEncode.HtmlEncode(distinctItems.GetValue(i).ToString());

                                                      int index = r.IndexOf('#');
                                                      if (index > 0)
                                                      {
                                                          r = r.Remove(0, index + 1);
                                                      }

                                                      if (r == ptw.getDefaultValue)
                                                      {
                                                      Button btn = (Button)ButtonPanel.FindControl("Button" + i.ToString());
                                                      btn.CssClass = "buttonNav2";
                                                      break;
                                                      }

                                                      tmp = "<Where><Eq><FieldRef Name='" + groupedbyFieldName + "' /><Value Type='" + fieldType + "'>" + r + "</Value></Eq></Where>" + tmp;
                                                       
                                                  }

                                              }
                                          }
                                          else
                                          {
                                              //not first load//
                                              tmp = "<Where><Eq><FieldRef Name='" + groupedbyFieldName + "' /><Value Type='" + fieldType + "'>" + _TabFilter + "</Value></Eq></Where>" + tmp; 
                                          }

                                          tquery.Query = tmp;
                                         
                                          SPListItemCollection item = list.GetItems(tquery);
                                          if (item.Count != 0)
                                          {
                                              GridView1.DataSource = item.GetDataTable();
                                              GridView1.DataBind();
                                          }
                                          else
                                          {
                                              lblError.Text = "No Items found";
                                              lblError.Visible = true;
                                          }    

                                }                      
                            catch (Exception ex)
                            {
                                lblError.Text = "An error occurred:" + ex.ToString();
                                lblError.Visible = true;
                            }                          
                        }                        
                    }
                });
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred:" + ex.ToString();
                lblError.Visible = true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //getReport();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            getReport();
        }

        public void btn_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var clickedButton = sender as Button;
                var clickedQuestion = clickedButton.ID;
                _TabFilter = clickedButton.Text;
                getResetCSS();
                clickedButton.CssClass = "buttonNav2";
                getReport();
            }
        }

        public void getResetCSS()
        {
                try
                {
                    foreach (Control c in ButtonPanel.Controls)
                    {
                        if (c is Button)
                        {
                            Button buttoncss = (Button)c;
                            buttoncss.CssClass = "buttonNav";                         
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "An error occurred:" + ex.ToString();
                    lblError.Visible = true;
                }
        }



   
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PHO_Dynamic_Tab_Web_Part ptw = (PHO_Dynamic_Tab_Web_Part)this.Parent;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ptw.OpenFileinNewWindow == true)
                {
                    HyperLink myHL = (HyperLink)e.Row.FindControl("HyperLink1");
                    myHL.NavigateUrl = ptw.GetSiteURL + "/" + ptw.getLibrary.Replace(" ", string.Empty) + "/" + e.Row.Cells[2].Text;
                    myHL.Target = "_blank";
                }
                else
                {
                    HyperLink myHL = (HyperLink)e.Row.FindControl("HyperLink1");
                    myHL.NavigateUrl = ptw.GetSiteURL + "/" + ptw.getLibrary.Replace(" ", string.Empty) + "/" + e.Row.Cells[2].Text;
                }
            }
        }
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView1.Columns[2].Visible = true;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView1.Columns[2].Visible = false;
            PHO_Dynamic_Tab_Web_Part ptw = (PHO_Dynamic_Tab_Web_Part)this.Parent;
            if (ptw.GetPublishedDate == false)
            {
                GridView1.Columns[1].Visible = false;
            }

        }
    }
}
