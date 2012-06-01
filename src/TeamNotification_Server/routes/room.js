var methods = {};

var pg = require('pg');
var connectionString = "postgres://postgres:1234@localhost/dtt_main";
var client_db = new pg.Client(connectionString);
client_db.connect();

methods.post_room = function(req, res, next){
    var name = req.param('name');
    if(!name){ next( new Error(500,"name is require"));}
    client_db.query("INSERT INTO chat_room(name) VALUES($1)", [name]);
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


