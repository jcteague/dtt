
/**
 * Module dependencies.
 */

var express = require('express'),
    everyauth = require('everyauth')
    ;

everyauth.github
    .appId('8218ee1deea21ffe9cff')
    .appSecret('1d8218268acbd18aadbd6b768cfe132480dbcfbf')
    .findOrCreateUser(function(session, accessToken, extra, userMetaData){
        console.log("find or create user");
    })
    .redirectPath("/");
var app = module.exports = express.createServer();
//require('./routes')(app);
//require('./helper')(app);


/**
 * Mock Database
 */
var apiKeys = ['foo', 'bar', 'baz'];

var error = function(status, msg) {
    var err = new Error(msg);
    err.status = status;
    return err;
}

app.configure(function(){
    app.set('views', __dirname + '/views');
    app.set('view engine', 'jade');
    app.use(express.bodyParser());
    app.use(express.methodOverride());
    app.use(express.cookieParser('dtt'))
    app.use(express.session({ secret: 'terces'}));
    app.use(everyauth.middleware());
    // app.use('/', function(req, res, next){
    //     //disable
    //     next();
    //     return;
    //     //disable

    //     var key = req.param('api-key');
    //     if (!key) return next(error(400, 'api key required'));
    //     if (!~apiKeys.indexOf(key)) return next(error(401, 'invalid api key'));
    //     req.key = key;
    //     next();
    // });
    app.use(app.router);
    app.use(express.static(__dirname + '/public'));
});
 everyauth.helpExpress(app);

app.get('/',function(request,response){
    console.log(request.user);
    response.render('index',{locals:{title:"Auth Test"}});
});
app.configure('development', function(){
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }));
});

app.configure('production', function(){
    app.use(express.errorHandler());
});

app.listen(3000, function(){
    console.log("Express server listening on port %d in %s mode", app.address().port, app.settings.env);
});
