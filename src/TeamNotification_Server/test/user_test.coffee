expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock =
    core:
        entity_factory:
            get: sinon.stub()

routes_service_mock =
    build: sinon.stub()

user = module_loader.require('../routes/user.js', {
    requires:
        '../support/routes_service': routes_service_mock
})

describe 'User', ->

    describe 'build_routes', ->
        app = null

        beforeEach (done) ->
            app = { get:sinon.spy() }
            user.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/user/:id',user.methods.get_user)
            sinon.assert.calledWith(app.get,'/user/:id/rooms',user.methods.get_user_rooms)
            done()

    describe 'methods', ->

        collection = null
        collection_value = null
        collection_factory = null
        res = null
        req = null
        user_id = null

        beforeEach (done) ->
            collection_value = 'blah collection'
            collection =
                to_json: ->
                    collection_value

            collection_factory =
                for: sinon.stub()
            req =
                param: sinon.stub()
            user_id = 10
            req.param.withArgs('id').returns(user_id)
            res = 
                json: sinon.spy()
            done()
        
        describe 'get_user', ->

            beforeEach (done) ->
                routes_service_mock.build.withArgs('user_collection').returns(collection_factory)
                user_collection =
                    fetch_to: (callback) ->
                        callback(collection)
                collection_factory.for.withArgs(user_id).returns(user_collection)
                user.methods.get_user(req, res)
                done()

            it 'should return the built collection for the user model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()

        describe 'get_user_rooms', ->

            beforeEach (done) ->
                routes_service_mock.build.withArgs('user_rooms_collection').returns(collection_factory)
                user_rooms_collection =
                    fetch_to: (callback) ->
                        callback(collection)
                collection_factory.for.withArgs(user_id).returns(user_rooms_collection)
                user.methods.get_user_rooms(req, res)
                done()

            it 'should return the built collection for the user rooms model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()


