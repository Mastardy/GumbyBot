namespace GumbyBot.Database
{
    public class Vote
    {
        public int Id { get; set; }
        public User Voter { get; set; }
        public DateTime VoteTime { get; set; }
    }
}