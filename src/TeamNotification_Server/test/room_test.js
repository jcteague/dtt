var should = require('should');
var sinon = require('sinon');

//Commented need to figure out how to mock database connection
return;

var room = require('../routes/room.js');
var support = require('support').core;

describe('Room', function(){
    describe('build_routes', function(){
        var app;

        beforeEach(function(done){
            app = { get:sinon.spy(), post:sinon.spy() };

            room.build_routes(app);
            done();
        });

        it('should configure the routes with its corresponding callback', function(done){
            sinon.assert.calledWith(app.get,'/room',room.methods.post_room);
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
            describe('when all correct parameters where sent to create the room',function(){
                var json_data;
                var res;
                var chat_room;
                var req;

                beforeEach(function(done){
                    chat_room = {save:sinon.spy()};
                    res = { send: sinon.spy()};
                    req = {
                        param: function(param_name){
                            if(param_name === 'chat_room') return 'blah chat_room';
                        }
                    };

                    support.entity_factory = { create: function(entity_name,params){
                        if(entity_name ==='ChatRoom' && params === 'blah chat_room') return chat_room;
                    }};
                    room.methods.post_room(req,res);
                    done();
                });

                it('should notify the user the room was created', function(done){
                    sinon.assert.calledWith(res.send,'room created');
                    done();
                });

                it('should create the user on the database', function(done){
                    sinon.assert.called(chat_room.save);
                    done();
                });
            });
        });
    });
});
