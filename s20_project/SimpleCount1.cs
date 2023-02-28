using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace s20_project
{
    public class SimpleCount1
    {
        public List<Candidate> Candidates;
        public List<BallotPaper> BallotPapers;
        public int Seats;
        public double Quota;
        public double TotalSurplus;
        List<Candidate> Elected;

        public SimpleCount1(Contest contest)
        {
            Candidates = contest.Candidates;
            BallotPapers = contest.BallotPapers;
            Seats = contest.Seats;
        }

        // comment 

        public double truncateTwoPlaces(int total, int divisor)
        {
            // example 10 / 3 -->  3.33,  not 3.34
            // 10 * 100 = 1000
            // 1000 - ( 1000 % 3 ) = 999
            // 999 / 3 = 333
            // 333 / 100 = 3.33

            total *= 100;
            total -= (total % divisor);
            total /= divisor;
            double result = total / 100.00;
            return result;
        }

        public double truncateTwoPlaces(double total, int divisor)
        {
            total *= 100;
            total -= (total % divisor);
            total /= divisor;
            double result = total / 100.00;
            return result;
        }


        public string FirstCount()
        {
            string out1 = "";
            foreach (BallotPaper bp in BallotPapers)
            {
                Candidate gotVote = bp.getPreferenceOfInt(1);
                gotVote.gotAVote(1.0);
            }


            // display the candidates and their votes 
            out1 += ListCanVotes();
            out1 += "\n";

            // deem elected any candidate whose vote equals or exceeds the quota
            Elected = new List<Candidate>();

            // double totalSurplus = 0.0;

            foreach (Candidate c in Candidates)
            {
                if (c.VotesReceived >= Quota)
                {
                    double surplus = c.VotesReceived - Quota;
                    out1 += c.CandidateName + " is elected with a surplus of " + surplus;
                    out1 += "\n";
                    TotalSurplus += surplus;

                    Elected.Add(c);
                }
            }
            out1 += "\n";
            return out1;
            // deem elected any candidate whose vote equals or exceeds
            // (on very rare occasions, where this is less than the quota)
            // the total active vote, 
            // divided by one more than the number of places not yet filled,
            // up to the number of places to be filled, subject to paragraph 5.6.2.
            // ...
        }


        public bool CheckForEmpty()
        {
            if (Candidates.Count() == 0)
            {
                return true;
            }
            if (BallotPapers.Count() == 0)
            {
                return true;
            }
            return false;
        }

        public string getResults()
        {
            // set variables to zero
            foreach( Candidate c in Candidates)
            {
                c.Transfers = new List<BallotPaper>();
                c.VotesReceived = 0;
            }

            if ( CheckForEmpty() )
            {
                return "there are either zero candidates \nor zero ballotpapers";
            }

            string out1 = "";
            // seats : 2

            // Count the number of invalid papers, 
            // and subtract this from the total vote 
            // to get the total valid vote.

            // total valid vote : 10
            // BallotPapers.Count()

            // step 1 
            // quota
            // "The Quota is determined by dividing the total valid vote 
            // by one more than the number of places to be filled,
            // continuing the calculation to two decimal places, and rounding up."

            // 10 / 3 = 3.33 --> 3.34


            // Calculate the quota by dividing the total valid vote 
            // by one more than the number of places to be filled. 
            // Take the division to two decimal places. 
            // If the result is exact that is the quota. 
            // Otherwise ignore the remainder, and add 0.01.

            // don't round, truncate remainder
            Quota = truncateTwoPlaces(BallotPapers.Count(), (Seats + 1));

            // If the result is exact that is the quota.
            if (Quota != (BallotPapers.Count() / (Seats + 1)))
            {
                Quota += 0.01;
            }

            // display the seats, total votes and quota
            out1 += "seats: " + Seats + "\n";
            out1 += "total votes: " + BallotPapers.Count() + "\n";
            out1 += "quota: " + Quota + "\n\n";


            out1 += FirstCount();



            // step 2

            // If one or more candidates have surpluses, 
            // the largest of these should now be transferred. 

            // However the transfer of a surplus or surpluses 
            // is deferred and reconsidered at the next stage, 
            // if the total of such surpluses does not exceed either:

            // (a)
            // The difference between the votes of the two candidates 
            // who have the fewest votes, or

            // (b)
            // The difference between the total of the votes
            // of two or more candidates with the fewest votes 
            // who could be excluded under rule 5.2.5, 
            // and the vote of the candidate next above.

            // does the total surplus exceed 
            // the diference between the two lowest candidates?

            // total surplus
            out1 += "total surplus: " + TotalSurplus;
            out1 += "\n";

            // diiference between the two lowest candidates
            double differenceOfLowest =
                Candidates[(Candidates.Count() - 2)].VotesReceived -
                Candidates[(Candidates.Count() - 1)].VotesReceived;

            out1 += "diff of 2 lowest : " + differenceOfLowest;
            out1 += "\n";



            if (TotalSurplus > differenceOfLowest)
            {
                out1 += "total surplus exceeds the diference";
            }
            else
            {
                out1 += "total surplus does not exceed the diference";
            }
            out1 += "\n";

            out1 += "\n";
            // (b) ...

            // If one or more candidates have surpluses 
            // which have not been deferred, 
            // transfer the largest surplus. 
            // If the surpluses of two or more candidates are equal, 
            // decide which surplus to transfer by lot.

            // Transfer of a surplus

            // If a surplus arises at the first stage, 
            // select for examination all the papers 
            // which the candidate has received.

            // Examine the selected voting papers and 
            // sort them into their next available preferences 
            // for continuing candidates. 
            // Set aside as non - transferable papers 
            // any on which no next available preference is expressed.

            // find highest surplus

            Candidate highestSCandidate = Elected[0];
            foreach (Candidate c in Elected)
            {
                if (c.VotesReceived > highestSCandidate.VotesReceived)
                {
                    highestSCandidate = c;
                }
            }



            out1 += highestSCandidate.CandidateName +
                " has the highest surplus\n\n";

            // find all papers with this candidate as first
            List<BallotPaper> countPile = new List<BallotPaper>();

            /*
            foreach (BallotPaper b in BallotPapers)
            {
                if (b.Votes[0].Candidate.CandidateName.Equals(highestSCandidate.CandidateName))
                {
                    countPile.Add(b);
                }
            }
            */

            var query1 = from b in BallotPapers
                         where b.Votes[0].Candidate.CandidateName.Equals(highestSCandidate.CandidateName)
                         select b;

            foreach( var b in query1.ToList() )
            {
                countPile.Add(b);
            }

            string out2 = "";

            // Distribute them to other candidates and to non-transferable
            List<BallotPaper> nonTransferrable = new List<BallotPaper>();
            int transferablePapers = 0;
            foreach (BallotPaper b in countPile)
            {
                if (b.Votes.Count() > 1)
                {
                    // add here if 2nd candidate elected, do not give transfer
                    if (Elected.Contains(b.Votes[1].Candidate))
                    {
                        nonTransferrable.Add(b);
                        out2 +=
                            highestSCandidate.CandidateName +
                            " gave " +
                            "no transfer " + b.Votes[1].Candidate + " is elected\n";
                    }
                    else
                    {
                        b.Votes[1].Candidate.gotATransfer(b);
                        out2 +=
                            highestSCandidate.CandidateName +
                            " gave " +
                            b.Votes[1].Candidate.CandidateName +
                            " a transfer\n";
                        transferablePapers++;
                    }

                }
                else
                {
                    nonTransferrable.Add(b);
                    out2 +=
                        highestSCandidate.CandidateName +
                        " gave " +
                        "no transfer \n";
                }
            }
            out2 += "\n";
            out1 += out2;
            out2 = "";

            // Count the number of transferable papers 
            out1 += "transferrable papers: " + transferablePapers;
            out1 += "\n";

            // If this exceeds the surplus,
            // determine the transfer value by
            // dividing the surplus by the number of transferable papers
            // to two decimal places, ignoring any remainder.
            // otherwise the transfer value 
            // of each paper is its present value.

            double transferValue = 1.0;
            double thisSurplus = highestSCandidate.VotesReceived - Quota;

            if (transferablePapers > thisSurplus)
            {
                transferValue = truncateTwoPlaces(thisSurplus, transferablePapers);
            }

            out1 += "transfer value: " + transferValue;
            out1 += "\n";
            out1 += "\n";

            // Calculate the value to be credited 
            // to each candidate by 
            // multiplying the transfer value by the number of papers 


            foreach (Candidate c in Candidates)
            {
                if (!Elected.Contains(c))
                {
                    // out1 += c.CandidateName + " is not elected";
                    // out1 += "\n";
                    foreach (BallotPaper b in c.Transfers)
                    {
                        highestSCandidate.gotAVote(-1 * transferValue);
                        c.gotAVote(transferValue);
                        out1 += c.CandidateName + " receives " + transferValue;
                        out1 += "\n";
                    }
                }
            }
            out1 += "\n";

            // display the candidates and their votes 
            out1 += ListCanVotes();
            out1 += "\n";










            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";
            out1 += "\n";

            return out1;
        }




        public string ListCanVotes()
        {
            string out1 = "";
            Candidates.Sort();
            foreach (Candidate c in Candidates)
            {
                out1 += String.Format("{0} got {1} votes\n", c.CandidateName, Math.Round(c.VotesReceived, 2));
            }
            return out1;
        }
    }




}


