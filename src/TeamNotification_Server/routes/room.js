var methods = {};
var support = require('support').core;

methods.post_room = function(req, res, next){
    var name = req.param('name');
    if(!name){ next( new Error(400,"name is require"));}
    var chat_room= new support.entity.ChatRoom();
    chat_room.name = name;
    chat_room.save(function(err,chat_room){
        if(!err) res.send('room created');
    });
};

methods.get_room_by_id = function(req, res){
    var r = { links: { "self" : { href: "#" } } };
    res.json(r);
};

module.exports = {
    methods: methods,
    build_routes: function(app){
        app.get('/room',methods.post_room);
        app.get('/rooms/:id',methods.get_room_by_id)
    }
};
