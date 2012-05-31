var pg = require('pg');
var connectionString = "postgres://postgres:1234@localhost/dtt_main";
var client_db = new pg.Client(connectionString);

function verify (value){
    if(!value) next(new Error(500,"blah"));
}

module.exports = function(app){
    app.post('/room',function(req, res){

        var name = req.param('name');
        verify(name);
        client_db.query("INSERT INTO chat_room (name) VALUES($1)", [name]);

    });

    app.get('/rooms/:id',function(req, res){
        var r = {
            links: {
                "self" : { href: "#" }
            }
        };
        res.json(r);
    });
};

