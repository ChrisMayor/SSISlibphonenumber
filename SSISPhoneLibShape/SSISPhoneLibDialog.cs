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
        public int _selectedPhoneNumberColumnLinage = -1;
        private Dictionary<string, int> _columnInfoIso;
        public int _selectedPhoneNumberIsoColumnLinage = -1;
        private bool _populatingList;
        private IDTSInputColumn100 _column;
        public Microsoft.SqlServer.Dts.Runtime.Variables Varibles { get; set; }

        public SSISPhoneLibDialog()
        {
            InitializeComponent();
        }

        private void SSISPhoneLibDialog_Load(object sender, EventArgs e)
        {
            chkInputColumn.Visible = false;
            chkInputColumn.Items.Clear();
            IDTSVirtualInput100 virtualInput = ComponentMetaData.InputCollection[0].GetVirtualInput();




            _populatingList = true;

            PopulatePhoneNumberColumn(virtualInput);

            PopulatePhoneNumberCountryIsoColumn(virtualInput);

            if (ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberIsoCodeColumn] != null)
            {
                txtIso2Default.Text = (string)ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberIsoCodeColumn].Value;
            }

            _populatingList = false;
            chkInputColumn.Visible = true;
        }

        private void PopulatePhoneNumberCountryIsoColumn(IDTSVirtualInput100 virtualInput)
        {
            _columnInfoIso = new Dictionary<string, int>(virtualInput.VirtualInputColumnCollection.Count);

            var lineageId = new Nullable<int>();

            if (ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberIsoLinageColumn] != null && ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberIsoLinageColumn].Value != null)
            {
                lineageId = (int)ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberIsoLinageColumn].Value;
            }

            foreach (IDTSVirtualInputColumn100 virtualColumn in virtualInput.VirtualInputColumnCollection)
            {
                int itemIndex = this.chkInputColumnISO.Items.Add(virtualColumn.Name);
                if (lineageId.HasValue)
                {
                    chkInputColumnISO.SetItemChecked(itemIndex, lineageId == virtualColumn.LineageID);
                }
                _columnInfoIso.Add(virtualColumn.Name, virtualColumn.LineageID);
            }
        }

        private void PopulatePhoneNumberColumn(IDTSVirtualInput100 virtualInput)
        {
            _columnInfo = new Dictionary<string, int>(virtualInput.VirtualInputColumnCollection.Count);
            var lineageId = new Nullable<int>();

            if (ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberLinageColumn] != null && ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberLinageColumn].Value != null)
            {
                lineageId = (int)ComponentMetaData.CustomPropertyCollection[Constants.PhoneNumberLinageColumn].Value;
            }

            foreach (IDTSVirtualInputColumn100 virtualColumn in virtualInput.VirtualInputColumnCollection)
            {
                int itemIndex = this.chkInputColumn.Items.Add(virtualColumn.Name);
                if (lineageId.HasValue)
                {
                    chkInputColumn.SetItemChecked(itemIndex, lineageId == virtualColumn.LineageID);
                }
                _columnInfo.Add(virtualColumn.Name, virtualColumn.LineageID);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AddValueToCustomProperty(Constants.PhoneNumberIsoCodeColumn, txtIso2Default.Text);
            SetUsageTypes();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AddValueToCustomProperty(string propertyName, object value)
        {
            ComponentMetaData.CustomPropertyCollection[propertyName].Value = value;
        }


        private void ChkInputColumns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (_populatingList)
            //{
            //    return;
            //}

            int selectedItemLineageId = _columnInfo[(string)chkInputColumn.Items[e.Index]];
            if (e.NewValue == CheckState.Checked)
            {
                UncheckOtherItems(e, chkInputColumn, _columnInfo);
                var virtualInput = ComponentMetaData.InputCollection[0].GetVirtualInput();
                _selectedPhoneNumberColumnLinage = selectedItemLineageId;
                AddValueToCustomProperty(Constants.PhoneNumberLinageColumn, selectedItemLineageId);
            }
            else
            {
                var virtualInput = ComponentMetaData.InputCollection[0].GetVirtualInput();
                var column = ComponentMetaData.InputCollection[0].InputColumnCollection.GetInputColumnByLineageID(selectedItemLineageId);
                AddValueToCustomProperty(Constants.PhoneNumberLinageColumn, null);
                _selectedPhoneNumberColumnLinage = -1;
            }
        }

        private void UncheckOtherItems(ItemCheckEventArgs e, CheckedListBox listbox, Dictionary<string, int> columnInfo)
        {
            foreach (var column in columnInfo.Where(c => listbox.CheckedItems.Contains(c.Key)))
            {
                var index = listbox.FindStringExact(column.Key);
                if (index != e.Index) listbox.SetItemCheckState(index, CheckState.Unchecked);
            }
        }

        private void ChkInputColumnISO_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (_populatingList)
            //{
            //    return;
            //}


            int selectedItemLineageId = _columnInfo[(string)chkInputColumnISO.Items[e.Index]];

            if (e.NewValue == CheckState.Checked)
            {
                UncheckOtherItems(e, chkInputColumnISO, _columnInfoIso);
                AddValueToCustomProperty(Constants.PhoneNumberIsoLinageColumn, selectedItemLineageId);
                _selectedPhoneNumberIsoColumnLinage = selectedItemLineageId;
            }
            else
            {
                var virtualInput = ComponentMetaData.InputCollection[0].GetVirtualInput();
                AddValueToCustomProperty(Constants.PhoneNumberIsoLinageColumn, null);
                _selectedPhoneNumberIsoColumnLinage = -1;
            }

        }

        private void SetUsageTypes()
        {
            var virtualInput = ComponentMetaData.InputCollection[0].GetVirtualInput();
            foreach (IDTSVirtualInputColumn100 column in virtualInput.VirtualInputColumnCollection)
            {
                if (column.UsageType != DTSUsageType.UT_IGNORED && !new[] { _selectedPhoneNumberColumnLinage, _selectedPhoneNumberIsoColumnLinage }.Contains(column.LineageID))
                {
                    virtualInput.SetUsageType(column.LineageID, DTSUsageType.UT_IGNORED);
                }
                else if (column.UsageType == DTSUsageType.UT_IGNORED && new[] { _selectedPhoneNumberColumnLinage, _selectedPhoneNumberIsoColumnLinage }.Contains(column.LineageID))
                {
                    virtualInput.SetUsageType(column.LineageID, DTSUsageType.UT_READWRITE);
                }
            }

        }


        private void ChkInputColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Currently not used
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
