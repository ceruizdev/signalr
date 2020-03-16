"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/logHub").build();

connection.on("NuevoLog", function (lista) {
    document.getElementById("contenido").innerHTML = ""
    document.getElementById("contenido").innerHTML = lista;
});



connection.start().then(function () {
    setInterval(function goFaster() {
        connection.invoke("LogMessage").catch(function (err) {
            return console.error(err.toString());
        })
    }, 1000);
        
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    connection.invoke("LogMessage").catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});