should = require('should')
sinon = require('sinon')
module_loader = require('sandboxed-module')

routes_service_mock =
    build: sinon.stub()

sut = module_loader.require('../subdomains/api/routes/root', {
    requires:
        '../../../support/routes_service': routes_service_mock
})

describe 'Root', ->

    describe 'build_routes', ->
        app  = null

        beforeEach (done) ->
            app = { get:sinon.spy() } 

            sut.build_routes(app)
            done() 

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/', sut.methods.get_root) 
            done() 

    describe 'methods', ->

        describe 'get_root', ->

            collection_value = null
            res = null

            beforeEach (done) ->
                user_id = 3
                collection_value = 'blah collection'
                collection =
                    to_json: ->
                        collection_value

                collection_action =
                    fetch_to: (callback) ->
                        callback(collection)
                routes_service_mock.build.withArgs('root_collection').returns(collection_action)

                res = 
                    json: sinon.spy()
                req =
                    user:
                        id: user_id
                sut.methods.get_root(req, res)
                done()

            it 'should return the built collection for the root model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()
