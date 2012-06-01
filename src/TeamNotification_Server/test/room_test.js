var should = require('should');
var sinon = require('sinon');

var room = require('../routes/room.js');
var support = require('support').core;

describe('Room', function(){
    describe('build_routes', function(){
        var app;

        beforeEach(function(done){
            app = { get:sinon.spy(), post:sinon.spy() };

            room.build_routes(app)
            done();
        });

        it('should configure the routes with its corresponding callback', function(done){ sinon.assert.calledWith(app.post,'/room',room.methods.post_room);
            sinon.assert.calledWith(app.get,'/rooms/:id',room.methods.get_room_by_id);
            done();
        });
    });

    describe('methods', function(){
        describe('get_room_by_id', function(){
            var json_data;

            beforeEach(function(done){
                var res = { json: function(json){json_data = json}};

                room.methods.get_room_by_id({},res);
                done();
            });

            it('should return as a json object all the links for the current room', function(done){
                var links = json_data['links'];
                links['self']['href'].should.equal('#');
                done();
            });

        });

        describe('post_room', function(){
            var json_data;
            var res;
            var req;
            var db_client;

            beforeEach(function(done){
                res = { send: sinon.spy()};
                req = {
                    param: function(param_name){
                        param_name.should.equal('name');
                        return 'blah name';
                    }
                };

                db_client = {query: sinon.spy()};
                support.create_db_client = function(){ return db_client; }
                room.methods.post_room(req,res);
                done();
            });

            it('should notify the user the room was created', function(done){
                sinon.assert.calledWith(res.send,'room created');
                done();
            });

            it('should create the user on the database', function(done){
                sinon.assert.calledWith(db_client.query,'INSERT INTO chat_room(name) VALUES($1)', [ 'blah name' ]);
                done();
            });
        });
    });
});
