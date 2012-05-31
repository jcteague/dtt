module.exports = function(app){
    app.get('/',function(req, res){
        var r = {
            links: {
                "self" : { href: "/" },
                "user" : { href: "/user" }
            }
        };
        res.json(r);
    });
};

