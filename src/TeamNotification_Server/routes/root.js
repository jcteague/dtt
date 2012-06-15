var methods = {};
methods.get_root = function(req, res){
    var r = {
        links: [
            {"rel": "self", "href": "/" },
            {"rel": "user", "href": "/user" }
        ]
    };
    res.json(r);
};

module.exports = {
    methods: methods,
    build_routes: function(app){
        app.get('/',methods.get_root);
    }
};
