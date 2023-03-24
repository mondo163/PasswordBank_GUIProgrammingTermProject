using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UserPasswordDatabaseLib.Converters
{

    //Help from https://blog.codeinside.eu/2015/03/30/json-dotnet-deserialize-to-abstract-class-or-interface/
    //on how to deserialize abstract objects with Json. Super simple honestly..
    public class JsonRegUserConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IUserType));
        }
        //this function I would check values of different types if user had any to return the specific user,
        //but since I only have RegUsersas an IUserType, I return every object back as a RegUser type
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            return jo.ToObject<RegUser>(serializer); 
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
