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
using System.Windows.Shapes;

namespace s20_project
{
    /// <summary>
    /// Interaction logic for AddVote.xaml
    /// </summary>
    public partial class AddVote : Window
    {
        MainWindow MainWindow;

        public int CurrentPref = 0;
        public List<Candidate> VotedCandidates = new List<Candidate>();
        readonly Random r = new Random();

        public AddVote( MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Lsb_Vote_Candidates.ItemsSource = MainWindow.ContestCurrent.Candidates;
            Lsb_Vote_Candidates.Items.Refresh();
        }

        private void Ex6Bu2tton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Lsb_Vote_Candidates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int x2 = Lsb_Vote_Candidates.SelectedIndex;
                VoteForInt(x2);
            }
            catch (Exception err)
            {
                MessageBox.Show("" + " Btn_add err1: " + err.Message);
            }
        }


        private void VoteForInt( int CanNumber)
        {
            try
            {
                Candidate x = (Candidate)Lsb_Vote_Candidates.Items.GetItemAt(  CanNumber );

                if (VotedCandidates.Contains(x) == false)
                {
                    String s = "";
                    if (CurrentPref != 0)
                    {
                        s += "\n";
                    }
                    CurrentPref++;
                    s += CurrentPref + ".  " + x.CandidateName;
                    Txb_Paper.Text += s;
                    VotedCandidates.Add(x);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("" + " Btn_add err2: " + CanNumber + " qqq " + err.Message);
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            // IsCancel="true" 
            this.Close();
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            VotedCandidates = new List<Candidate>();
            CurrentPref = 0;
            Txb_Paper.Text = "";
        }

        private void Btn_Cast_Click(object sender, RoutedEventArgs e)
        {
            if( VotedCandidates.Count() < 1 )
            {
                return;
            }

            BallotPaper b;
            b = new BallotPaper();
            int xPref = 1;
            foreach ( Candidate x in  VotedCandidates)
            {
                b.AddVote(new Vote( x, xPref ));
                xPref++;
            }

            b.Votes.Sort();
            MainWindow.ContestCurrent.AddBallotPaper(b);
            MainWindow.Lsb_Votes.Items.Refresh();
        }

        private void Btn_Random_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            for (int i = 0; i < Lsb_Vote_Candidates.Items.Count ; i++)
            {
                VoteForInt(r.Next(0, Lsb_Vote_Candidates.Items.Count));
            }
        }
    }
}
