var express = require('express');
var app= express.createServer();
app.use(express.bodyParser());
app.use(express.methodOverride());

app.options('/',function(req,res){
    r = {
        get: [ { type: 'get_rooms', url: '/user/rooms'} ],
        post: [ { type: 'create_room', url: '/room'} ]
    }
    res.send(r);
});

app.listen(3000);


