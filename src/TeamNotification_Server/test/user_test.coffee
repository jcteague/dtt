expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock =
    core:
        entity_factory:
            get: sinon.stub()

user = module_loader.require('../routes/user.js', {
    requires:
        'support': support_mock
})

describe 'User', ->

    describe 'build_routes', ->
        app = null

        beforeEach (done) ->
            app = { get:sinon.spy() }
            user.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/user',user.methods.get_user)
            sinon.assert.calledWith(app.get,'/user/rooms',user.methods.get_user_rooms)
            done()

    describe 'methods', ->
        
        describe 'get_user', ->
            app = null
            json_data = null

            beforeEach (done) ->
                res = 
                    json: (json) -> json_data = json
                app = { get:sinon.spy() }

                user.methods.get_user({},res)
                done()

            it 'should return the correct links for the user model', (done) ->
                links = json_data['links']
                expect(links[0]).to.eql {"rel": "self", "href": "/user"}
                expect(links[1]).to.eql {"rel": "rooms", "href": "/user/rooms"}
                done()

        describe 'get_user_rooms', ->

            app = null
            json_data = null
            chat_rooms = null

            beforeEach (done) ->
                chat_rooms = [
                    {id: 1, name: 'foo'}
                    {id: 2, name: 'bar'}
                ]
                chat_room_entity =
                    find: (callback) ->
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
