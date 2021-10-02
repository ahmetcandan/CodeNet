using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Net5Api.Model
{
    public class UserModel
    {
        public string Username { get; set; }

        public IList<string> Roles { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
    }
}
