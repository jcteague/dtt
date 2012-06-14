expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock = 
    core:
        entity_factory: 
            create: sinon.stub()


sut = module_loader.require('../routes/room.js', {
    requires:
        'support': support_mock
})

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
                res = json: (json) -> 
                    json_data = json

                sut.methods.get_room_by_id({},res)
                done()

            it 'should return as a json object all the links for the current room', (done) ->
                links = json_data['links']
                expect(links[0]).to.eql {"rel": "self", "href": "/rooms/1"}
                done()


        describe 'post_room', ->

            describe 'when all correct parameters where sent to create the room', ->
                json_data = null
                res = null
                chat_room = null
                req = null
                chat_room_id = null

                beforeEach (done) ->
                    chat_room = 
                        save: (callback) ->
                            callback(false, {id: chat_room_id})

                    sinon.spy(chat_room, 'save')

                    support_mock.core.entity_factory.create.returns(chat_room)
                    res =  send: sinon.spy()
                    req =
                        param: sinon.stub()
                    req.param.withArgs('chat_room').returns('blah chat_room')


                    sut.methods.post_room(req,res)
                    done()

                it 'should notify the user the room was created', (done) ->
                    sinon.assert.calledWith(res.send,"room #{chat_room_id} created")
                    done()

                it 'should create the user on the database', (done) ->
                    sinon.assert.called(chat_room.save)
                    done()
