using Microsoft.SqlServer.Dts.Pipeline.Design;
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
    public partial class SSISPhoneLibUi : IDtsComponentUI
    {

        private IDTSComponentMetaData100 _cmd = null;
        private IServiceProvider _sp = null;


        public SSISPhoneLibUi()
        {
        }

        public void Delete(IWin32Window parentWindow)
        {
        }

        public bool Edit(IWin32Window parentWindow, Microsoft.SqlServer.Dts.Runtime.Variables variables, Microsoft.SqlServer.Dts.Runtime.Connections connections)
        {
            var propertiesEditor = new SSISPhoneLibDialog();
            propertiesEditor.ComponentMetaData = _cmd;
            propertiesEditor.ServiceProvider = _sp;
            ////propertiesEditor.Variables = variables;

            return propertiesEditor.ShowDialog(parentWindow) == DialogResult.OK;
        }

        public void Help(IWin32Window parentWindow)
        {

        }

        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            _cmd = dtsComponentMetadata;
            _sp = serviceProvider;
        }

        public void New(IWin32Window parentWindow)
        {
            
        }

    }
}
