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
            sinon.assert.calledWith(app.get,'/users/:username?',user.methods.get_users)
            done()

    describe 'methods', ->

        collection = null
        collection_value = null
        collection_factory = null
        res = null
        req = null
        user_id = null
        username = null

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
            req.param.withArgs('username').returns(username)
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

        describe 'get_users', ->
            users_array = null        
            json_data = null
            beforeEach (done) ->
                routes_service_mock.build.withArgs('users_collection').returns(collection_factory)
                users_collection = 
                    fetch_to: (callback) ->
                        callback(collection_value)
                collection_factory.for.withArgs(username).returns(users_collection)
                user.methods.get_users(req,res)
                done()
            it 'should return the built collection for the users model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()

        ###

        describe 'get_user_rooms', ->

            app = null
            json_data = null
            chat_rooms = null
            chat_room_entity = null

            beforeEach (done) ->
                chat_rooms = [
                    {id: 1, name: 'foo'}
                    {id: 2, name: 'bar'}
                ]
                chat_room_entity =
                    find: (condition, order, callback) ->
                        callback(chat_rooms)

                sinon.spy(chat_room_entity, 'find')
                support_mock.core.entity_factory.get.withArgs('ChatRoom').returns(chat_room_entity)
                res = 
                    json: (json) -> json_data = json
                app = get: sinon.spy()

                user.methods.get_user_rooms({},res)
                done()

            it 'should return the correct links for the user rooms model', (done) ->
                links = json_data['links']
                expect(links[0]).to.eql {"rel": "self", "href": "/user/rooms"}
                expect(links[1]).to.eql {"rel": chat_rooms[0].name, "href": "/rooms/#{chat_rooms[0].id}"}
                expect(links[2]).to.eql {"rel": chat_rooms[1].name, "href": "/rooms/#{chat_rooms[1].id}"}
                done()

            it 'should fetch the entities ordered by ids', (done) ->
                sinon.assert.calledWith(chat_room_entity.find, '', 'id asc')
                done()
        ###
