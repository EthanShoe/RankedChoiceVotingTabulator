using System;
using System.Collections.Generic;
using System.Text;

namespace RankedChoiceVotingCalculator.Classes
{
    public class Vote
    {
        public Vote(List<string> names)
        {
            OrderPreference = new Dictionary<int, string>();

            AddPreference(names);
        }

        public Dictionary<int, string> OrderPreference { get; set; }

        public void AddPreference(List<string> names)
        {
            for (int loop = 1; loop <= names.Count; loop++)
            {
                OrderPreference.Add(loop, names[loop - 1]);
            }
        }
    }
}
