using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace s20_project
{
    class DBClass
    {
        static dbOneDataSet1 db = new dbOneDataSet1();

        // static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\USERVT\SOURCE\REPOS\ADVENTITIOUS\LAB_CODING\S20_PROJECT\DBONE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static string ConnectionString;
        // string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\uservt\source\repos\adventitious\lab_coding\s20_project\dbOne.mdf;Integrated Security=True;Connect Timeout=30";
        public static int rowCount; 

        public static void InsertContest(Contest contest, string connectionString )
        {
            ConnectionString = connectionString;
            try
            {
                DropAll();
                rowCount = 0;

                InsertCandidates(contest.Candidates);
                InsertBallotPapers(contest.BallotPapers);
            }
            catch (Exception ee)
            {
                MessageBox.Show("err: " + ee.Message);
            }
        }

        public static Contest SelectContest( string connectionString )
        {
            ConnectionString = connectionString;
            Contest contest = new Contest( 2 );   // database does not record number of seats in contest...
            rowCount = 0;
            try
            {
                SelectCandidates(contest);
                SelectBallotPapers(contest);
            }
            catch( Exception ee)
            {
                MessageBox.Show("err: " + ee.Message);
            }

            return contest;
        }



        static void InsertCandidates(List<Candidate> candidates)
        {
            foreach (Candidate c in candidates)
            {
                InsertCandidate( c );
            }
        }

        static void InsertCandidate(Candidate candidate)
        {
            string sql = "insert into Candidates ([Id], [CandidateName]) values( @candidateId, @candidateName )";

            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.Parameters.Add("@candidateId", SqlDbType.Int).Value = candidate.CandidateId;
                        cmd.Parameters.Add("@candidateName", SqlDbType.NVarChar).Value = candidate.CandidateName;

                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            rowCount++;  // MessageBox.Show("Row inserted!!" + "");
                        else
                            MessageBox.Show("No row inserted");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("database ERROR:" + ex.Message);
                }
            }
        }

        static void InsertBallotPapers(List<BallotPaper> ballotPapers)
        {
            foreach (BallotPaper b in ballotPapers)
            {
                InsertBallotPaper( b );
            }
        }

        static void InsertBallotPaper( BallotPaper ballotPaper )
        {
            string sql = "insert into BallotPapers ( [Id] ) values(  @ballotPaperId )";
            
            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.Parameters.Add("@ballotPaperId", SqlDbType.Int).Value = ballotPaper.BallotPaperId;

                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            rowCount++;  // MessageBox.Show("Row inserted!!" + "");
                        else
                            MessageBox.Show("No row inserted");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("database ERROR:" + ex.Message + "qq " + ballotPaper.BallotPaperId + " ee");
                }
            }

            foreach( Vote vote in ballotPaper.Votes )
            {
                InsertVote( vote, ballotPaper.BallotPaperId );
            }
        }

        static void InsertVote(Vote vote, int ballotPaperId)
        {
            string sql = "insert into Votes ( Id, BallotPaperId, CandidateId, VotePreference ) values(  @VoteId, @BallotPaperId, @CandidateId, @VotePref )";

            using (SqlConnection cnn = new SqlConnection( ConnectionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.Parameters.Add("@VoteId", SqlDbType.Int).Value = vote.VoteId;
                        cmd.Parameters.Add("@BallotPaperId", SqlDbType.Int).Value = ballotPaperId;
                        cmd.Parameters.Add("@CandidateId", SqlDbType.Int).Value = vote.Candidate.CandidateId;
                        cmd.Parameters.Add("@VotePref", SqlDbType.Int).Value = vote.Preference;

                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            rowCount++;  // MessageBox.Show("Row inserted!!" + "");
                        else
                            MessageBox.Show("No row inserted");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("database ERROR:" + ex.Message);
                }
            }
        }

        static void DropAll()
        {
            string sql = "delete from Votes; delete from Candidates; delete from BallotPapers";
            // sql = "truncate table Votes; truncate table BallotPapers";

            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.ExecuteNonQuery();
                        // MessageBox.Show("ok");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("database ERROR:" + ex.Message);
                }
            }
        }


        static void SelectCandidates(Contest contest )
        {
            string sql = "Select CandidateName from Candidates ";

            try
            {
                using (SqlConnection cnn = new SqlConnection(ConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read() != false )
                            {
                                // MessageBox.Show("usrs: " + reader["CandidateName"] ) ; // Console.WriteLine(String.Format("{0}", reader["id"]));

                                string s = (string)reader["CandidateName"];

                                contest.AddCandidate( new Candidate( s ) );
                                rowCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("err: " + ee.Message);
            }
        }


        static void SelectBallotPapers( Contest contest )
        {
            string sql = "Select Id from BallotPapers";

            try
            {
                using (SqlConnection cnn = new SqlConnection(ConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read() != false)
                            {
                                // MessageBox.Show("usrs: " + reader["Id"]); // Console.WriteLine(String.Format("{0}", reader["id"]));

                                int ballotPaperId = (int)reader["Id"];

                                contest.AddBallotPaper(SelectVote(contest, ballotPaperId) );
                                rowCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("err: " + ee.Message);
            }
        }


        static BallotPaper SelectVote( Contest contest, int ballotPaperId )
        {
            string sql = "Select CandidateId, VotePreference from Votes where BallotPaperId = @BallotPaperId ";

            try
            {
                BallotPaper ballotPaper = new BallotPaper();

                using (SqlConnection cnn = new SqlConnection(ConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.Parameters.Add("@BallotPaperId", SqlDbType.Int).Value = ballotPaperId ;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read() != false)
                            {   
                                int candidateId = (int)reader["CandidateId"];
                                int votePreference = (int)reader["VotePreference"];

                                Candidate c = contest.GetCandidateById(candidateId);
                                
                                Vote v = new Vote( c, votePreference );
                                ballotPaper.AddVote(v);
                                rowCount++;
                            }
                        }
                    }
                }
                return ballotPaper;
            }
            catch (Exception ee)
            {
                MessageBox.Show("err: " + ee.Message);
            }
            return null;
        }






        // https://stackoverflow.com/questions/12142806/how-to-insert-records-in-database-using-c-sharp-language
        public void methodTwo()
        {
            // All the info required to reach your db. See connectionstrings.com

            // Prepare a proper parameterized query 
            string sql = "insert into Candidates ([Id], [CandidateName]) values(@first,@last)";

            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                try
                {
                    // Open the connection to the database. 
                    // This is the first critical step in the process.
                    // If we cannot reach the db then we have connectivity problems
                    cnn.Open();

                    // Prepare the command to be executed on the db
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        // Create and set the parameters values 
                        cmd.Parameters.Add("@first", SqlDbType.NVarChar).Value = 1;
                        cmd.Parameters.Add("@last", SqlDbType.NVarChar).Value = "krnt";

                        // Let's ask the db to execute the query
                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            MessageBox.Show("Row inserted!!" + "");
                        else
                            // Well this should never really happen
                            MessageBox.Show("No row inserted");

                    }
                }
                catch (Exception ex)
                {
                    // We should log the error somewhere, 
                    // for this example let's just show a message
                    MessageBox.Show("ERROR:" + ex.Message);
                }
            }

        }

        public static void methodThree()
        {
            try
            {
                // returns zero rows
                var query1 = from c in db.Candidates select c.CandidateName;

                var results = query1.ToList();

                MessageBox.Show("count " + results.Count() + "  " + results.Count);
                foreach (var r in results)
                {
                    MessageBox.Show("res: " + r);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("err: " + ee.Message);
            }
        }
    }

}

