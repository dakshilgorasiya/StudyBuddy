namespace StudyBuddy.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
    }

    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string[] Tags { get; set; }
        public string TweetData { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO Author { get; set; }
        public int OwnerFollowers { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public string[] Images { get; set; }
    }
}
