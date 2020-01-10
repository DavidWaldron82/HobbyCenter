using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;
namespace HobbyCenter.Models {
    public class ViewModel {

        public User OneUser { get; set; }
        public Hobby OneHobby {get;set;}
        public UserHob OneUserHob {get;set;}
        public List<Hobby> AllHobbies {get;set;}
        List<User> AllUsers {get; set;}
        public List<UserHob> Enthusiasts {get;set;}

    }
}