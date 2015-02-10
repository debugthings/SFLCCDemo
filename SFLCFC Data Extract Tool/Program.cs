using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFLCCExtract
{
    class Program
    {
        public class charcomp : IEqualityComparer<char>
        {

            public bool Equals(char x, char y)
            {
                return x.ToString().Equals(y.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(char obj)
            {
                return obj.GetHashCode();
            }
        }
        public struct slot
        {
            public DateTime start;
            public DateTime end;
        }

        public struct track
        {
            public string name;
        }

        public struct trackslot
        {
            public track track;
            public slot slot;
        }
        static void Main(string[] args)
        {
            var hap = new HtmlAgilityPack.HtmlDocument();
            var txtReader = System.IO.File.ReadAllText("c:\\temp\\loadpage.html");
            hap.LoadHtml(txtReader);

            var speakerhap = new HtmlAgilityPack.HtmlDocument();
            var speakertxtReader = System.IO.File.ReadAllText("c:\\temp\\speakers.html");
            speakerhap.LoadHtml(speakertxtReader);

            var speakers = speakerhap.DocumentNode.SelectNodes("//table[@id='ctl00_ContentPlaceHolder1_gvspeakers']/tr[not(@*)] | //table[@id='ctl00_ContentPlaceHolder1_gvspeakers']/tr[@style='background-color:#CCCCCC;']");
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


            }

            List<track> trackList = new List<track>();
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
                trackList.Add(new track() { name = track });

            }


            List<slot> slotsList = new List<slot>();

            //var slots = hap.DocumentNode.SelectNodes("//td[@class='style7']/strong[not(contains(text(),'Keynote')) and not(contains(text(),'Registration')) and not(contains(text(),'Lunch'))]");
            var slots = hap.DocumentNode.SelectNodes("//td[@class='style7']/strong");
            foreach (var item in slots)
            {
                var slotStart = item.InnerText.Split('-')[0];
                var slotStop = item.InnerText.Split('-')[1];
                var ampm = "PM";
                if (System.Threading.Thread.CurrentThread.CurrentCulture.CompareInfo.Compare(slotStart, "Wrap", System.Globalization.CompareOptions.IgnoreCase) >= 0)
                {
                    slotsList.Add(new slot()
                    {
                        start = DateTime.Parse(string.Format("02/07/2015 17:10", slotStart, ampm)),
                        end = DateTime.Parse(string.Format("02/07/2015 18:00", slotStart))
                    });
                }
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.CompareInfo.Compare(slotStart, "Keynote", System.Globalization.CompareOptions.IgnoreCase) >= 0)
                {
                    slotsList.Add(new slot()
                    {
                        start = DateTime.Parse(string.Format("02/07/2015 08:00", slotStart, ampm)),
                        end = DateTime.Parse(string.Format("02/07/2015 8:30", slotStart))
                    });
                }
                else if (System.Threading.Thread.CurrentThread.CurrentCulture.CompareInfo.Compare(slotStart, "Regis", System.Globalization.CompareOptions.IgnoreCase) >= 0)
                {
                    slotsList.Add(new slot()
                    {
                        start = DateTime.Parse(string.Format("02/07/2015 07:30", slotStart, ampm)),
                        end = DateTime.Parse(string.Format("02/07/2015 08:00", slotStart))
                    });
                }
                else
                {
                    if (int.Parse(slotStart.Split(':')[0]) > 4)
                    {
                        ampm = "AM";
                    }

                    slotsList.Add(new slot()
                    {
                        start = DateTime.Parse(string.Format("02/07/2015 {0} {1}", slotStart, ampm)),
                        end = DateTime.Parse(string.Format("02/07/2015 {0} {1}", slotStart, ampm)).AddHours(1).AddMinutes(10)
                    });
                }
            }
            int trackcounter = 0;
            trackslot[,] trackslots = new trackslot[50, 50];

            foreach (var track in trackList)
            {
                int slotcounter = 0;
                foreach (var slot in slotsList)
                {
                    var tslot = new trackslot
                    {
                        slot = slot,
                        track = track
                    };
                    trackslots[trackcounter, slotcounter++] = tslot;

                }
                ++trackcounter;
            }

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

                var tslot = trackslots[trackNumber, slotNumber];



            }


            //Carl DeSantis Building


        }
    }
}
