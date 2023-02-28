using Newtonsoft.Json;
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



/*

done	 1		WPF/XAML 
done	 2		Classes/Objects 
no  	 3		Inheritance ( optional )  :: count type descend from abstract count
no		 4		Interfaces ( also optional ) 
done	 5		Sorting/Filter/Searching 

done	 6		Lists/Observable Collections 
done	 7		Event Handling 
no		 8		Working with Dates 
done	 9		Random
done	10		Github

done	11		Hand coded XAML - not drag and drop --> columns
to do	12		LINQ - connecting to a database, line 261
done	13		Additional Windows/Navigation 
done	14		JSON 
done	15		Images

maybe	16		Styles 
no		17		Data Templates 
done	18		Exception Handling/Defensive Coding  
done	19		Testing, line 27


*/

namespace s20_project
{
     
    public partial class MainWindow : Window
    {
        public Contest ContestCurrent;
        public AddCandidate w1;
        public Save         w2;
        public LoadWindow   w3;
        public AddVote      w4;

        Random r = new Random();
        public string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\USERVT\SOURCE\REPOS\ADVENTITIOUS\LAB_CODING\S20_PROJECT\DBONE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Races_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (w1 != null)
            {
                w1.Close();
            }
            if (w2 != null)
            {
                w2.Close();
            }
            if (w3 != null)
            {
                w3.Close();
            }
            if (w4 != null)
            {
                w4.Close();
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " wwww:333 " );
            ContestCurrent = new Contest( 2 );

            Txb_Seats.Text = ContestCurrent.Seats + "";

            Lsb_Candidates.ItemsSource = ContestCurrent.Candidates;
            Lsb_Votes.ItemsSource = ContestCurrent.BallotPapers;
        }



        private void doSimpleCount()
        {
            try
            {
                // MessageBox.Show("You said: " + " wwww: " + ContestCurrent.Candidates.Count );
                ContestCurrent.Seats = int.Parse( Txb_Seats.Text );
                SimpleCount1 simpleCount = new SimpleCount1( ContestCurrent );
                Txb_Results.Text = simpleCount.getResults();
            }
            catch ( Exception exc)
            {
                MessageBox.Show("error:" + " simple count " + exc.Message);
            }
        }


        private void Btn_NewContest(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("You said: " + " Btn_NewContest: " );

            int exampleNo = CmbBx_Example.SelectedIndex;

            if (exampleNo == 0)
            {
                ContestCurrent = new Contest( 2 );
            }
            if (exampleNo == 1)
            {
                ContestCurrent = ContestMaker.ExampleContest1();
            }
            if (exampleNo == 2)
            {
                ContestCurrent = ContestMaker.ExampleContest2();
            }
            if (exampleNo == 3)
            {
                ContestCurrent = ContestMaker.RandomContest1();
            }
            if (exampleNo == 4)
            {
                ContestCurrent = ContestMaker.RandomContest2();
            }

            Txb_Seats.Text = ContestCurrent.Seats + "";

            Lsb_Candidates.ItemsSource = ContestCurrent.Candidates;
            Lsb_Candidates.Items.Refresh();

            Lsb_Votes.ItemsSource = ContestCurrent.BallotPapers;
            Lsb_Votes.Items.Refresh();

            // doSimpleCount();
        }


        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " Btn_Save: " );

            if (w2 == null)
            {
                w2 = new Save( this );

                // make w1 null when window is closed
                // https://stackoverflow.com/questions/1335785/how-can-i-make-sure-only-one-wpf-window-is-open-at-a-time
                w2.Closed += (a, b) => w2 = null;
                w2.Show();
            }
        }

        private void Btn_Load(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " Btn_Load: " + "");

            if (w3 == null)
            {
                w3 = new LoadWindow(this);
                w3.Closed += (a, b) => w3 = null;
                w3.Show();
            }
        }

        private void Btn_AddCandidate(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " add candidate: " + "");
            if( w1 == null )
            {
                w1 = new AddCandidate( ContestCurrent, Lsb_Candidates );

                // make w1 null when window is closed
                // https://stackoverflow.com/questions/1335785/how-can-i-make-sure-only-one-wpf-window-is-open-at-a-time
                w1.Closed += (a, b) => w1 = null;
                w1.Show();
            }
        }
        private void Btn_RemoveCandidate(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " Btn_Remove_Candidate: " + "");

            Candidate candidate = (Candidate)Lsb_Candidates.SelectedItem;
            ContestCurrent.Candidates.Remove(candidate);
            Lsb_Candidates.Items.Refresh();
        }


        private void Btn_CastVote(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " Btn_Cast_vote: " + "");

            if (w4 == null)
            {
                w4 = new AddVote( this );

                // make w1 null when window is closed
                // https://stackoverflow.com/questions/1335785/how-can-i-make-sure-only-one-wpf-window-is-open-at-a-time
                w4.Closed += (a, b) => w4 = null;
                w4.Show();
            }
        }
        private void Btn_RemoveVote(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + "Btn_Gen_vote: " + "");

            BallotPaper b = (BallotPaper)Lsb_Votes.SelectedItem;
            ContestCurrent.BallotPapers.Remove(b);
            Lsb_Votes.Items.Refresh();
        }

        private void Btn_Count(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " Btn_Count: " + "");
            doSimpleCount();
        }
        private void Btn_Recount(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You said: " + " Btn_Recount: " + "");
            try
            {
                //DBClass.methodThree();

                ImageWindow w5 = new ImageWindow();
                w5.Show();
            }
            catch( Exception eee )
            {
                MessageBox.Show("You said: " + " Btn_Recount: " + eee.Message );

            }
            
        }

        private void Lsb_Candidates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // https://stackoverflow.com/questions/5139956/listbox-with-single-select-and-also-unselect-on-click
            if (Lsb_Candidates.SelectedItems.Count <= 1)
            {

            }

            while (Lsb_Candidates.SelectedItems.Count > 1)
            {
                Lsb_Candidates.SelectedItems.RemoveAt(0);
            }
        }

        private void Lsb_Candidates_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

    }

}
