/**
 * Module Dependencies
 */
var express = require('express');
var app= module.exports = express.createServer();
var http = require('http');
var response = http.ServerResponse.prototype;
response.send_expanded = function(req,data){
    var get_expansion_url = function(data_to_expand, action_type ){
        var array = data_to_expand.get;
        if(!array) return;

        var length = array.length;
        var obj;
        while( length-- ){
            obj = array[ length ];
            if( obj.type == action_type ) return obj.url;
        }
    }

    var get_expansion_callback = function(url){
        var matchers = app.match(url);
        if(!matchers || matchers.length == 0) return;

        var callbacks = matchers[0].callbacks;
        if(!callbacks || callbacks.length == 0) return;
        var callback = callbacks[0];
        return callback;
    };

    var expand = function(expand,data_to_expand){
        var expansion_url = get_expansion_url(data_to_expand,expand);
        if(!expansion_url) return data_to_expand;

        var expansion_callback = get_expansion_callback(expansion_url);
        if(!expansion_callback) return data_to_expand;

        var expansion;
        var expansion_setter = function(x){expansion=x};
        expansion_callback(req,{ send: expansion_setter, send_expanded: expansion_setter});

        if(!expansion) return data_to_expand;

        data_to_expand[expand] = expansion;
        return data_to_expand;
    }

    if(req.body.expand) data = expand(req.body.expand,data);
    this.send.call(this,data);
};

/*config.js*/
app.use(express.bodyParser());
app.use(express.methodOverride());


/**
 * Mock Database
 */
var apiKeys = ['foo', 'bar', 'baz'];

var error = function(status, msg) {
    var err = new Error(msg);
    err.status = status;
    return err;
}

//API key Validation
app.use('/', function(req, res, next){
    var key = req.query['api-key'];
    if (!key) return next(error(400, 'api key required'));
    if (!~apiKeys.indexOf(key)) return next(error(401, 'invalid api key'));
    req.key = key;
    next();
});

//Configure routes after API validation
app.use(app.router);

//Application Error Handler
var appErrorHandler = function(err, req, res, next){
    res.send(err.status || 500, { error: err.message });
};
app.use(appErrorHandler);


app.options('/',function(req,res){
    var r = {  get: [ { type: 'rooms', url: '/user/rooms'} ], post: [ { type: 'room', url: '/room'} ]};
    res.send_expanded(req,r);
});


app.get('/user/rooms',function(req,res){
    var r = ["room1","room2"];
    res.send(r);
});

app.listen(3000);
console.log('Express server listening on port 3000');





