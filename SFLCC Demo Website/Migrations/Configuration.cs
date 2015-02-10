namespace SFLCC_Demo_Website.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SFLCC_Demo_Website.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SFLCC_Demo_Website.Models.ApplicationDbContext";
        }

        protected override void Seed(SFLCC_Demo_Website.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            //CONSTRAINT [FK_dbo.Attendees_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();

            var nova = new Models.Location()
            {
                LocationName = "Nova South Eastern University",
                Address1 = "3301 College Avenue",
                City = "Fort Lauderdale-Davie",
                State = "FL",
                ZipCode = 33314
            };
            context.Locations.AddOrUpdate(
                nova
                );

            context.SaveChanges();

            var sflccevent = new Models.Event()
            {
                EventName = "South Florida Code Camp - 2015",
                EventStartDate = DateTime.Parse("02/07/2015 07:30"),
                EventEndDate = DateTime.Parse("02/07/2015 17:30"),
                Location = nova
            };
            var carlos = new Models.Building()
            {
                BuildingName = "Carl DeSantis Building",
                NumberOfFloors = 5,
                HandicapAccesible = true,
                HasElevator = 1,
                Location = nova,
                BuildingIdentifier = "CDB"
            };
            context.Buildings.AddOrUpdate(carlos
              );
            context.SaveChanges();

            //var wc = new System.Net.WebClient();
            //var agendatxt = wc.DownloadString("http://www.fladotnet.com/codecamp/Agenda.aspx");
            //var speakerstxt = wc.DownloadString("http://www.fladotnet.com/codecamp/Speakers.aspx");
            //wc.Dispose();

            //var hap = new HtmlAgilityPack.HtmlDocument();
            //hap.LoadHtml(agendatxt);

            //var speakerhap = new HtmlAgilityPack.HtmlDocument();
            //speakerhap.LoadHtml(speakerstxt);

            var hap = new HtmlAgilityPack.HtmlDocument();
            var txtReader = System.IO.File.ReadAllText("C:\\temp\\loadpage.html");
            hap.LoadHtml(txtReader);

            var speakerhap = new HtmlAgilityPack.HtmlDocument();
            var speakertxtReader = System.IO.File.ReadAllText("C:\\temp\\speakers.html");
            speakerhap.LoadHtml(speakertxtReader);



            var speakers = speakerhap.DocumentNode.SelectNodes("//table[@id='ctl00_ContentPlaceHolder1_gvspeakers']/tr[not(@*)] | //table[@id='ctl00_ContentPlaceHolder1_gvspeakers']/tr[@style='background-color:#CCCCCC;']");
            var speakerDict = new System.Collections.Generic.Dictionary<string, Models.Speaker>();
            foreach (var item in speakers)
            {
                var namecollection = item.SelectNodes(item.XPath + "//span[contains(@id, 'ctl00_ContentPlaceHolder1_gvspeakers_ctl') and contains(@id, '_SpeakerName')]");
                var imagecollection = item.SelectNodes(item.XPath + "/td[1]/img");
                var companycollection = item.SelectNodes(item.XPath + "//span[contains(@id, 'ctl00_ContentPlaceHolder1_gvspeakers_ctl') and contains(@id, 'Company')]");
                var companyimagecollection = item.SelectNodes(item.XPath + "/td[4]/img");
                var biocollection = item.SelectNodes(item.XPath + "/td[3]");

                var speakername = namecollection != null ? namecollection[0].InnerText.Trim() : "";
                var speakerimage = imagecollection != null ? imagecollection[0].Attributes["src"].Value.Trim() : "";
                var speakercompany = companycollection != null ? companycollection[0].InnerText.Trim() : "";
                var speakercompanyimage = companyimagecollection != null ? companyimagecollection[0].Attributes["src"].Value.Trim() : "";
                var speakerbio = biocollection != null ? biocollection[0].InnerText.Trim() : "";

                if (!speakerDict.ContainsKey(speakername))
                {
                    speakerDict.Add(speakername,
                        new Models.Speaker()
                        {
                            SpeakerBio = speakerbio,
                            SpeakerName = speakername,
                            SpeakerImage = speakerimage,
                            Company = speakercompany,
                            CompanyImage = speakercompanyimage
                        });
                }
            }
            foreach (var item in speakerDict)
            {
                context.Speakers.AddOrUpdate(item.Value);
            }

            context.SaveChanges();

            var tracks = hap.DocumentNode.SelectNodes("//span[@class='style8']");
            foreach (var item in tracks)
            {
                var track = item.SelectSingleNode(item.XPath + "/text()[1]").InnerText.Trim();
                string room = "";
                if (track == "Azure/Cloud")
                {
                    room = "Room 1053";
                }
                else
                {
                    room = item.SelectSingleNode(item.XPath + "/text()[2]").InnerText.Trim();
                }
                int floor = 1;
                int.TryParse(room.Split(' ')[1].Substring(0, 1), out floor);

                context.Tracks.AddOrUpdate(
                    new Models.Track()
                    {
                        TrackName = track,
                        Event = sflccevent,
                        Room = new Models.Room()
                        {
                            RoomName = room,
                            Floor = floor,
                            Bulding = carlos
                        }
                    });


            }

            //<td class="style7"><strong>
            //var slots = hap.DocumentNode.SelectNodes("//td[@class='style7']/strong[not(contains(text(),'Keynote')) and not(contains(text(),'Registration')) and not(contains(text(),'Lunch'))]");
            var slots = hap.DocumentNode.SelectNodes("//td[@class='style7']/strong");
            foreach (var item in slots)
            {
                var slotStart = item.InnerText.Split('-')[0];
                var slotStop = item.InnerText.Split('-')[1];
                var ampm = "PM";

                if (System.Threading.Thread.CurrentThread.CurrentCulture.CompareInfo.Compare(slotStart, "Wrap", System.Globalization.CompareOptions.IgnoreCase) >= 0)
                {
                    context.SessionSlots.AddOrUpdate(
                        new Models.SessionSlot()
                        {
                            SessionStart = DateTime.Parse(string.Format("02/07/2015 17:10", slotStart, ampm)),
                            SessionEnd = DateTime.Parse(string.Format("02/07/2015 18:00", slotStart))
                        });
                }
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.CompareInfo.Compare(slotStart, "Keynote", System.Globalization.CompareOptions.IgnoreCase) >= 0)
                {
                    context.SessionSlots.AddOrUpdate(
                new Models.SessionSlot()
                   {
                       SessionStart = DateTime.Parse(string.Format("02/07/2015 08:00", slotStart, ampm)),
                       SessionEnd = DateTime.Parse(string.Format("02/07/2015 8:30", slotStart))
                   });
                }
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.CompareInfo.Compare(slotStart, "Regis", System.Globalization.CompareOptions.IgnoreCase) >= 0)
                {
                    context.SessionSlots.AddOrUpdate(
                      new Models.SessionSlot()
                    {
                        SessionStart = DateTime.Parse(string.Format("02/07/2015 07:30", slotStart, ampm)),
                        SessionEnd = DateTime.Parse(string.Format("02/07/2015 08:00", slotStart))
                    });
                }
                else
                {


                    if (int.Parse(slotStart.Split(':')[0]) > 4)
                    {
                        ampm = "AM";
                    }
                    context.SessionSlots.AddOrUpdate(
                        new Models.SessionSlot()
                        {
                            SessionStart = DateTime.Parse(string.Format("02/07/2015 {0} {1}", slotStart, ampm)),
                            SessionEnd = DateTime.Parse(string.Format("02/07/2015 {0} {1}", slotStart, ampm)).AddHours(1).AddMinutes(10)
                        });
                }
            }

            context.SaveChanges();
            Models.TrackSlot[,] trackslots = new Models.TrackSlot[50, 50];
            int trackcounter = 0;
            var trackList = context.Tracks.ToList();
            var sessionLost = context.SessionSlots.ToList();
            foreach (var track in trackList)
            {
                int slotcounter = 0;
                foreach (var slot in sessionLost)
                {
                    var tslot = new Models.TrackSlot()
                    {
                        Slot = slot,
                        Track = track
                    };
                    trackslots[trackcounter, slotcounter++] = tslot;

                }
                ++trackcounter;
            }

            context.SaveChanges();

            //var nodes = hap.DocumentNode.SelectNodes(@"//div[@id='contentBody'][not(h3/text() = 'Keynote') and not(h3/text() = 'Registration') and not(h3/text() = 'Lunch Break')]");
            var nodes = hap.DocumentNode.SelectNodes(@"//div[@id='contentBody']");
            foreach (var item in nodes)
            {
                var sessiontitle = item.SelectSingleNode(item.XPath + "/h3").InnerText.Trim();
                var sessiondesc = item.SelectSingleNode(item.XPath + "/text()[2]").InnerText.Trim();
                var speakername = item.SelectSingleNode(item.XPath + "/strong").InnerText.Trim();

                var track = item.ParentNode.ParentNode.ParentNode.ParentNode;
                var trackNumber = int.Parse(track.XPath.Split('/').Last().Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).Last()) - 1;
                var slot = track.ParentNode;
                var slotNumber = int.Parse(slot.XPath.Split('/').Last().Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).Last()) - 1;


                Models.Speaker speakitem = null;
                if (speakerDict.ContainsKey(speakername))
                {
                    var speaker = from speak in context.Speakers where speak.SpeakerName == speakername select speak;
                    speakitem = speaker.First();
                }
                context.Sessions.Add(
                    new Models.Session()
                    {
                        SessionName = sessiontitle,
                        SessionDescription = sessiondesc,
                        Speaker = speakitem,
                        Slot = trackslots[trackNumber, slotNumber]
                    });
            }

            context.SaveChanges();
            //Carl DeSantis Building



        }

    }
}
