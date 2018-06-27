namespace KodisoftTestAssignment.Requests
{
    public class AddFeedToCollectionRequest
    {
        public string UserId { get; set; }
        public int FeedCollectionId { get; set; }
        public int FeedId { get ; set ; }
    }
}
