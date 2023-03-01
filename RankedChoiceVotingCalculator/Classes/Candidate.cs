using System;
using System.Collections.Generic;
using System.Text;

namespace RankedChoiceVotingCalculator.Classes
{
    public class Candidate
    {
        public Candidate() { }

        public Candidate(string name)
        {
            Name = name;
            FirstPlaceVotes = 0;
            Position = 0;
            IsOut = false;
        }

        public string Name { get; set; }
        public int FirstPlaceVotes { get; set; }
        public int Position { get; set; }
        public bool IsOut { get; set; }

        public Candidate Clone()
        {
            return new Candidate
            {
                Name = Name,
                FirstPlaceVotes = FirstPlaceVotes,
                Position = Position,
                IsOut = IsOut
            };
        }
    }
}
