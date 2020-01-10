using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace HobbyCenter.Models {
    public class Hobby {
        [Key]
        public int HobbyId{get;set;}
        [Required(ErrorMessage=" Please name your hobby")]
        public string Name{get;set;}
        [Required(ErrorMessage=" Please describe your hobby")]
        public string Description{get;set;}
        public List<UserHob> Ethusiasts{get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }}