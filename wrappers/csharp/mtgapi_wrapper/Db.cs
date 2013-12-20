using System;
using System.Net;
using Newtonsoft.Json;
using MtgDb.Info;

namespace MtgDb.Info.Driver
{
    public class Db
    {
        public string ApiUrl { get; set; }

        public Db ()
        {
            ApiUrl = "http://api.mtgdb.info";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Mtgdb.Info.Wrapper.Db"/> class.
        /// Only use this method if you running a local version MtgDB api. 
        /// </summary>
        /// <param name="url">Custom Url if not using: "http://api.mtgdb.info";</param>
        public Db(string url)
        {
            ApiUrl = url;
        }

        public Card GetCard(int id)
        {
            using (var client = new WebClient())
            {
                string url = string.Format ("{0}/cards/{1}", this.ApiUrl, id.ToString());
                var json = client.DownloadString(url);
                //var serializer = new JavaScriptSerializer();
                Card card = JsonConvert.DeserializeObject<Card>(json);

                return card;
            }
        }

        public Card[] GetCards()
        {
            using (var client = new WebClient())
            {
                string url = string.Format ("{0}/cards/", this.ApiUrl);
                var json = client.DownloadString(url);

                Card[] cards = JsonConvert.DeserializeObject<Card[]>(json);

                return cards;
            }
        }

        public Card[] FilterCards(string property, string value)
        {
            using (var client = new WebClient())
            {
                string url = string.Format ("{0}/cards/?{1}={2}", this.ApiUrl, property, value);
                var json = client.DownloadString(url);

                Card[] cards = JsonConvert.DeserializeObject<Card[]>(json);

                return cards;
            }
        }

        public Card[] GetSetCards(string setId)
        {
            using (var client = new WebClient())
            {
                string url = string.Format ("{0}/sets/{1}/cards/", this.ApiUrl, setId);
                var json = client.DownloadString(url);

                Card[] cards = JsonConvert.DeserializeObject<Card[]>(json);

                return cards;
            }
        }

        public CardSet GetSet(string setId)
        {
            using (var client = new WebClient())
            {
                string url = string.Format ("{0}/sets/{1}", this.ApiUrl, setId);
                var json = client.DownloadString(url);

                CardSet set = JsonConvert.DeserializeObject<CardSet>(json);

                return set;
            }
        }

        public CardSet[] GetSets()
        {
            using (var client = new WebClient())
            {
                string url = string.Format ("{0}/sets/", this.ApiUrl);
                var json = client.DownloadString(url);

                CardSet[] sets = JsonConvert.DeserializeObject<CardSet[]>(json);

                return sets;
            }
        }
    }
}

