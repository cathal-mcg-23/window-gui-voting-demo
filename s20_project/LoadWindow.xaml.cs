using Microsoft.Win32;
using Newtonsoft.Json;
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
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        MainWindow MainWindow;

        public LoadWindow(MainWindow  mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Txb_ConnectionString.Text = "";// MainWindow.ConnectionString;
        }

        private void Btn_LoadJson_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(openFileDialog.FileName);

                // MessageBox.Show("You said: " + " Btn_Save3: " + json + "  " + openFileDialog.FileName);

                try
                {
                    MainWindow.ContestCurrent = JsonConvert.DeserializeObject<Contest>(json);


                    // the json imports,
                    // but the objects are not properly related
                    // so they have to be matched by Id and added to a new list
                   
                    List<BallotPaper> ballotPapers = new List<BallotPaper>();

                    foreach( BallotPaper b in MainWindow.ContestCurrent.BallotPapers )
                    {
                        BallotPaper bNew = new BallotPaper();
                        foreach( Vote v in b.Votes )
                        {
                            Candidate c = MainWindow.ContestCurrent.GetCandidateById( v.Candidate.CandidateId );
                            Vote vNew = new Vote( c, v.Preference);
                            bNew.AddVote(vNew);
                        }
                        ballotPapers.Add(bNew);
                    }
                    MainWindow.ContestCurrent.BallotPapers = ballotPapers;

                    MainWindow.Txb_Seats.Text = MainWindow.ContestCurrent.Seats + "" ;

                    MainWindow.Lsb_Candidates.ItemsSource = MainWindow.ContestCurrent.Candidates;
                    MainWindow.Lsb_Candidates.Items.Refresh();

                    MainWindow.Lsb_Votes.ItemsSource = MainWindow.ContestCurrent.BallotPapers;
                    MainWindow.Lsb_Votes.Items.Refresh();
                }
                catch ( Exception ee )
                {
                    MessageBox.Show("" + " error decoding file: " + ee.Message );
                }
            }
        }

        private void Btn_LoadFromDb_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = Txb_ConnectionString.Text;

            if ( connectionString == "")
            {
                MessageBox.Show("Connection string is blank");
            }
            else
            {
                try
                {

                    MainWindow.ContestCurrent = DBClass.SelectContest(connectionString);
                    MainWindow.Lsb_Votes.ItemsSource = MainWindow.ContestCurrent.BallotPapers;
                    MainWindow.Lsb_Votes.Items.Refresh();

                    MainWindow.Lsb_Candidates.ItemsSource = MainWindow.ContestCurrent.Candidates;
                    MainWindow.Lsb_Candidates.Items.Refresh();

                    MessageBox.Show("You said: " + " selected: " + DBClass.rowCount + " rows");
                }
                catch (Exception eee)
                {
                    MessageBox.Show("You said: " + " Btn_LoadFromDb_Click: " + eee.Message);
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            // IsCancel="true" 
            this.Close();
        }
    }
}
