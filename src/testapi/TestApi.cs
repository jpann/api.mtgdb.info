using NUnit.Framework;
using System;
using Mtg;
using Mtg.Model;
using Nancy;
using System.Collections.Generic;
using MongoDB.Driver;

namespace testapi
{
    //// xsp4 --port 8082
    /// run the above command in api.mtgdb.info
    [TestFixture ()]
    public class TestApi
    {
        private const string connectionString = "mongodb://localhost";
        IRepository repository = new MongoRepository (connectionString);

        [Test()]
        public void Test_get_card_sub_types()
        {
            string [] subtypes = repository.GetCardSubTypes().Result;

            Assert.Greater(subtypes.Length, 0);
        }

        [Test()]
        public void Test_get_card_types()
        {
            string [] types = repository.GetCardTypes().Result;

            Assert.Greater(types.Length, 0);
        }

        [Test()]
        public void Test_get_card_rarity_types()
        {
            string [] types = repository.GetCardRarity().Result;

            Assert.Greater(types.Length, 0);
        }

        [Test()]
        public void Test_search_verify()
        {
            CardSearch search = new CardSearch("name eq 'shit and shit' and type not creature");
            string[] elements = search.Verify();
            foreach(string s in elements)
            {
                System.Console.WriteLine(s);
            }
        }

        [Test()]
        public void Test_mongo()
        {
            CardSearch search = 
                new CardSearch("name eq 'Giant Growth' and color eq green");
            List<IMongoQuery> queries = search.MongoQuery();
            Assert.Greater(queries.Count,1);
        }
            
        [Test()]
        public void Test_complex_search()
        {
            Card [] cards = repository.Search("name m 'giant'",isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("name eq 'giant Growth'",isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("name not 'Giant Growth'",isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("name gt 'Giant Growth'",isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("name gte 'Giant Growth'",isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("name lt 'Giant Growth'",isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("name lte 'Giant Growth'",isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("color m green and name m 'Growth'", isComplex: true).Result;
            Assert.Greater(cards.Length, 0);
            cards = repository.Search("convertedmanacost lt 3", isComplex: true).Result;
            Assert.Greater(cards.Length, 0);

            //c:rb+t:knight+r:u
            cards = repository.Search("color eq white and color eq green and subtype m 'Knight' and " +
                "rarity eq Uncommon", isComplex: true).Result;
            Assert.Greater(cards.Length, 0);

            //"c=u+t:creature+d:flying+cc<3+n:cloud"
            cards = repository.Search("color eq blue and type m 'Creature' and description m 'flying' " +
                "and convertedmanacost lt 3 and name m 'Cloud'", isComplex: true).Result;

            Assert.Greater(cards.Length, 0);
        }

        [Test()]
        public void Test_get_card_by_setNumber()
        {
            Card card = repository.GetCardBySetNumber("THS", 90).Result;
            System.Console.Write(card.Id.ToString() + ":" + card.SetNumber.ToString());
            Assert.AreEqual("THS", card.CardSetId);
            Assert.AreEqual(90, card.SetNumber);
        }
            
        [Test()]
        public void Test_get_random_card()
        {
            Card rcard = repository.GetRandomCard().Result;
            System.Console.Write(rcard.Id.ToString());
            Assert.IsNotNull(rcard);
        }

        [Test()]
        public void Test_get_random_card_performance()
        {
            Card rcard = repository.GetRandomCard().Result;
           
            for(int i = 0; i < 1000; i++ )
            {
                rcard = repository.GetRandomCard().Result;
            }
        }
            
        [Test()]
        public void Test_get_random_card_in_set()
        {
            Card rcard = repository.GetRandomCardInSet("lea").Result;
            System.Console.Write(rcard.Id.ToString());
            Assert.IsNotNull(rcard);
        }
            

            
        [Test ()]
        public void Test_get_multiple_sets ()
        {
            CardSet[] sets = repository.GetSets(new string[]{"all","arb"}).Result;
            Assert.Greater (sets.Length,1);
        }

        [Test ()]
        public void Test_get_multiple_cards ()
        {
            Card[] cards = repository.GetCards(new int[]{1,2,3,4,5}).Result;
            Assert.Greater (cards.Length,1);
        }

        [Test ()]
        public void Test_search_cards ()
        {
            Card[] cards = repository.Search ("giant").Result;
            Assert.Greater (cards.Length,1);
        }

        [Test ()]
        public void Test_get_cards_by_filter ()
        {
            DynamicDictionary query = new DynamicDictionary(); 

            query.Add ("name", "Ankh of Mishra");
            Card[] cards = repository.GetCards (query).Result;
            Assert.Greater (cards.Length,1);
        }

        [Test ()]
        public void Test_get_cards_by_set ()
        {
            Card[] cards = repository.GetCardsBySet ("10E").Result;
            Assert.Greater (cards.Length,1);
        }

        [Test ()]
        public void Test_get_cards_by_set_with_range ()
        {
            Card[] cards = repository.GetCardsBySet ("10E",11,20).Result;
            Assert.AreEqual (10, cards.Length);
        }


        [Test ()]
        public void Test_get_card()
        {
            Card card = repository.GetCard(1).Result;
            Assert.IsNotNull (card);
        }

        [Test ()]
        public void Test_get_cards_by_name ()
        {
            Card[] cards = repository.GetCards ("ankh of mishra").Result;
            Assert.Greater (cards.Length, 1);
        }

        [Test ()]
        public void Test_get_cards_by_name_without_match ()
        {
            Card[] cards = repository.GetCards ("").Result;
            Assert.Greater (cards.Length, 0);
        }

        [Test ()]
        public void Test_get_sets ()
        {
            CardSet[] sets = repository.GetSets().Result;
            Assert.Greater (sets.Length,1);
        }

        [Test ()]
        public void Test_get_set ()
        {
            CardSet set = repository.GetSet("10E").Result;
            Assert.IsNotNull (set);
        }
    }
}

