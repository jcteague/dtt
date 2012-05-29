module.exports = function(app){
    app.options('/',function(req, res){
        var r = {  get: [ { type: 'rooms', url: '/user/rooms'} ], post: [ { type: 'room', url: '/room'} ]};
        res.send_expanded(req,r);
    });
};

