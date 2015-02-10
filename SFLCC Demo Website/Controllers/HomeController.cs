using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFLCC_Demo_Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agenda()
        {
            using (var mod = new Models.ApplicationDbContext())
            {

                mod.Configuration.AutoDetectChangesEnabled = false;
                var items = from sess in mod.Set<Models.Session>()
                                .Include("Speaker")
                                .Include("Slot.Track")
                                .Include("Slot.Slot")
                            select sess;

                var track = from sess in mod.Tracks select sess;
                var avm = new Models.AgendaViewModel()
                {
                    Sessions = items.ToList(),
                    Tracks = track.ToList()
                };

                return View(avm);
            }

        }

        [HttpPost]
        public ActionResult Agenda(string searchstring)
        {
            var mod = new Models.ApplicationDbContext();
            mod.Configuration.AutoDetectChangesEnabled = false;

            var items = from sess in mod.Set<Models.Session>()
                               .Include("Speaker")
                               .Include("Slot.Track")
                               .Include("Slot.Slot")
                        where sess.SessionName.Contains(searchstring) | sess.SessionDescription.Contains(searchstring)
                        select sess;

            var track = from sess in mod.Tracks select sess;

            var avm = new Models.AgendaViewModel()
            {
                Sessions = items.ToList(),
                Tracks = track.ToList()
            };

            return View(avm);
        }

        [HttpGet]
        public ActionResult Agenda(int? id = int.MinValue)
        {
            if (id == int.MinValue)
            {
                return Agenda();
            }
            var mod = new Models.ApplicationDbContext();
            mod.Configuration.AutoDetectChangesEnabled = false;


            var items = from sess in mod.Set<Models.Session>()
                            .Include("Speaker")
                            .Include("Slot.Track")
                            .Include("Slot.Slot")
                        where sess.Slot.Track.TrackId == id
                        select sess;
            var track = from sess in mod.Tracks select sess;

            var avm = new Models.AgendaViewModel()
            {
                Sessions = items.ToList(),
                Tracks = track.ToList()
            };

            return View(avm);
        }

        public ActionResult Speakers()
        {
            using (var mod = new Models.ApplicationDbContext())
            {

                mod.Configuration.AutoDetectChangesEnabled = false;
                var all = from speak in mod.Speakers select speak;
                return View(all.ToList());
            }
        }

        public ActionResult Speaker(int id)
        {
            using (var mod = new Models.ApplicationDbContext())
            {

                mod.Configuration.AutoDetectChangesEnabled = false;
                var all = from speak in mod.Speakers where speak.SpeakerId == id select speak;
                return View(all.First());
            }
        }

        public ActionResult Sponsors()
        {
            return View();
        }
    }
}