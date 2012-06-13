should = require('should')
sinon = require('sinon')

#Commented need to figure out how to mock database connection
return

room = require('../routes/room.js')
support = require('support').core

describe 'Room', () ->

    describe 'build_routes', ->
        app = null

        beforeEach (done) ->
            app = get:sinon.spy(), post:sinon.spy()
            room.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/room',room.methods.post_room)
            sinon.assert.calledWith(app.get,'/rooms/:id',room.methods.get_room_by_id)
            done()

    describe 'methods', ->

        describe 'get_room_by_id', ->
            json_data = null

            beforeEach (done) ->
                res = json: (json) -> json_data = json

                room.methods.get_room_by_id({},res)
                done()

            it 'should return as a json object all the links for the current room', (done) ->
                links = json_data['links']
                links['self']['href'].should.equal('#')
                done()


        describe 'post_room', ->

            describe 'when all correct parameters where sent to create the room', ->
                json_data = null
                res = null
                chat_room = null
                req = null

                beforeEach (done) ->
                    chat_room = save:sinon.spy()
                    res =  send: sinon.spy()
                    req = 
                        param: (param_name) -> 
                            return 'blah chat_room' if param_name == 'chat_room'

                    support.entity_factory =  create: (entity_name,params) ->
                        return chat_room if entity_name is 'ChatRoom' and params is 'blah chat_room'
                    room.methods.post_room(req,res)
                    done()

                it 'should notify the user the room was created', (done) ->
                    sinon.assert.calledWith(res.send,'room created')
                    done()

                it 'should create the user on the database', (done) ->
                    sinon.assert.called(chat_room.save)
                    done()
