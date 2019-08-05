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
            //    ComponentMetaData.CustomPropertyCollection["NullDefaultTableName"].Value.ToString();

            _populatingList = false;
            chkInputColumns.Visible = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (_column != null)
            {
                //UpdateProperties(_column);
            }
        }
    }
}
