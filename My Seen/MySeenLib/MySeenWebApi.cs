using System.Collections.Generic;
using Newtonsoft.Json;

namespace MySeenLib
{
    public static class MySeenWebApi
    {
        public static string ApiHost => Admin.IsDebug ? "http://localhost:44301" : "http://myseen.by";
        public static string ApiHostAndroid => Admin.IsDebug ? "https://10.0.2.2:443" : ApiHost;

        public enum DataModes
        {
            Road = 1
        }

        public enum SyncModesApiData
        {
            GetRoads = 1
        }

        public enum SyncModesApiUsers
        {
            IsUserExists = 1
        }
        public enum SyncModesApiLogin
        {
            GetKey = 1
        }

        public static int ApiVersion = 1;

        public static bool CheckApiVersion(int apiVersion)
        {
            if (apiVersion > ApiVersion) return false;

            return true;
        }

        public static string ApiUsers { get; } = @"/api/Users/";
        public static string ApiLogin { get; } = @"/api/Login/";
        public static string ApiSync { get; } = @"/api/Sync/";

        public static string ShareTracks { get; } = @"/roads/shared/";
        public static string ShareEvents { get; } = @"/events/shared/";
        public static string ShareFilms { get; } = @"/films/shared/";
        public static string ShareSerials { get; } = @"/serials/shared/";
        public static string ShareBooks { get; } = @"/books/shared/";

        public static IEnumerable<SyncJsonData> GetResponse(string data)
        {
            return JsonConvert.DeserializeObject<IEnumerable<SyncJsonData>>(data);
        }

        public static string SetResponse(IEnumerable<SyncJsonData> data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static SyncJsonAnswer GetResponseAnswer(string data)
        {
            SyncJsonAnswer answer = null;
            try
            {
                answer = JsonConvert.DeserializeObject<SyncJsonAnswer>(data);
            }
            catch
            {
                // ignored
            }
            return answer;
        }
        public class SyncJsonAnswer
        {
            [JsonProperty("Value")]
            public Values Value { get; set; }

            [JsonProperty("Data")]
            public string Data { get; set; }

            public enum Values
            {
                Ok = 1,
                NoData = 2,
                BadRequestMode = 3,
                UserNotExist = 4,
                NewDataRecieved = 5,
                NoLongerSupportedVersion = 6,
                SomeErrorObtained = 7
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class SyncJsonData
        {
            [JsonProperty("DataMode")]
            public int DataMode { get; set; } //in DataModes

            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Type")]
            public int Type { get; set; }

            //[JsonProperty("Date")]
            //public DateTime Date { get; set; }

            [JsonProperty("Coordinates")]
            public string Coordinates { get; set; }

            [JsonProperty("Distance")]
            public double Distance { get; set; }
        }
    }
}