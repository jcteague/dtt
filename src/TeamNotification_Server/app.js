var express = require('express');
var redis = require('redis');
var client = redis.createClient();

client.on('error',function(err){
   console.log('error');
   console.log(err);
});

var app= express.createServer();

app.get('/',function(req,res){
    var result = {
        name:'raymi',
        id: 1
    };

    var userId = req.param('userId');
    var userName = req.param('userName');
    var message = req.param('userMessage');

    var full_message = userName + '-' + message;
    console.log('publishing message: ' + full_message);
    client.publish('chat1',full_message);
    res.send(full_message);
});


app.listen(3000);
