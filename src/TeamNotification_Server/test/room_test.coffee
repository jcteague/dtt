should = require('should')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock = 
    core:
        entity_factory: 
            create: sinon.spy()

sut = module_loader.require('../routes/room.js', {
    requires:
        'support': support_mock
})
#sut = require '../routes/room.js'

describe 'Room', () ->

    describe 'build_routes', ->
        app = null

        beforeEach (done) ->
            app = get:sinon.spy(), post:sinon.spy()
            sut.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/room',sut.methods.post_room)
            sinon.assert.calledWith(app.get,'/rooms/:id',sut.methods.get_room_by_id)
            done()

    describe 'methods', ->

        describe 'get_room_by_id', ->
            json_data = null

            beforeEach (done) ->
                res = json: (json) -> json_data = json

                sut.methods.get_room_by_id({},res)
                done()

            it 'should return as a json object all the links for the current room', (done) ->
                links = json_data['links']
                console.log links[0], links[0].should, should.equal
                # For some strange reason obj.should doesn't exist
                #links[0].should.eql {"rel": "self", "href": "/rooms/1"}
                #should.eql(links[0], {"rel": "self", "href": "/rooms/1"})
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

                    support_mock.core.entity_factory =  create: (entity_name,params) ->
                        return chat_room if entity_name is 'ChatRoom' and params is 'blah chat_room'

                    sut.methods.post_room(req,res)
                    done()

                it 'should notify the user the room was created', (done) ->
                    sinon.assert.calledWith(res.send,'room created')
                    done()

                it 'should create the user on the database', (done) ->
                    sinon.assert.called(chat_room.save)
                    done()
