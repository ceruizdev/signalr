using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConsultaLogs
{
    public class LogData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        public string Id { get; set; }
        [BsonElement("id_process")]
        public int id_process { get; set; }
        [BsonElement("fecha_termina")]
        public string fecha_termina { get; set; }
    }
}
