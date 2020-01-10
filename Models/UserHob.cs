using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;
namespace HobbyCenter.Models {
    public class UserHob {
        public int UserHobId {get;set;}
        public int UserId{get;set;}
        public int HobbyId{get;set;}
        public User User {get;set;}
        public Hobby Hobby {get;set;}
    }}