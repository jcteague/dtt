module.exports = function(app){
    app.get('/user',function(req, res){
        var r = {
            links: {
                "self" : { href: "/user" },
                "rooms" : { href: "/user/rooms" }
            }
        };
        res.json(r);
    });

    app.get('/user/rooms',function(req, res){
        var r = {
            links: {
                "self" : { href: "/user/rooms" },
                "OpenRoom1" : { href: "/rooms/1" },
                "OpenRoom2" : { href: "/rooms/2" },
                "OpenRoom2" : { href: "/rooms/3" }
            }
        };
        res.json(r);
    });
};

