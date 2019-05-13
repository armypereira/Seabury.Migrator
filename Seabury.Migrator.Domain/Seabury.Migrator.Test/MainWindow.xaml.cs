using Seabury.Migrator.Contract;
using Seabury.Migrator.SqlRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Seabury.Migrator.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDataInfoRepository _IDataInfoRepository;
        public MainWindow()
        {
            InitializeComponent();
           txtDirExport.Text =   System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _IDataInfoRepository = new DataInfoRepository();
            List<string> vColunns = new List<string>();
            vColunns.Add("ID");
            vColunns.Add("Name");
            _IDataInfoRepository.ExportToXML(vColunns, "dbo.Report", 1000, "Demo.xml");
        }

        private void ExporFilter_Click(object sender, RoutedEventArgs e)
        {
            AppService.DataInfoAppService ins = new AppService.DataInfoAppService();
            List<string> vFilter = new List<string>();
            vFilter.Add("53");
            vFilter.Add("55");
            ins.ExportReportByListToXML(vFilter, 1000, "Demo.xml");
        }

        private void ExPortTable_Click(object sender, RoutedEventArgs e)
        {
            AppService.DataInfoAppService ins = new AppService.DataInfoAppService();
            Dictionary<string, string> vExport = new Dictionary<string, string>();
            vExport.Add("QueryID", "ssQueries");
            vExport.Add("QueryColumnID", "ssQueries_Columns");
            ins.ExportToXML(vExport, 2000, txtDirExport.Text);
        }

        private void ImportBT_Click(object sender, RoutedEventArgs e)
        {
            AppService.DataInfoAppService ins = new AppService.DataInfoAppService();
            ins.Import(txtDirExport.Text);
        }
    }
}
