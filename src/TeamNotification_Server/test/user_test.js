var should = require('should');
var sinon = require('sinon');

var user = require('../routes/user.js');

describe('User', function(){
    describe('build_routes', function(){
        var app;

        beforeEach(function(done){
            app = { get:sinon.spy() };

            user.build_routes(app)
            done();
        });

        it('should configure the routes with its corresponding callback', function(done){
            sinon.assert.calledWith(app.get,'/user',user.methods.get_user);
            sinon.assert.calledWith(app.get,'/user/rooms',user.methods.get_user_rooms);
            done();
        });
    });

    describe('methods', function(){
        describe('get_user', function(){
            var app;
            var json_data;

            beforeEach(function(done){
                var res = { json: function(json){json_data = json}};
                app = { get:sinon.spy() };

                user.methods.get_user({},res);
                done();
            });

            it('should return the correct links for the user model', function(done){
                var links = json_data['links'];
                links['self']['href'].should.equal('/user');
                links['rooms']['href'].should.equal('/user/rooms');
                done();
            });

        });

        describe('get_user_rooms', function(){
            var app;
            var json_data;

            beforeEach(function(done){
                var res = { json: function(json){json_data = json}};
                app = { get:sinon.spy() };

                user.methods.get_user_rooms({},res);
                done();
            });

            it('should return the correct links for the user rooms model', function(done){
                var links = json_data['links'];
                links['self']['href'].should.equal('/user/rooms');
                links['OpenRoom1']['href'].should.equal('/rooms/1');
                done();
            });
        });
    });
});
