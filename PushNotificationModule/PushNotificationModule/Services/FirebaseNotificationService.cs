using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;

public class FirebaseNotificationService
{
    public FirebaseNotificationService()
    {
        var defaultApp = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("dp-lawyersystem-task-firebase-adminsdk-5hxsw-b91df7bc84.json")
        });
    }
}
