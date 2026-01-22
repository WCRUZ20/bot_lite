using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOT_LITE.Estilos
{
    public static class DataGridViewExtensions
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgv, new object[] { setting });
        }
    }
}
