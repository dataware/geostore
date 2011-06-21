using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GeoStoreServiceWebRole.Model;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GeoStoreServiceWebRole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GeoStoreService" in code, svc and config file together.
    public class GeoStoreService : IGeoStoreService
    {
        /*************************
         * To be implemented.
         * **********************/
        static string deviceId = "HHFHDEHDHD";
        static int userId = 1;
 
        public Location GetLocation(string id)
        {
            int parseId = int.Parse(id);
            GeoStoreModelDataContext objectContext = new GeoStoreModelDataContext();

            Location location = (from loc in objectContext.Locations
                                 where loc.id == parseId
                                 select loc).First();
            return location;
        }

        public List<Location> GetLocations()
        {
            GeoStoreModelDataContext objectContext = new GeoStoreModelDataContext();
            List<Location> locationQuery = (from loc in objectContext.Locations
                                            select loc).ToList();
            return locationQuery;
        }

        public String UploadLocation(JsonLocation location)
        {
            Console.WriteLine(location.accuracy.ToString());
            //Location instance = new Location();
            Location modelLocation = prepareLocation(location);
            insertLocation(modelLocation);
            return "Location uploaded";
        }

        public String UploadLocations(IList<JsonLocation> locations)
        {
            Console.WriteLine(locations.ToString());
            foreach (JsonLocation location in locations)
            {
                Location modelLocation = prepareLocation(location);
                insertLocation(modelLocation);
            }
            return ("Locations uploaded successfully.");
        }

        public void UploadStream(Stream data)
        {
            Console.WriteLine(data);
        }
        
        public void CreateLocation(Location instance)
        {
            GeoStoreModelDataContext objectContext = new GeoStoreModelDataContext();
            objectContext.Locations.InsertOnSubmit(instance);
            objectContext.SubmitChanges();
        }

        public void UploadFile(Stream fileContents)
        {
            GZipStream decompressStream = null;
            try
            {
                //decompress
                decompressStream = new GZipStream(fileContents, CompressionMode.Decompress);

                //Read input compressed data and get the json data back.
                readAndSaveJsonData(decompressStream);
            }
            finally
            {
                decompressStream.Close();
            }
        }

        private Location prepareLocation(JsonLocation instance)
        {
            Location modelLocation = new Location();

            //Fill location object.
            modelLocation.accuracy = instance.accuracy;
            modelLocation.altitude = instance.altitude;
            modelLocation.bearing = instance.bearing;
            modelLocation.deviceId = instance.deviceId;
            modelLocation.deviceType = instance.deviceType;
            //modelLocation.entryTime = DateTime.Parse(instance.entryTime);
            modelLocation.extras = modelLocation.extras;
            modelLocation.IntersensorAgreement = instance.IntersensorAgreement;
            //modelLocation.latitude = instance.latitude;
            //modelLocation.longitude = instance.longitude;
            modelLocation.processingMethod = instance.processingMethod;
            modelLocation.provider = instance.provider;
            modelLocation.sensorModel = instance.sensorModel;
            modelLocation.sensorType = instance.sensorType;
            modelLocation.solutionConfidence = instance.solutionConfidence;
            modelLocation.speed = instance.speed;

            return modelLocation;
        }

        private void insertLocation(Location instance)
        {
            using (GeoStoreModelDataContext objectContext = new GeoStoreModelDataContext())
            {
                objectContext.Locations.InsertOnSubmit(instance);
                objectContext.SubmitChanges();
            }
        }
        
        /*
        //Reads stream and converts it into String.
        private static String readInputStream(Stream inputStream)
        {
            StringBuilder data = new StringBuilder();
            GZipStream decompressStream = null;
            StreamReader reader = null;
            try
            {
                //decompress
                decompressStream = new GZipStream(inputStream, CompressionMode.Decompress);
            }
            finally
            {
                decompressStream.Close();
            }

            try
            {
                reader = new StreamReader(decompressStream);
                String jsonDataLine = reader.ReadLine();
                data.Append(jsonDataLine);
                return jsonDataLine;
            }
            finally
            {
                reader.Close();
            }
            
        }*/

        //Convert bytes into string without affecting the data encoding.
        public static string ConvertBytesToString(byte[] bytes)
        {
            string output = String.Empty;
            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream(bytes);
                memoryStream.Position = 0;

                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    output = streamReader.ReadToEnd();
                }
                return output;
            }
            finally
            {
                memoryStream.Close();
            }
        }

        //decode base64 encoded data.
        private string base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        private void readAndSaveJsonData(GZipStream inputStream)
        {
            StreamReader streamReader = null;

            try
            {
                streamReader = new StreamReader(inputStream);
                String jsonLine = null;

                while ((jsonLine = streamReader.ReadLine()) != null)
                {
                    //get geoLocation object
                    JObject geoLocation = JObject.Parse(jsonLine);
                    //create entry object.
                    Entry entry = createEntry(geoLocation);
                    //create location object.
                    Location location = createAndInsertLocation(geoLocation, entry);
                    //Data object
                    JObject jsonIdentifierObj = (JObject)geoLocation["identifier"];
                    Object obj = createAndInsertDataWithContent(jsonIdentifierObj, (String)geoLocation["data"], entry);

                    //save objects into database.
                    using (GeoStoreModelDataContext objectContext = new GeoStoreModelDataContext())
                    {
                        objectContext.Entries.InsertOnSubmit(entry);
                        objectContext.SubmitChanges();
                    }
                }
            }
            finally
            {
                streamReader.Close();
            }
        }

        private Entry createEntry(JObject geoLocation)
        {
            //get timestamp and identifier.
            Entry entry = new Entry();
            entry.externalTimestamp = (long)geoLocation["timestamp"];
            entry.deviceId = deviceId;
            entry.userId = userId;
            return entry;
        }

        private Location createAndInsertLocation(JObject geoLocation, Entry entry)
        {
            JObject jsonLocation = (JObject)geoLocation["location"];
            Location location = new Location();
            location.accuracy = (double)jsonLocation["accuracy"];
            location.altitude = (double)jsonLocation["altitude"];
            location.bearing = (double)jsonLocation["bearing"];
            //get extras string.
            JObject extras = (JObject)jsonLocation["extras"];
            location.extras = extras.ToString();
            location.latitude = (double)jsonLocation["latitude"];
            location.longitude = (double)jsonLocation["longitude"];
            location.provider = (string)jsonLocation["provider"];
            location.speed = (double)jsonLocation["speed"];
            //entryTime
            long time = (long)jsonLocation["time"];
            location.measurementTime = ConvertMilliSecondsToDateTime(time);
            //insert location into entry object.
            entry.Locations.Insert(0, location);
            return location;
        }

        private Object createAndInsertDataWithContent(JObject identiferObj, String data, Entry entry)
        {
            Object dataObj = null;
            //Decode base 64 encoded data.
            String jsonDataString = base64Decode(data);
            
            String objectIdentifier = (String)identiferObj["uniqueID"];
            switch (objectIdentifier)
            {
                case "aethers.notebook.logger.managed.celllocation.CellLocationLogger":
                    CellLocation cellLocation = createCellLocation(jsonDataString, entry);
                    dataObj = cellLocation;
                    break;
                case "aethers.notebook.logger.managed.dataconnectionstate.DataConnectionStateLogger":
                    DataConnection dataConnection = createDataConnection(jsonDataString, entry);
                    dataObj = dataConnection;
                    break;
                case "aethers.notebook.logger.managed.servicestate.ServiceStateLogger":
                    ServiceState serviceState = createServiceState(jsonDataString, entry);
                    dataObj = serviceState;
                    break;
                case "aethers.notebook.logger.managed.signalstrength.SignalStrengthLogger":
                    SignalStrength signalStrength = createSignalStrength(jsonDataString, entry);
                    dataObj = signalStrength;
                    break;
                case "aethers.notebook.logger.managed.wifi.WifiLogger":
                    List<Wifi> wifiPoints = createWifiPoints(jsonDataString, entry);
                    dataObj = wifiPoints;
                    break;
            }

            return dataObj;
        }

        private CellLocation createCellLocation(String jsonDataString, Entry entry)
        {
            JObject jsonDataObj = JObject.Parse(jsonDataString);
            CellLocation cellLocation = new CellLocation();
            string type = (string)jsonDataObj["type"];
            cellLocation.type = type;
            
            switch (type)
            {
                case "android.telephony.cdma.CdmaCellLocation":
                    cellLocation.baseStationId = (int)jsonDataObj["baseStationId"];
                    cellLocation.baseStationLatitude = (int)jsonDataObj["baseStationLatitude"];
                    cellLocation.baseStationLongitude = (int)jsonDataObj["baseStationLongitude"];
                    cellLocation.networkId = (int)jsonDataObj["networkId"];
                    cellLocation.systemId = (int)jsonDataObj["systemId"];
                    break;
                case "android.telephony.gsm.GsmCellLocation":
                    cellLocation.cid = (int)jsonDataObj["cid"];
                    cellLocation.lac = (int)jsonDataObj["lac"];
                    break;
            }

            //insert array of cell locations.
            JArray jsonNeighboringCells = (JArray)jsonDataObj["neighboringCells"];
            if (jsonNeighboringCells != null)
            {
                foreach (JObject jsonNeighboringCellObj in jsonNeighboringCells)
                {
                    NeighboringCell neighboringCell = createNeighboringCell(jsonNeighboringCellObj);
                    cellLocation.NeighboringCells.Add(neighboringCell);
                }
            }

            //insert cell location into entry.
            entry.CellLocations.Add(cellLocation);

            return cellLocation;
        }

        private NeighboringCell createNeighboringCell(JObject jsonNeighboringCellObj)
        {
            NeighboringCell neighboringCell = new NeighboringCell();
            neighboringCell.cid = (int)jsonNeighboringCellObj["cid"];
            neighboringCell.lac = (int)jsonNeighboringCellObj["lac"];
            neighboringCell.psc = (int)jsonNeighboringCellObj["psc"];
            neighboringCell.rssi = (int)jsonNeighboringCellObj["rssi"];
            neighboringCell.networkType = (string)jsonNeighboringCellObj["networkType"];
            return neighboringCell;
        }

        private DataConnection createDataConnection(String jsonDataString, Entry entry)
        {
            JObject jsonDataObj = JObject.Parse(jsonDataString);
            DataConnection dataConnection = new DataConnection();
            dataConnection.state = (string)jsonDataObj["state"];
            dataConnection.networkType = (string)jsonDataObj["networkType"];
            //insert data connection.
            entry.DataConnections.Add(dataConnection);
            return dataConnection;
        }

        private ServiceState createServiceState(String jsonDataString, Entry entry)
        {
            JObject jsonDataObj = JObject.Parse(jsonDataString);
            ServiceState serviceState = new ServiceState();
            serviceState.isManualSelection = (Boolean)jsonDataObj["isManualSelection"];
            serviceState.operatorAlphaLong = (string)jsonDataObj["operatorAlphaLong"];
            serviceState.operatorAlphaShort = (string)jsonDataObj["operatorAlphaShort"];
            serviceState.operatorNumeric = (string)jsonDataObj["operatorNumeric"];
            serviceState.roaming = (Boolean)jsonDataObj["roaming"];
            serviceState.state = (string)jsonDataObj["state"];
            //insert service state
            entry.ServiceStates.Add(serviceState);
            return serviceState;
        }

        private SignalStrength createSignalStrength(String jsonDataString, Entry entry)
        {
            JObject jsonDataObj = JObject.Parse(jsonDataString);
            /*
            SignalStrength signalStrength = JsonConvert.DeserializeObject<SignalStrength>(jsonDataObj.ToString());
            //insert signal strength.
            entry.SignalStrengths.Add(signalStrength);
            return signalStrength;*/

            SignalStrength signalStrength = new SignalStrength();
            signalStrength.cdmaDbm = (int)jsonDataObj["cdmaDbm"];
            signalStrength.cdmaEcio = (int)jsonDataObj["cdmaEcio"];
            signalStrength.evdoDbm = (int)jsonDataObj["evdoDbm"];
            signalStrength.evdoEcio = (int)jsonDataObj["evdoEcio"];
            signalStrength.evdoSnr = (int)jsonDataObj["evdoSnr"];
            signalStrength.gsmBitErrorRate = (int)jsonDataObj["gsmBitErrorRate"];
            signalStrength.gsmSingalStrength = (int)jsonDataObj["gsmSignalStrength"];
            signalStrength.isGsm = (Boolean)jsonDataObj["isGsm"];
            //insert service state
            entry.SignalStrengths.Add(signalStrength);
            return signalStrength;
       }

        private List<Wifi> createWifiPoints(String jsonDataString, Entry entry)
        {
            List<Wifi> wifiPoints = new List<Wifi>();
            JArray jsonWifiPoints = JArray.Parse(jsonDataString);
            foreach (JObject jsonWifi in jsonWifiPoints)
            {
                Wifi wifi = new Wifi();
                wifi.bssid = (string)jsonWifi["bssid"];
                wifi.ssid = (string)jsonWifi["ssid"];
                wifi.capabilities = (string)jsonWifi["capabilities"];
                wifi.frequency = (int)jsonWifi["frequency"];
                wifi.level = (int)jsonWifi["level"];

                //insert wifi object.
                entry.Wifis.Add(wifi);
                wifiPoints.Add(wifi);
            }
            return wifiPoints;
        }
        
        private DateTime ConvertMilliSecondsToDateTime(long milliSeconds)
        {
            DateTime UTCBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = UTCBaseTime.Add(new TimeSpan(milliSeconds * TimeSpan.TicksPerMillisecond)).ToLocalTime();
            return dt;
        }

    }
}
