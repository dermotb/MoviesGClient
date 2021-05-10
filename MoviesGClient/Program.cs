using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MoviesGQL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesGClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GraphQLHttpClient graphQLHttpClient;

            var httpClientOption = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("https://moviesg.azurewebsites.net/api")
            };

            graphQLHttpClient = new GraphQLHttpClient(httpClientOption, new NewtonsoftJsonSerializer());
            GetAllMovies(graphQLHttpClient).Wait();
        }

            private static async Task GetAllMovies(GraphQLHttpClient _gqlClient)
            {
                //Query q = new Query();
                string queryText = $"query{{movies{{movieID, title}}}}";
                string queryName = "movies";

                try
                {
                    var request = new GraphQLRequest
                    {
                        Query = queryText
                    };

                var response = await _gqlClient.SendQueryAsync<object>(request);

                string result = response.Data.ToString();
                result = result.Replace($"\"{queryName}\":", string.Empty);
                result = result.Remove(0, 1);
                result = result.Remove(result.Length - 1, 1);

                var collection = JsonConvert.DeserializeObject<IEnumerable<Movie>>(result);

                foreach (Movie m in collection)
                {
                    Console.WriteLine("Movie ID: {0} - Title: {1}",m.MovieID, m.Title);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
