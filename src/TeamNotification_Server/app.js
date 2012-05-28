var express = require('express');
var app= express.createServer();
app.use(express.bodyParser());
app.use(express.methodOverride());

app.options('/',function(req,res){
    var r = {
        get: [ { type: 'rooms', url: '/user/rooms'} ], post: [ { type: 'room', url: '/room'} ]
    };

    send(req,res,r);
});

function send(req,res,object_to_return){
    var expand = req.body.expand;
    console.log(expand);
    var expantion_url = get_expantion_url(object_to_return,expand);
    console.log(expantion_url);
    if(expantion_url){
        var expantion_function = app.match(expantion_url)[0].callbacks[0];
        var expantion_object;
        var fake_res = {
            send: function(expanded_object) {
                      expantion = expanded_object;
                  }
        };

        expantion_function(req,fake_res);
        object_to_return[expand] = expantion;
    }

    res.send(object_to_return);
}

function get_expantion_url( result, action_type ){
    var array = result.get;
    var length = array.length;
    var obj;

    while( length-- ){
        obj = array[ length ];
        if( obj.type == action_type ) return obj.url;
    }

    return null;
}

app.get('/user/rooms',function(req,res){
    var r = ["room1","room2"];
    res.send(r);
});

app.listen(3000);



