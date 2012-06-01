var methods = {};

methods.get_user = function(req, res){
    var r = {
        links: {
            "self" : { href: "/user" },
            "rooms" : { href: "/user/rooms" }
        }
    };
    res.json(r);
};

methods.get_user_rooms = function(req, res){
    var r = {
        links: {
            "self" : { href: "/user/rooms" },
            "OpenRoom1" : { href: "/rooms/1" },
        }
    };
    res.json(r);
};

module.exports = {
    methods : methods,
    build_routes: function(app){
        app.get('/user',methods.get_user);
        app.get('/user/rooms',methods.get_user_rooms);
    }
};
