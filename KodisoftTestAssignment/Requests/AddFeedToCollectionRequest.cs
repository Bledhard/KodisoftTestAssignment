namespace KodisoftTestAssignment.Requests
{
    public class AddFeedToCollectionRequest
    {
        public string UserId { get; set; }
        public int CollectionId { get; set; }
        public int FeedId { get ; set ; }
    }
}
