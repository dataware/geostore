using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GeoStoreServiceWebRole.Model;
using System.IO;

namespace GeoStoreServiceWebRole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGeoStoreService" in both code and config file together.
    [ServiceContract]
    public interface IGeoStoreService
    {
        [OperationContract]
        //find a location.
        [WebGet(UriTemplate = "Locations/{id}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        Location GetLocation(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "UploadLocation",
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        String UploadLocation(JsonLocation location);

        [OperationContract]
        [WebInvoke(UriTemplate = "UploadLocations",
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        String UploadLocations(IList<JsonLocation> locations);

        [OperationContract]
        [WebInvoke(UriTemplate = "UploadFile", Method = "POST")]
        void UploadFile(Stream fileContents);

        [OperationContract]
        [WebGet(UriTemplate = "GetLocations",
            ResponseFormat = WebMessageFormat.Json)]
        List<Location> GetLocations();

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateLocation", Method = "POST",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json)]
        void CreateLocation(Location instance);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class JsonLocation
    {
        [DataMember(Name = "accuracy")]
        public System.Nullable<double> accuracy
        {
            get;
            set;
        }

        [DataMember(Name = "altitude")]
        public System.Nullable<double> altitude
        {
            get;
            set;
        }

        [DataMember(Name = "bearing")]
        public System.Nullable<double> bearing
        {
            get;
            set;
        }

        [DataMember(Name = "latitude")]
        public System.Nullable<double> latitude
        {
            get;
            set;
        }

        [DataMember(Name = "longitude")]
        public System.Nullable<double> longitude
        {
            get;
            set;
        }

        [DataMember(Name = "provider")]
        public string provider
        {
            get;
            set;
        }

        [DataMember(Name = "speed")]
        public System.Nullable<double> speed
        {
            get;
            set;
        }

        [DataMember(Name = "extras")]
        public string extras
        {
            get;
            set;
        }

        [DataMember(Name = "entryTime")]
        public string entryTime
        {
            get;
            set;
        }

        [DataMember(Name = "id")]
        public System.Nullable<long> id
        {
            get;
            set;
        }

        [DataMember(Name = "jabberID")]
        public string jabberID
        {
            get;
            set;
        }

        [DataMember(Name = "processingMethod")]
        public string processingMethod
        {
            get;
            set;
        }

        [DataMember(Name = "deviceType")]
        public string deviceType
        {
            get;
            set;
        }

        [DataMember(Name = "deviceId")]
        public string deviceId
        {
            get;
            set;
        }

        [DataMember(Name = "sensorType")]
        public string sensorType
        {
            get;
            set;
        }

        [DataMember(Name = "sensorModel")]
        public string sensorModel
        {
            get;
            set;
        }

        [DataMember(Name = "IntersensorAgreement")]
        public System.Nullable<bool> IntersensorAgreement
        {
            get;
            set;
        }

        [DataMember(Name = "solutionConfidence")]
        public System.Nullable<double> solutionConfidence
        {
            get;
            set;
        }
    }

    [DataContract]
    public class FileData
    {
        [DataMember(Name = "Data")]
        public String Data
        {
            get;
            set;
        }
    }
}
