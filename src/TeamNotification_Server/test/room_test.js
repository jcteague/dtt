var should = require('should');
var sinon = require('sinon');

var room = require('../routes/room.js');

describe('Room', function(){
    describe('build_routes', function(){
        var app;

        beforeEach(function(done){
            app = { get:sinon.spy(), post:sinon.spy() };

            room.build_routes(app)
            done();
        });

        it('should configure the routes with its corresponding callback', function(done){
            sinon.assert.calledWith(app.post,'/room',room.methods.post_room);
            sinon.assert.calledWith(app.get,'/rooms/:id',room.methods.get_room_by_id);
            done();
        });
    });

    describe('methods', function(){
        describe('get_room_by_id', function(){
            var app;
            var json_data;

            beforeEach(function(done){
                var res = { json: function(json){json_data = json}};
                app = { get:sinon.spy() };

                room.methods.get_room_by_id({},res);
                done();
            });

            it('', function(done){
                var links = json_data['links'];
                links['self']['href'].should.equal('#');
                done();
            });

        });
    });
});
