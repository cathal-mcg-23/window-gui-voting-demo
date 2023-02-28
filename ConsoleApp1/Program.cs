using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using s20_project;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            new try1();
        }

    }

    public class try1
    {
        public Contest ContestCurrent;
        Random r = new Random();


        public try1()
        {
            new Contest( 2 );
            Console.WriteLine("qq");

            ContestCurrent = ContestMaker.ExampleContest2();

            ContestCurrent.Seats = 2; // Int32.Parse(Txb_Seats.Text);

            // Lsb_Candidates.ItemsSource = ContestCurrent.Candidates;
            // Lsb_Candidates.Items.Refresh();

            // Lsb_Votes.ItemsSource = ContestCurrent.BallotPapers;
            // Lsb_Votes.Items.Refresh();

            // doSimpleCount();


            SimpleCount1 sc = new SimpleCount1(ContestCurrent);

            // string s = sc.getResults();

            Console.WriteLine( sc.getResults() );

        }
    }
        
}
