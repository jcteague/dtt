var methods = {};
var support = require('support').core;

methods.post_room = function(req, res, next){
    var chat_room= support.entity_factory.create('ChatRoom',req.param('chat_room'));
    chat_room.save(function(err,saved_chat_room){
        if(!err) res.send('room '+ saved_chat_room.id + ' created');
        else next(new Error(err.code,err.message));
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
        app.get('/rooms/:id',methods.get_room_by_id);
    }
};
