using System.Collections.Generic;
using System.Linq;

namespace HexedSubworlds.Core.Voting
{
    public class Poll
    {
        public Dictionary<string, int> Options;
        public string Result;

        public Poll()
        {
            setDefaults();
            ClearResult();
        }

        private void setDefaults()
        {
            Options = new Dictionary<string, int>();
        }

        public int GetVotesSoFar()
        {
            int total = 0;

            foreach (int i in Options.Values)
                total += i;

            return total;
        }

        public void Start(params string[] options)
        {
            setDefaults();
            ClearResult();

            foreach (string s in options)
            {
                Options.Add(s, 0);
            }
        }

        public bool IsOption(string option) => Options.ContainsKey(option);

        public bool VoteFor(string option)
        {
            if (!IsOption(option))
                return false;

            Options[option]++;
            return true;
        }

        public void Cancel()
        {
            setDefaults();
            ClearResult();
        }

        public PollResult End()
        {
            if (Options.All((kvp) => kvp.Value <= 0))
            {
                Result = "";
                return PollResult.NoVotes;
            }

            int currentMostVotes = -1;

            foreach (KeyValuePair<string, int> kvp in Options)
            {
                if (kvp.Value > currentMostVotes)
                {
                    Result = kvp.Key;
                    currentMostVotes = kvp.Value;
                }
            }

            if (Options.Any((kvp) => kvp.Value == currentMostVotes)) // Another option in the poll has the same amount of votes as the winning option, it's a tie
                return PollResult.Tie;

            return PollResult.ResultChosen;
        }

        public void ClearResult()
        {
            Result = null;
        }
    }
}
