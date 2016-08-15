using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NiceHashMiner.Interfaces {
    interface IListItemCheckColorSetter {
        void LviSetColor(ref ListViewItem lvi);
    }
}
