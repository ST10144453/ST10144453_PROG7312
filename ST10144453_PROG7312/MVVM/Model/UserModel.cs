//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    //============== Class: Form1 ==============//
    public class UserModel
    {
        //++++++++++++++ Declarations ++++++++++++++//
        public Guid userID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public bool isStaff { get; set; }
        public string profilePhoto { get; set; }
        public List<TagsModel> SelectedTags { get; set; } = new List<TagsModel>();


        //~~~~~~~~~~~~~ Methods: Default Constructor ~~~~~~~~~~~~~//
        /// <summary>
        /// The default constructor for the UserModel class.
        /// </summary>
        public UserModel()
        {
            userID = Guid.NewGuid();
        }

        //~~~~~~~~~~~~~ Methods: StaffUsers ~~~~~~~~~~~~~//
        /// <summary>
        /// The list that stores Staff users. Staff users are hard coded on purpose because no one should just be able to register as a staff member.
        /// There is only 1 staff member as I feel like that any more would be redundant. 
        /// </summary>
        public static List<UserModel> Users => new List<UserModel>
            {
                new UserModel
                {
                    userID = Guid.NewGuid(),
                    userName = "StaffUser",
                    password = "StaffPassword",
                    email = "staffuser@example.com",
                    isStaff = true,
                    profilePhoto = "pack://application:,,,/Resources/Hardcoded/ProfilePhoto/koos_pfp.jpeg",
                    SelectedTags = TagsModel.Tags // Assuming TagsModel.Tags is already populated with the available tags

                }
            };

    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
