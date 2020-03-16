using ConsultaLogs;
using ConsultaLogs.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace signalrprotect
{
    public class LogHub : Hub
    {
        private List<LogData> data;
        private IMongoDBSettings settings;
        private string tabla = "";
        public LogHub(IMongoDBSettings settings)
        {
            this.settings = settings;
        }
        public async Task LogMessage()
        {
            GetLogs();
        }

        public async void GetLogs()
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            var _collection = db.GetCollection<LogData>(settings.CollectionName);
            try
            {
                var pipeline = new[]{new BsonDocument(){
                {"$group", new BsonDocument(){
                    {"_id", "$id_process"},
                    {"data", new BsonDocument(){
                        {"$push", "$$ROOT"}
                    }}
                }}
            },new BsonDocument(){
                {"$addFields", new BsonDocument(){
                    {"data", new BsonDocument(){
                        {"$arrayElemAt", new BsonArray(){new BsonDocument(){
                            {"$filter", new BsonDocument(){
                                {"input", "$data"},
                                {"cond", new BsonDocument(){
                                    {"$eq", new BsonArray(){"$$this.fecha_termina",new BsonDocument(){
                                        {"$max", "$data.fecha_termina"}
                                    }}}
                                }}
                            }}
                        },0}}
                    }}
                }}
            },new BsonDocument(){
                {"$replaceRoot", new BsonDocument(){
                    {"newRoot", "$data"}
                }}
            },new BsonDocument(){
                {"$sort", new BsonDocument(){
                    {"id_process", 1}
                }}
            }};
                var aggregate = _collection.Aggregate<LogData>(pipeline);

                var results = await aggregate.ToListAsync();

                foreach (var item in results)
                {
                    tabla += "<tr>";
                    tabla += "<td scope='col'>" + item.id_process + "</td>";
                    tabla += "<td scope='col'>" + item.fecha_termina + "</td>";
                    tabla += "</tr>";
                }
                await Clients.All.SendAsync("NuevoLog", tabla);
                tabla = "";
            }
            catch (Exception e)
            {
                throw new System.ArgumentException(e.Message);
            }
        }
    }
}





