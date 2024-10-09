using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class UserModel
    {
        public Guid userID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public bool isStaff { get; set; }
        public string profilePhoto { get; set; } 

        public UserModel()
        {
            userID = Guid.NewGuid();
        }
    }
}
