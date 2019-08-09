using System;

namespace WPF.Models
{
    public class MailBoxItemModel
    {
        public class AuthorInfoModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime Issued { get; set; }
        public AuthorInfoModel Author { get; set; }

        public MailBoxItemModel()
        {
            Author = new AuthorInfoModel();
        }
    }
}
