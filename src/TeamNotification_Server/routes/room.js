var methods = {};
var support = require('support');

methods.post_room = function(req, res, next){
    var name = req.param('name');
    if(!name){ next( new Error(400,"name is require"));}

    var db_client = support.create_db_client();
    db_client.query("INSERT INTO chat_room(name) VALUES($1)", [name]);

    res.send('room created');
};

methods.get_room_by_id = function(req, res){
    var r = { links: { "self" : { href: "#" } } };
    res.json(r);
};

module.exports = {
    methods: methods,
    build_routes: function(app){
        app.post('/room',methods.post_room);
        app.get('/rooms/:id',methods.get_room_by_id)
    }
};
