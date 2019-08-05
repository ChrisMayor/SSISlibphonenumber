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

        public SSISPhoneLibDialog()
        {
            InitializeComponent();
        }
    }
}
