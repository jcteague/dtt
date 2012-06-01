var methods = {};
methods.get_root = function(req, res){
    var r = {
        links: {
            "self" : { href: "/" },
            "user" : { href: "/user" }
        }
    };
    res.json(r);
};

module.exports = {
    methods: methods,
    build_routes: function(app){
        app.get('/',methods.get_root);
    }
};
