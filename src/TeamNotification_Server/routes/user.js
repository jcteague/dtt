module.exports = function(app){
    app.get('/user/rooms',function(req, res){
        var r = ["room1","room2"];
        res.send(r);
    });
};

