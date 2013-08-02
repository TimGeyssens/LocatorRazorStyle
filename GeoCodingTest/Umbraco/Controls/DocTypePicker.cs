using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeekAndDestroy.Umbraco.Controls
{
    public class DocTypePicker
    {
        public Guid ObjectGuid {
            get
            {
                return new Guid("a2cb7800-f571-4787-9638-bc48539a0efb");
            }
        }

        private System.Web.UI.WebControls.DropDownList ddl = new System.Web.UI.WebControls.DropDownList();

        private string _val = string.Empty;

        public string Value
        {
            get
            {
                return ddl.SelectedValue;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _val = value;
            }
        }

        public System.Web.UI.WebControls.WebControl RenderControl()
        {

            ddl.ID = "DocTypePicker";

            ddl.Items.Clear();
            List<KeyValuePair<String, String>> items = new List<KeyValuePair<String, String>>();

            if (ObjectGuid != Guid.Empty)
            {
                Guid[] guids = umbraco.cms.businesslogic.CMSNode.getAllUniquesFromObjectType(ObjectGuid);
                foreach (Guid g in guids)
                {
                    umbraco.cms.businesslogic.CMSNode node = new umbraco.cms.businesslogic.CMSNode(g);
                    items.Add(new KeyValuePair<string, string>(node.Id.ToString(), node.Text));
                }
            }

            items.Sort(delegate(KeyValuePair<String, String> x, KeyValuePair<String, String> y) { return x.Value.CompareTo(y.Value); });

            foreach (KeyValuePair<String, String> kv in items)
            {
                ddl.Items.Add(new System.Web.UI.WebControls.ListItem(kv.Value, kv.Key));
            }

            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Choose...", "");
            li.Selected = true;
            ddl.Items.Insert(0, li);


            ddl.SelectedValue = _val;

            return ddl;
        }
    }
}