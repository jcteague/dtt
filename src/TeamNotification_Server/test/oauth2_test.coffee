should = require('should')
sinon = require('sinon')
module_loader = require('sandboxed-module')

routes_service_mock =
    build: sinon.stub()

sut = module_loader.require('../routes/oauth2', {})

describe 'OAuth2', ->

    describe 'build_routes', ->
        app  = null

        beforeEach (done) ->
            app = get:sinon.spy(), post: sinon.spy()

            sut.build_routes(app)
            done() 

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get, '/oauth2/authorize', sut.methods.authorize) 
            sinon.assert.calledWith(app.post, '/oauth2/authorize', sut.methods.post_authorize) 
            sinon.assert.calledWith(app.get, '/oauth2/token', sut.methods.get_token) 
            done() 
