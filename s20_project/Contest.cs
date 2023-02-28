using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s20_project
{
    // Contest
    // ContestMaker
    // Vote
    // Candidate
    // BallotPaper


    public class Contest
    {
        public List<Candidate> Candidates = new List<Candidate>();
        public List<BallotPaper> BallotPapers = new List<BallotPaper>();
        public int Seats { get; set; }

        public Contest( int seats )
        {
            Seats = seats;
            Candidate.IdCounter = 0;
            BallotPaper.IdCounter = 0;
            Vote.IdCounter = 0;
        }

        public void AddCandidate(Candidate candidate)
        {
            Candidates.Add(candidate);
        }
        public void AddBallotPaper(BallotPaper ballotPaper)
        {
            BallotPapers.Add(ballotPaper);
        }

        public Candidate GetCandidateById(int Id)
        {
            foreach (Candidate c in Candidates)
            {
                if (c.CandidateId == Id)
                {
                    return c;
                }
            }
            return null;
        }
    }


    public class Candidate : IComparable<Candidate>
    {
        public static int IdCounter = 0;
        public string CandidateName { get; set; }
        //public int VotesReceived { get; set; }
        public double VotesReceived { get; set; }
        public int CandidateId{ get; set; }

        public List<BallotPaper> Transfers { get; set; }

        public Candidate(string candidateName)
        {
            CandidateName = candidateName;
            Transfers = new List<BallotPaper>();
            IdCounter++;
            CandidateId = IdCounter;
        }

        public override string ToString()
        {
            return CandidateName;
        }

        public void gotAVote(double voteValue)
        {
            VotesReceived += voteValue;
        }

        public void gotATransfer(BallotPaper transfer)
        {
            Transfers.Add(transfer);
        }

        public int CompareTo(Candidate that)
        {
            return that.VotesReceived.CompareTo(VotesReceived);
        }
    }

    public class BallotPaper : IComparable<BallotPaper>
    {
        public static int IdCounter = 0;
        public List<Vote> Votes = new List<Vote>();
        public int BallotPaperId { get; set; }

        public BallotPaper()
        {
            IdCounter++;
            BallotPaperId = IdCounter;
        }

        public void AddVote(Vote vote)
        {
            Votes.Add(vote);
        }

        public override string ToString()
        {
            string out1 = "";
            Votes.Sort();
            foreach (var vote in Votes)
            {
                out1 += vote.Candidate.CandidateName + ": " + vote.Preference + ", ";
            }
            out1 = out1.Remove(out1.Length - 2);
            return out1;
        }

        public Candidate getPreferenceOfInt(int preference)
        {
            foreach (Vote vote in Votes)
            {
                if (vote.Preference == preference)
                {
                    return vote.Candidate;
                }
            }
            return null;
        }

        public int CompareTo(BallotPaper that)
        {
            for (int i = 0; i < Votes.Count(); i++)
            {
                if (i < that.Votes.Count())
                {
                    int sortInt = that.Votes[i].Candidate.CandidateName.CompareTo(Votes[i].Candidate.CandidateName);

                    if (sortInt != 0)
                    {
                        return -sortInt;
                    }
                }
            }
            /*
            if( sortInt != 0 )
            {
                return 0;
            }
            */
            return 0;
        }
    }

    public class Vote : IComparable<Vote>
    {
        public static int IdCounter = 0;
        public Candidate Candidate { get; set; }
        public int Preference { get; set; }
        public int VoteId { get; set; }

        public Vote(Candidate candidate, int preference)
        {
            Candidate = candidate;
            Preference = preference;
            IdCounter++;
            VoteId = IdCounter;
        }

        public int CompareTo(Vote that)
        {
            return -that.Preference.CompareTo(Preference);
        }
    }

    // example contests
    public class ContestMaker
    {
        static Random r = new Random();
        public static Contest ExampleContest1()
        {
            Contest ContestEx = new Contest(2);

            ContestEx.AddCandidate(new Candidate("John"));
            ContestEx.AddCandidate(new Candidate("Mary"));
            ContestEx.AddCandidate(new Candidate("Dan "));

            BallotPaper b;
            b = new BallotPaper();
            b.AddVote(new Vote(ContestEx.Candidates[0], 1));
            b.AddVote(new Vote(ContestEx.Candidates[1], 2));
            b.AddVote(new Vote(ContestEx.Candidates[2], 3));

            b.Votes.Sort();
            ContestEx.AddBallotPaper(b);

            return ContestEx;
        }
        public static Contest ExampleContest2()
        {
            Contest ContestEx = new Contest(3);

            ContestEx.AddCandidate(new Candidate("John"));
            ContestEx.AddCandidate(new Candidate("Mary"));
            ContestEx.AddCandidate(new Candidate("Dan "));
            ContestEx.AddCandidate(new Candidate("Lady"));

            BallotPaper b;
            b = new BallotPaper();
            b.AddVote(new Vote(ContestEx.Candidates[0], 1));
            b.AddVote(new Vote(ContestEx.Candidates[1], 2));
            b.AddVote(new Vote(ContestEx.Candidates[2], 3));
            b.AddVote(new Vote(ContestEx.Candidates[3], 4));

            b.Votes.Sort();
            ContestEx.AddBallotPaper(b);

            return ContestEx;
        }

        public static Contest RandomContest1()
        {
            Contest ContestEx = new Contest(2);

            ContestEx.AddCandidate(new Candidate("John"));
            ContestEx.AddCandidate(new Candidate("Mary"));
            ContestEx.AddCandidate(new Candidate("Dan "));

            for (int i = 0; i < 5; i++)
            {
                AddSomeVotes(ContestEx, 3);
            }

            for (int i = 0; i < 5; i++)
            {
                AddSomeVotes(ContestEx, 2);
            }

            for (int i = 0; i < 5; i++)
            {
                AddSomeVotes(ContestEx, 1);
            }

            ContestEx.BallotPapers.Sort();

            return ContestEx;
        }

        public static Contest RandomContest2()
        {
            Contest ContestEx = new Contest(3);

            ContestEx.AddCandidate(new Candidate("John"));
            ContestEx.AddCandidate(new Candidate("Mary"));
            ContestEx.AddCandidate(new Candidate("Dan "));
            ContestEx.AddCandidate(new Candidate("Lady"));

            for (int i = 0; i < 5; i++)
            {
                AddSomeVotes(ContestEx, 4);
            }
            for (int i = 0; i < 5; i++)
            {
                AddSomeVotes(ContestEx, 3);
            }

            for (int i = 0; i < 5; i++)
            {
                AddSomeVotes(ContestEx, 2);
            }

            for (int i = 0; i < 5; i++)
            {
                AddSomeVotes(ContestEx, 1);
            }

            ContestEx.BallotPapers.Sort();

            return ContestEx;
        }

        public static void AddSomeVotes(Contest ContestEx, int prefsToAdd)
        {
            List<int> prefs = new List<int>();

            BallotPaper b;
            b = new BallotPaper();

            for (int j = 0; j < ContestEx.Candidates.Count(); j++)
            {
                prefs.Add(j);
            }

            // prefs = [ 0, 1, 2 ]

            ShuffleList(prefs);

            // prefs = [ 2, 0, 1 ]
            for (int j = 0; j < prefsToAdd; j++)
            {
                // j = 0 , 1, 2
                b.AddVote(new Vote(ContestEx.Candidates[prefs[j]], j + 1));
            }

            b.Votes.Sort();
            ContestEx.AddBallotPaper(b);
        }


        public static void ShuffleList(List<int> list)
        {
            // https://stackoverflow.com/questions/273313/randomize-a-listt
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }










}
