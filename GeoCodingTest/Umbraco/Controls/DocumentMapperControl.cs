using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using umbraco.cms.businesslogic.web;
using umbraco.cms.businesslogic.propertytype;
using System.Web;

namespace SeekAndDestroy.Umbraco.Controls
{
    [ToolboxData("<{0}:DocumentMapperControl runat=server></{0}:DocumentMapperControl>")]
    public class DocumentMapperControl : WebControl, INamingContainer
    {

        public DropDownList picker = new DropDownList();
        public DropDownList locationPicker = new DropDownList();
        public Panel p_items = new Panel();

      

        private Dictionary<string, DropDownList> m_fields = new Dictionary<string, DropDownList>();


        public DocumentMapperControl()
        {

           

            EnsureChildControls();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (picker.SelectedIndex > 0)
                CreateMappers();
        }
        public string SelectedDocType
        {
            get
            {
                return picker.SelectedValue;
            }
        }

        public string SelectedAddressProperties
        {
            get
            {
                string value = string.Empty;
                foreach (KeyValuePair<string, DropDownList> kv in m_fields)
                {
                    if(kv.Value.SelectedValue != string.Empty)
                        value += kv.Value.SelectedValue + ",";


                }

                return value;
            }
        }

        public string SelectedLocationProperty
        {
            get
            {
                return locationPicker.SelectedValue;
            }
        }

       

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Control parent;
            Control container;

            UpdatePanel up = new UpdatePanel();
            container = up.ContentTemplateContainer;
            parent = up;

            //Doctype picker
            picker = (DropDownList)new DocTypePicker().RenderControl();
            picker.AutoPostBack = true;
            picker.SelectedIndexChanged += new EventHandler(picker_SelectedIndexChanged);
            picker.ID = "doctypepicker";

            locationPicker.ID = "locationpicker";
            locationPicker.AutoPostBack = true;

            p_items.CssClass = "docMapper";
            p_items.Visible = false;

            container.Controls.Add(picker);
            container.Controls.Add(p_items);
            container.Controls.Add(locationPicker);
            Controls.Add(parent);
        }


        void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            locationPicker.Items.Clear();
            CreateMappers();
        }

        private void CreateMappers()
        {
            

            p_items.Controls.Clear();
            p_items.Visible = false;

            int doctypeId = 0;
            string dt = picker.SelectedValue;

            if (int.TryParse(dt, out doctypeId))
            {
                DocumentType doctype = new DocumentType(doctypeId);

                p_items.Visible = true;

                Literal header = new Literal();
                header.Text = "<h4>Field(s) containing address</h4>";

               

                p_items.Controls.Add(header);

                locationPicker.Items.Insert(0, new ListItem("Choose...", ""));

                foreach (PropertyType pt in doctype.PropertyTypes)
                {
                    locationPicker.Items.Add(new ListItem(pt.Name, pt.Alias));

                    Panel pl = new Panel();
                    pl.CssClass = "field";
                    pl.ID = "panel_" + pt.Id.ToString();
                    pl.CssClass = "docmapping";

                  

                    DropDownList dd = new DropDownList();
                  
                    dd.ID = "dd_" + pt.Id.ToString();
                   

                    dd.Items.Insert(0, new ListItem("Choose...", ""));
                   
                    foreach (PropertyType ptt in doctype.PropertyTypes)
                    {
                        dd.Items.Add(new ListItem(ptt.Name, ptt.Alias));
                    }

                    if(!m_fields.ContainsKey(pt.Alias))
                        m_fields.Add(pt.Alias, dd);
                 
                    pl.Controls.Add(dd);

                    p_items.Controls.Add(pl);
                }

                Literal headercoor = new Literal();
                headercoor.Text = "<h4>Field to store coordinates</h4>";
                p_items.Controls.Add(headercoor);


            }

        }

    }
}