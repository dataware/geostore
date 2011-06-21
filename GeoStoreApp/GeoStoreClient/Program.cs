using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;
using GeoStoreServiceWebRole.Model;
//using GeoStoreClient.GeoStoreServiceReference;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;

namespace GeoStoreClient
{
    class Program
    {

        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:18111/GeoStoreService.svc/";
            //UploadFile(baseAddress);

            
            //HttpClient client = new HttpClient("http://localhost:18111/GeoStoreService.svc/");

            //GeoStoreServiceClient client = new GeoStoreServiceClient();
            //Location location;
            //Random rand = new Random();



            /*
            //Getting the response as a string  
            Console.WriteLine("Get All locations:");
            Console.WriteLine(GetLocations(client));
            Console.ReadKey();

                    
            //Get location
            int locationId = 1;
            location = GetLocation(client, locationId);
            WriteOutLocation(location);

            //Update location
            location.longitude = 28.7;
            location.latitude = 43.8;
            Location locationUpdated = UpdateLocation(client, location);
            WriteOutLocation(locationUpdated);

            //Get Location to see the updated status.
            location = GetLocation(client, locationId);
            WriteOutLocation(location);

            Console.ReadKey();

            //Return location to its original state.
            location.longitude = 95.95;
            location.latitude = 95.95;
            UpdateLocation(client, location);

            //New location
            location = new Location { latitude = rand.NextDouble() * 100.0, longitude = rand.NextDouble() * 100.0 };
            InsertLocation(client, location);

            //Get all locations.
            Console.WriteLine("Get All locations:");
            Console.WriteLine(GetLocations(client));
            Console.ReadKey();*/
        }

        static String GetLocations(HttpClient client)
        {
            String locations;
            HttpResponseMessage response = client.Get("Locations");
            response.EnsureStatusIsSuccessful();
            locations = response.Content.ReadAsString();
            return locations;
        }

        /*
        static void UploadFile(string baseAddress)
        {
            string uploadUri = "UploadFile/" + "jsonData.zip";
            Console.WriteLine("Uplaoding location data");
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(baseAddress + uploadUri);
            req.ContentType = "text/plain";
            Stream reqStream = req.GetRequestStream();

            
            HttpContent content = HttpContentExtensions.CreateDataContract<string>(data);
            HttpResponseMessage response = client.Post(uploadUri, content);
            response.EnsureStatusIsSuccessful();
            return response.Content.ReadAsString();
        }*/

        static Location GetLocation(HttpClient client, int id)
        {
            // Getting the response as a string  
            Console.WriteLine("Location: {0}", id);
            string getUri = "Locations/" + id.ToString();
            HttpResponseMessage response = client.Get(getUri);
            response.EnsureStatusIsSuccessful();
            return response.Content.ReadAsJsonDataContract<Location>();
        }

        static Location UpdateLocation(HttpClient client, Location location)
        {
            Console.WriteLine("Updating location '{0}':", location.id);
            Console.WriteLine();

            string updateUri = "Locations/" + location.id.ToString();
            HttpContent content = HttpContentExtensions.CreateJsonDataContract(location);
            Console.WriteLine("Request");
            WriteOutContent(content);

            using (HttpResponseMessage response = client.Put(updateUri, content))
            {
                response.EnsureStatusIsSuccessful();
                return response.Content.ReadAsJsonDataContract<Location>();
            }

        }

        static void InsertLocation(HttpClient client, Location location)
        {
            Console.WriteLine("Inserting location");
            Console.WriteLine();

            string insertUri = "Locations";
            HttpContent content = HttpContentExtensions.CreateJsonDataContract(location);

            using (HttpResponseMessage response = client.Post(insertUri, content))
            {
                response.EnsureStatusIsSuccessful();
            }
        }

        static void WriteOutLocation(Location location)
        {
            Console.WriteLine(" Id: {0}", location.id);
            Console.WriteLine(" Longitude {0}", location.longitude);
            Console.WriteLine(" Latitude {0}", location.latitude);
        }

        static void WriteOutContent(HttpContent content)
        {
            content.LoadIntoBuffer();
            Console.WriteLine(content.ReadAsString());
            Console.WriteLine();
        }

        /*
        static String GetLocations(GeoStoreServiceClient client)
        {
            String locations;
            List<Location> list = client.GetLocations().ToList();
            locations = list.ToString();
            return locations;
        }

        static Location GetLocation(GeoStoreServiceClient client, int id)
        {
            // Getting the response as a string  
            Console.WriteLine("Location: {0}", id);
            string getUri = "Locations/" + id.ToString();
            Location location = client.GetLocation(id.ToString());
            return location;
        }

        static Location UpdateLocation(GeoStoreServiceClient client, Location location)
        {
            Console.WriteLine("Updating location '{0}':", location.id.ToString());
            Console.WriteLine();

            Location updatedLocation = client.UpdateLocation(location.id.ToString(), location);
            return updatedLocation;
        }*/
    }
}
