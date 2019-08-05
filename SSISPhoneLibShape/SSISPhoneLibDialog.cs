using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSISPhoneLibShape
{
    public partial class SSISPhoneLibDialog : Form
    {
        public IDTSComponentMetaData100 ComponentMetaData { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        private Dictionary<string, int> _columnInfo;
        private bool _populatingList;
        private IDTSInputColumn100 _column;

        public SSISPhoneLibDialog()
        {
            InitializeComponent();
        }

        private void SSISPhoneLibDialog_Load(object sender, EventArgs e)
        {
            
            chkInputColumns.Visible = false;
            chkInputColumns.Items.Clear();
            IDTSVirtualInput100 virtualInput = ComponentMetaData.InputCollection[0].GetVirtualInput();


            _columnInfo = new Dictionary<string, int>(virtualInput.VirtualInputColumnCollection.Count);

            _populatingList = true;
            foreach (IDTSVirtualInputColumn100 virtualColumn in virtualInput.VirtualInputColumnCollection)
            {
                int itemIndex = this.chkInputColumns.Items.Add(virtualColumn.Name);
                chkInputColumns.SetItemChecked(itemIndex, virtualColumn.UsageType == DTSUsageType.UT_READWRITE);
                _columnInfo.Add(virtualColumn.Name, virtualColumn.LineageID);
            }

            //txtNullValuesTable.Text =
            //    ComponentMetaData.CustomPropertyCollection["CustomProperties"].Value.ToString();

            _populatingList = false;
            chkInputColumns.Visible = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (_column != null)
            {
                    //Utility.SetPropertyValue(_column, "PhoneNumberValidation", true);
    
            }
        }

        private void ChkInputColumns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_populatingList)
            {
                return;
            }

            IDTSVirtualInput100 virtualInput = ComponentMetaData.InputCollection[0].GetVirtualInput();

            int selectedItemLineageId = _columnInfo[(string)chkInputColumns.Items[e.Index]];
            if (e.NewValue == CheckState.Checked)
            {
                virtualInput.SetUsageType(selectedItemLineageId, DTSUsageType.UT_READWRITE);
                //var column = ComponentMetaData.InputCollection[0].InputColumnCollection.GetInputColumnByLineageID(selectedItemLineageId);
                //Utility.AddColumnProperties(column);
            }
            else
            {
                var column = ComponentMetaData.InputCollection[0].InputColumnCollection.GetInputColumnByLineageID(selectedItemLineageId);
                //Utility.RemoveColumnProperties(column);
                virtualInput.SetUsageType(selectedItemLineageId, DTSUsageType.UT_IGNORED);
            }
        }

        private void ChkInputColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
