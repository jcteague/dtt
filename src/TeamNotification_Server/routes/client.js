var http = require('http');
var methods = {};
methods.get_client = function(req, main_res){
    /*
    var path = req.param('path') || '/user';
    var options = { host: 'localhost', port: 3000, path: path };
    console.log(http);

    http.get(options,function(res){
        var data = '';

        res.on('data', function (chunk){
            data += chunk;
        });

        res.on('end',function() {
            var obj = JSON.parse(data);
            main_res.render('client.jade',{
                model: {
                    response: obj,
                }});
        });

    }).on('error',function(e){
        next(new Error(e));
    });
    */
};

module.exports = {
    methods: methods,
    build_routes: function(app){
        app.get('/client', methods.get_client);
    }
};
