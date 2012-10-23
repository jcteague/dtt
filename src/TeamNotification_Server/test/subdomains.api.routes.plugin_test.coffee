should = require('should')
sinon = require('sinon')
module_loader = require('sandboxed-module')

routes_service_mock =
    build: sinon.stub()

sut = module_loader.require('../subdomains/api/routes/plugin', {
    requires:
        '../../../support/routes_service': routes_service_mock
})

describe 'Plugin', ->

    describe 'build_routes', ->
        app  = null

        beforeEach (done) ->
            app = { get:sinon.spy() } 

            sut.build_routes(app)
            done() 

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/plugin/version', sut.methods.get_plugin_version) 
            done() 

    describe 'methods', ->

        describe 'get_plugin_version', ->

            res = collection_value = null

            beforeEach (done) ->
                collection_value = 'blah collection'
                collection =
                    to_json: ->
                        collection_value

                collection_action =
                    fetch_to: (callback) ->
                        callback(collection)

                routes_service_mock.build.withArgs('plugin_collection').returns(collection_action)
                res = 
                    json: sinon.spy()
                req = null

                sut.methods.get_plugin_version(req, res)
                done()

            it 'should return the built collection for the plugin', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()
