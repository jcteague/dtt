var http = require('http');
var methods = {};
methods.get_api_client = function(req, main_res){
    var path = req.param('path') || '/';
    var options = { host: 'localhost', port: 3000, path: path };
    http.get(options,function(res){
        var data = '';

        res.on('data', function (chunk){
            data += chunk;
        });

        res.on('end',function(){
            var obj = JSON.parse(data);
            main_res.render('api_client.jade',{
                model: {
                    response: obj,
                request: options
                }});
        });

    }).on('error',function(e){
        next(new Error(e));
    });
};

module.exports = {
    methods: methods,
    build_routes: function(app){
        app.get('/api_client',methods.get_api_client);
    }
};


