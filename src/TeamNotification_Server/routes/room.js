
var pg = require('pg');
var connectionString = "postgres://postgres:1234@localhost/dtt_main";
var client_db = new pg.Client(connectionString);
client_db.connect();

module.exports = function(app){
	
    app.post('/room',function(req, res, next){
	
	var name = req.param('name');
	console.log(name);
	if(!name){ next( new Error(500,"blah"));}
	client_db.query("INSERT INTO chat_room(name) VALUES($1)", [name]);
	res.send('room created');

    });
};

