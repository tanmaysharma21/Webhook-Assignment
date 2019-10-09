namespace webhookstest.Models
{
    public class CommentPayload
    {
        public string Url { get; set; }
        public int Pull_request_review_id { get; set; }
        public User User { get; set; }
        public string Body { get; set; }
    }
}
