public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SubscriptionKey { get; set; }
    public bool IsSubscribed { get; set; }
    public bool IsConnected { get; set; }
    public string FcmToken { get; set; }
}