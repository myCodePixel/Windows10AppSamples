using Microsoft.WindowsAzure.Mobile.Service;

namespace Azure_Mobile_Service.DataObjects
{
    public class Book : EntityData
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }

        public bool Complete { get; set; }
    }
}