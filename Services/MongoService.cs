using ConsultaLogs.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultaLogs.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<LogData> _log;
        public MongoService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _log = database.GetCollection<LogData>(settings.CollectionName);
        }

        public List<LogData> Get() =>
            _log.Find(log => true).ToList();

        public LogData Get(string id) =>
            _log.Find<LogData>(log => log.Id == id).FirstOrDefault();

        public LogData Create(LogData log)
        {
            _log.InsertOne(log);
            return log;
        }

        public void Update(string id, LogData logIn) =>
            _log.ReplaceOne(log => log.Id == id, logIn);

        public void Remove(LogData logIn) =>
            _log.DeleteOne(log => log.Id == logIn.Id);

        public void Remove(string id) =>
            _log.DeleteOne(log => log.Id == id);
    }
}
