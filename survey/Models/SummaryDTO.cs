using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace survey.Models
{
    public class SummaryDTO
    {
        [Display(Name = "Total number of surveyes")]
        public int numberOfSurveyes { get; set; }
        [Display(Name ="Average age")]
        public string averageAge { get; set; }
        [Display(Name = "Oldest person  who participated in the survey")]
        public string oldPersonName { get; set; }
        [Display(Name = "Youngest person who participated in the survey")]
        public string youngPersonName { get; set; }
        [Display(Name = "Percentage of people who like  Pizza")]
        public string percentagePeopleLikePizza { get; set; }
        [Display(Name = "Percentage of people who like  Pasta")]
        public string percentagePeopleLikePasta { get; set; }
        [Display(Name = "Percentage of people who like Pap and Wors")]
        public string percentagePeopleLikePapWors { get; set; }
        [Display(Name = "People like to eat out")]
        public string averageEatOut { get; set; }
        [Display(Name = "People like to watch movies")]
        public string averageWatchMovies { get; set; }
        [Display(Name = "People like to watch TV")]
        public string averageWatchTv { get; set; }
        [Display(Name = "People like to listen to the radio")]
        public string averageListenRadio { get; set; }

    }
}