/*


        public string getResults1()
        {
            string out1 = "";
            int[] counts = new int[Candidates.Count()];
            foreach (BallotPaper bp in BallotPapers)
            {
                Candidate gotVote = bp.getPreferenceOfInt(1);
                gotVote.gotAVote(1.0);
            }

            // seats : 2
            // total valid vote : 10

            // step 1 
            // quota
            // "The Quota is determined by dividing the total valid vote 
            // by one more than the number of places to be filled,
            // continuing the calculation to two decimal places, and rounding up."

            // 10 / 3 = 3.33 --> 3.34

            double seats = 2;

            double quota = BallotPapers.Count() / (seats + 1);
            quota = Math.Round(quota, 2); // round up some

            out1 += ListCanVotes();
            out1 += "\nquota: " + quota.ToString();
            out1 += "\n";




            // candidate with a vote of that exceeds the quota is deemed elected.
            List<Candidate> elected = new List<Candidate>();
            
            /
            foreach (Candidate c in Candidates)
            {
                if (c.VotesReceived > quota)
                {
                    out1 += '\n' + c.CandidateName + " is elected with a surplus of " + (c.VotesReceived - quota);
                    elected.Add(c);
                }
            }
            /


var query1 = from candidate in Candidates
             where candidate.VotesReceived > quota
             select candidate;

var queryResults = query1.ToList();
            foreach(var c in queryResults )
            {
                out1 += '\n' + c.CandidateName + " is elected with a surplus of " + (c.VotesReceived - quota);
                elected.Add(c);
            }
            


            // Step 2
            // if surplus is greater than the difference between the votes of the last two candidates,
            // the surplus must be transferred.
            // if it is less it won't make any difference, so do anyway...

            // elected got 5 votes, all fully filled in 
            // surplus: 1.67
            // 1.67 / 5 = .33
            // transfervalue = 0.33

            // how many of the electeds votes are transferrable, 
            // if their 2nd preference is elected,    not transferrable
            // if their 2nd preference is eliminated, not transferrable
            // if their 2nd preference is empty,      not transferrable




            foreach (Candidate c in elected)
            {
                out1 += "\n";
                out1 += DistributeCandidatesSurplus(c, BallotPapers, quota, elected, out1);
            }

            out1 += "\n\nafter step 2\n" + ListCanVotes();

            return out1;
        }




    

        public string DistributeCandidatesSurplus(Candidate c, List<BallotPaper> ballotPapers, double quota, List<Candidate> elected, string s2)
        {
            double surplus = c.VotesReceived - quota;
            double transferValue = surplus / c.VotesReceived;

            string out2 = "\nElected: " + c.CandidateName + " with " + c.VotesReceived + " votes,  tv: " + transferValue;

            foreach (BallotPaper b in ballotPapers)
            {
                out2 += "\n\tballot:\n\t" + b.ToString();
                // Votes[0].Candidate.CandidateName + "==" + c.CandidateName;
                if (b.Votes[0].Candidate.CandidateName.Equals(c.CandidateName))
                {
                    Candidate c2;
                    if (b.Votes.Count() > 1)
                    {
                        c2 = b.Votes[1].Candidate;
                        if (!elected.Contains(c2))
                        {
                            out2 += "\n\t\t" + c.CandidateName + " gives " + c2.CandidateName + " " + transferValue;
                            c2.gotAVote(transferValue);
                        }
                    }
                }
            }
            c.VotesReceived = c.VotesReceived - surplus;
            return out2;
        }

*/
