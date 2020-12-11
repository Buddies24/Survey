using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using survey.Models;

namespace survey.Controllers
{
    public class UserSurveysController : Controller
    {
        private surveyContext db = new surveyContext();

        // GET: UserSurveys
        public ActionResult Index()
        {
            return View(db.UserSurveys.ToList());
        }

        // GET: UserSurveys
        public ActionResult Results()
        {
            return View(GetSummary());
        }
        
        // GET: UserSurveys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSurvey userSurvey = db.UserSurveys.Find(id);
            if (userSurvey == null)
            {
                return HttpNotFound();
            }
            return View(userSurvey);
        }

        // GET: UserSurveys/Create
        public ActionResult Create()
        {
            ViewBag.RatingValues = GetRatingOptions();
            return View();
        }


        // POST: UserSurveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Surnames,Names,Date,ContactNumber,Age,isPizzaLover,isPastaLover,isPapNWorsLover,isChickenStirFryLover,isBeefStirFryLover,isOtherFoodLover,LikeEatingOut,LikeWatchingMovies,LikeWatchingTV,LikeListeningToRadio")] UserSurvey userSurvey)
        {
            if (ModelState.IsValid)
            {
                userSurvey.Date = DateTime.Now;
                db.UserSurveys.Add(userSurvey);
                db.SaveChanges();
                return RedirectToAction("Results");
            }
            ViewBag.RatingValues = GetRatingOptions();
            return View(userSurvey);
        }

        // GET: UserSurveys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSurvey userSurvey = db.UserSurveys.Find(id);
            if (userSurvey == null)
            {
                return HttpNotFound();
            }
            return View(userSurvey);
        }

        // POST: UserSurveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Surnames,Names,Date,ContactNumber,Age,isPizzaLover,isPastaLover,isPapNWorsLover,isChickenStirFryLover,isBeefStirFryLover,isOtherFoodLover,LikeEatingOut,LikeWatchingMovies,LikeWatchingTV,LikeListeningToRadio")] UserSurvey userSurvey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userSurvey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userSurvey);
        }

        // GET: UserSurveys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSurvey userSurvey = db.UserSurveys.Find(id);
            if (userSurvey == null)
            {
                return HttpNotFound();
            }
            return View(userSurvey);
        }

        // POST: UserSurveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserSurvey userSurvey = db.UserSurveys.Find(id);
            db.UserSurveys.Remove(userSurvey);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList GetRatingOptions()
        {
            List<SelectListItem> ratingOptions = new List<SelectListItem>();
            ratingOptions.Add(new SelectListItem() { Text = "Strongly Agree (1)", Value = "1", Selected = true});
            ratingOptions.Add(new SelectListItem() { Text = "Agree (2)", Value = "2"});
            ratingOptions.Add(new SelectListItem() { Text = "Neutral (3)", Value = "3"});
            ratingOptions.Add(new SelectListItem() { Text = "Disagree (4)", Value = "4"});
            ratingOptions.Add(new SelectListItem() { Text = "Strongly Disagree (5)", Value = "5"});
            return new SelectList(ratingOptions, "Value", "Text");
        }
        private SummaryDTO GetSummary()
        {
            var oldestAge = db.UserSurveys.Max(a => a.Age);
            var youngestAge = db.UserSurveys.Min(a => a.Age);
            var oldest = db.UserSurveys.FirstOrDefault(a=>a.Age.Equals(oldestAge));
            var youngest = db.UserSurveys.FirstOrDefault(a=>a.Age.Equals(youngestAge));

            return new SummaryDTO() {
                numberOfSurveyes = db.UserSurveys.ToList().Count,
                averageAge = GetAverage("Age"),
                oldPersonName = oldest.Equals(null) ? "" : $"{oldest.Names} {oldest.Surnames} - {oldest.Age}",
                youngPersonName = youngest.Equals(null) ? "" : $"{youngest.Names} {youngest.Surnames} - {youngest.Age}",
                percentagePeopleLikePizza = GetPercentage(db.UserSurveys.Where(a => a.isPizzaLover == true).ToList().Count),
                percentagePeopleLikePasta = GetPercentage(db.UserSurveys.Where(a => a.isPastaLover == true).ToList().Count),
                percentagePeopleLikePapWors = GetPercentage(db.UserSurveys.Where(a => a.isPapNWorsLover == true).ToList().Count),
                averageEatOut = GetAverage("EatOut"),
                averageWatchMovies = GetAverage("WatchMovies"),
                averageWatchTv = GetAverage("WatchTV"),
                averageListenRadio = GetAverage("Radio")
            };

        }
        public string GetPercentage(int numWhoLike)
        {
            double totSurveyes = db.UserSurveys.ToList().Count;
            double percentage = (numWhoLike / totSurveyes) * 100;
            return $"{Math.Round(percentage, 1).ToString()} %";
        }
        private string GetAverage(string averageFor)
        {
            var radio = 0;
            var tv = 0;
            var movies = 0;
            var eatout = 0;
            var totalSurveyes = db.UserSurveys.ToList().Count;

            if (!averageFor.Equals("Age"))
            {
                foreach (var item in db.UserSurveys)
                {
                    radio += Convert.ToInt32(item.LikeListeningToRadio);
                    tv += Convert.ToInt32(item.LikeWatchingTV);
                    movies += Convert.ToInt32(item.LikeWatchingMovies);
                    eatout += Convert.ToInt32(item.LikeEatingOut);
                }
            }
            if (averageFor.Equals("Age"))
            {
                return (db.UserSurveys.Sum(a => a.Age) /totalSurveyes).ToString();
            }
            else if (averageFor.Equals("WatchMovies"))
            {
                return (movies / totalSurveyes).ToString();
            }
            else if (averageFor.Equals("WatchTV"))
            {
                return (tv / totalSurveyes).ToString();
            }
            else if (averageFor.Equals("EatOut"))
            {
                return (eatout / totalSurveyes).ToString();
            }
            else
            {
                return (radio/totalSurveyes).ToString();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
