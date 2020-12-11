using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace survey.Models
{
    public class UserSurvey
    {
        #region Personal Information
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Surname")]
        public string Surnames { get; set; }
        [Required]
        public string Names { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "Contact Number")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }
        //Minimum and Max will be set on teh view
        public int Age { get; set; } 
        #endregion

        #region Favourites

        [Display(Name = "Pizza")]
        public bool isPizzaLover { get; set; }

        [Display(Name = "Pasta")]
        public bool isPastaLover { get; set; }

        [Display(Name = "Pap and Wors")]
        public bool isPapNWorsLover { get; set; }

        [Display(Name = "Chicken stir fry")]
        public bool isChickenStirFryLover { get; set; }

        [Display(Name = "Beef stir fry")]
        public bool isBeefStirFryLover { get; set; }


        [Display(Name = "Other")]
        public bool isOtherFoodLover { get; set; }

        #endregion

        #region Ratings
        [Required(ErrorMessage = "Please select a rating for ''I like to eat out''")]
        [Display(Name = "I like to eat out")]
        public string LikeEatingOut { get; set; }
        [Required(ErrorMessage ="Please select a rating for ''I like to watch movies''")]
        [Display(Name = "I like to watch movies")]
        public string LikeWatchingMovies { get; set; }
        [Required(ErrorMessage ="Please select a rating for ''I like to watch TV''")]
        [Display(Name = "I like to watch TV")]
        public string LikeWatchingTV { get; set; }
        [Required(ErrorMessage = "Please select a rating for ''I like listening to radio''")]
        [Display(Name = "I like listening to radio")]
        public string LikeListeningToRadio { get; set; }

        #endregion
    }
}