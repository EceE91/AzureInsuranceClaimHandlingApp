using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimHandlingApp.Models
{
    public enum TypeEnum
    {
        [Display(Name = "Collision")]
        Collision = 1,
        [Display(Name = "Grounding")]
        Grounding = 2,
        [Display(Name = "Bad Weather")]
        BadWeather = 3,
        [Display(Name = "Fire")]
        Fire = 4
    }
}
