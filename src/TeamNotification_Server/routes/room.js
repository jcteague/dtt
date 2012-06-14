var methods = {};
var support = require('support').core;
var express = require('express');
methods.post_room = function(req, res, next){
    
    var values =req.body;
    console.log(values);
    var chat_room= support.entity_factory.create('ChatRoom',values);
    chat_room.save(function(err,saved_chat_room){
        if(!err) {
            res.send('room '+ saved_chat_room.id + ' created');
        }
        else next(new Error(err.code,err.message));
    });
};

methods.get_room_by_id = function(req, res){
    var r = { 
        links: [ 
            {"rel":"self", "href": "/rooms/1" } 
        ] 
    };
    res.json(r);
};

methods.get_room = function(req, res){
    var r = {
        'links' : [
          {'rel':'self', 'href':'/room'}
        ],
        'template':{
            'data':[{'name':'name'}]
        }
    };
    res.json(r);
}
module.exports = {
    methods: methods,
    build_routes: function(app){
        app.post('/room',express.bodyParser(), methods.post_room);
        app.get('/rooms/:id',methods.get_room_by_id);
        app.get('/room',methods.get_room);
    }
};
