var should = require('should');
var sinon = require('sinon');

var module_loader = require('./support/module_loader');
var httpMock = {
    get: sinon.stub()
};
//var sut = module_loader.loadModule('./routes/client.js', {http: httpMock});
var sut = module_loader.loadModule('./routes/client.js', {http: httpMock}).module.exports;
console.log(sut);

describe('Client', function(){
    describe('build_routes', function(){
        var app;

        beforeEach(function(done){
            app = { get:sinon.spy() };

            sut.build_routes(app)
            done();
        });

        it('should configure the routes with its corresponding callback', function(done){
            sinon.assert.calledWith(app.get,'/client', sut.methods.get_client);
            done();
        });
    });

    describe('methods', function(){
        describe('get_client', function(){
            var app;

            beforeEach(function(done){
                var res = { json: function(json){json_data = json}};
                app = { get:sinon.spy() };

                sut.methods.get_client({},res);
                done();
            });

            it('should render the client template with the param data', function(done){
                done();
            });

        });
    });
});

