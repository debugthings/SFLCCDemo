using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;


namespace SFLCC_Performance_Test
{
    class CustomLinkExtraction : ExtractionRule
    {
        public CustomLinkExtraction() { }

        public bool ReturnRandom { get; set; }

        public string ExtractionPattern { get; set; }

        public bool UseRegex { get; set; }

        public int Index { get; set; }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            HtmlAgilityPack.HtmlDocument hapDoc = new HtmlAgilityPack.HtmlDocument();
            hapDoc.LoadHtml(e.Response.BodyString);
            List<HtmlAgilityPack.HtmlNode> hapNodes = null;
            if (string.IsNullOrEmpty(ExtractionPattern))
            {
                hapNodes = hapDoc.DocumentNode.SelectNodes("//a").ToList();
            }
            else
            {
                if (UseRegex)
                {
                    var rx = new System.Text.RegularExpressions.Regex(ExtractionPattern);
                    hapNodes = hapDoc.DocumentNode.SelectNodes("//a").Where(a => rx.IsMatch(a.Attributes["href"].Value)).ToList();

                } else
                {
                    hapNodes = hapDoc.DocumentNode.SelectNodes(string.Format("//a[contains(@href, '{0}')]", ExtractionPattern)).ToList();
                }
            }
            if (hapNodes != null)
            {
                if (ReturnRandom)
                {
                    e.WebTest.Context.Add(ContextParameterName, hapNodes[new Random().Next(0, hapNodes.Count)].Attributes["href"].Value);
                }
                else
                {
                    Index = hapNodes.Count < Index ? hapNodes.Count : Index;
                    e.WebTest.Context.Add(ContextParameterName, hapNodes[Index].Attributes["href"].Value);
                }
                
            }
        }
    }
}
