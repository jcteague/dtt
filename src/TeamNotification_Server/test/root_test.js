var should = require('should');
var sinon = require('sinon');

var root = require('../routes/root.js');

describe('Root', function(){
    describe('build_routes', function(){
        var app;

        beforeEach(function(done){
            app = { get:sinon.spy() };

            root.build_routes(app)
            done();
        });

        it('should configure the routes with its corresponding callback', function(done){
            sinon.assert.calledWith(app.get,'/',root.methods.get_root);
            done();
        });
    });

    describe('methods', function(){
        describe('get_root', function(){
            var app;
            var json_data;

            beforeEach(function(done){
                var res = { json: function(json){json_data = json}};
                app = { get:sinon.spy() };

                root.methods.get_root({},res);
                done();
            });

            it('should return as a json all the links for the root path', function(done){
                var links = json_data['links'];
                links['self']['href'].should.equal('/');
                links['user']['href'].should.equal('/user');
                done();
            });

        });
    });
});
