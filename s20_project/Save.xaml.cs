using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace s20_project
{
    /// <summary>
    /// Interaction logic for Save.xaml
    /// </summary>
    public partial class Save : Window
    {
        MainWindow MainWindow;
        public Save( MainWindow mainWindow )
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Txb_ConnectionString.Text = ""; //  MainWindow.ConnectionString;
        }

        private void Btn_SaveJson_Click(object sender, RoutedEventArgs e)
        {
            // set variables to zero
            foreach (Candidate c in MainWindow.ContestCurrent.Candidates)
            {
                c.Transfers = new List<BallotPaper>();
                c.VotesReceived = 0;
            }

            try
            {
                MainWindow.ContestCurrent.Seats = int.Parse(MainWindow.Txb_Seats.Text);

                /*
                string json = JsonConvert.SerializeObject(MainWindow.ContestCurrent);

                // https://stackoverflow.com/questions/29163755/dump-object-to-json-pretty-print-string
                JToken jt = JToken.Parse(json);
                string formattedJson = jt.ToString();
                */

                // this is the way to do what the above does
                string formattedJson = JsonConvert.SerializeObject(MainWindow.ContestCurrent, Formatting.Indented);


                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, formattedJson);

                    // MessageBox.Show("You said: " + " Btn_Save3: " + formattedJson + "  " + saveFileDialog.FileName);

                    // MainWindow.ContestCurrent = JsonConvert.DeserializeObject<Contest>(formattedJson);
                }

            }
            catch(Exception ee )
            {
                MessageBox.Show("You said: " + " Btn_Save3: " + ee.Message);
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            // IsCancel="true" 
            this.Close();
        }

        private void Btn_SaveDB_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = Txb_ConnectionString.Text;

            if ( connectionString == "")
            {
                MessageBox.Show("Connection string is blank");
            }
            else
            {
                DBClass.InsertContest(MainWindow.ContestCurrent, connectionString );
                MessageBox.Show("You said: " + " inserted: " + DBClass.rowCount + " rows");
            }
        }
    }
}
