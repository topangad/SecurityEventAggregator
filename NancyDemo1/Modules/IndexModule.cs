using Nancy;
using System;
using System.Text;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace NancyDemo1.Modules
{
    public class IndexModule : NancyModule
    {
        private string jsonString = "";
        public IndexModule()
        {
            Get["/"] = home =>
            {
                return View["index.html"];
            };

            Get["/event/{id}"] = x =>
            {
                //var query = Request.Query.eventid;
                int t1;
                t1 = (int)(x.id);
                getJsonData(t1);
                var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
                return new Response
                {
                    ContentType = "application/json",
                    Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length)
                };

            };
        }

        private String stringManager(string eID, string eDesc, string eEx)
        {
            return "{" + 
                String.Format("\"{0}\" : \"{1}\", \"{2}\" : \"{3}\", \"{4}\" : \"{5}\"",
                    "eID", eID ,
                    "eDesc", eDesc,
                    "eEx", eEx) 
                + "}";
        }

        private async void getJsonData(int eventId)
        {
            var url = "https://www.ultimatewindowssecurity.com/securitylog/encyclopedia/event.aspx";
            string urlParams = "?eventid=";            

            HttpClient httpClient = new HttpClient();

            Uri uri = new Uri("https://www.ultimatewindowssecurity.com/securitylog/encyclopedia/event.aspx?eventid=5152#ctl00_ctl00_ctl00_ctl00_Content_Content_Content_Content_FieldsDiv");
            //wvSampleFields.Navigate(uri);

            HttpResponseMessage response = httpClient.GetAsync(url + urlParams + eventId).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var htmlString = await response.Content.ReadAsStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml((String)htmlString);

                String eID;
                String eventDesc;
                String eventExample;

                IEnumerable<HtmlNode> dom = doc.DocumentNode.DescendantsAndSelf();

                var allElementsWithClassHey = doc.DocumentNode
                    .Descendants()
                    .Where(d => d.Attributes.Contains("class")
                        &&
                     d.Attributes["class"].Value.Contains("hey"));

                foreach (var d in allElementsWithClassHey.Take(1))
                {
                    var regex = new Regex(Regex.Escape(" "));
                    var newText = regex.Replace(d.InnerText, "", 1);

                    string intermediate = d.InnerText.Replace('\r', ' ').Replace('\n', ' ').Replace(':',' ').Trim();
                    eID = intermediate.Split(' ')[0];
                    eventDesc = d.InnerText.Split(':')[1].Replace('\r', ' ').Replace('\n', ' ').Trim();
                    this.jsonString = stringManager(eID, eventDesc, "lol");
                }                

            }
           
        }
    }
}