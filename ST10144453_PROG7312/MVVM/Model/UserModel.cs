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
                userID = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                userName = "Koos_Staff1",
                password = "K00s_B0x",
                email = "Koos@staff.co.za",
                isStaff = true,
                profilePhoto = "Resources/Hardcoded/ProfilePhoto/koos_pfp.jpeg" //Profile photo is hardcoded into the project because HE is :) 
            }
        };

    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
