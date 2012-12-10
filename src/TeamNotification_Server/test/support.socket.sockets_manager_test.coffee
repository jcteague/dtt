expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')


client_mock =
    auth: sinon.stub()
    publish: sinon.stub()
    subscribe: sinon.spy()
    on: sinon.spy()
    zadd: sinon.stub()


describe 'sockets manager tests', ->
    sut = null
    beforeEach (done)->
        sut = module_loader.require('../support/socket/sockets_manager')
        done()
    describe 'setup_message_transmission', ->
        io = listener_name = null

        beforeEach (done) ->
            io =
                of: sinon.stub()
            listener_name = 'blah listener'

            namespace_io =
                send: sinon.stub()
            io.of.withArgs(listener_name).returns(namespace_io)

            sut.setup_message_transmission(io, listener_name, client_mock)
            done()

        it 'should subscribe to the room channel', (done) ->
            sinon.assert.calledWith(client_mock.subscribe, listener_name)
            done()

        it 'should set up the message event', (done) ->
            sinon.assert.calledWith(client_mock.on, 'message', sinon.match.func)
            done()
    return
    describe 'set_socket_events', ->

        io = room_id = listener_name = null

        beforeEach (done) ->
            io = 'socket io'
            room_id = 9
            listener_name = "/api/room/#{room_id}/messages"
            sinon.stub(sut, 'setup_message_transmission')
            done()

        describe 'and there is not a listener for that room', ->

            beforeEach (done) ->
                sinon.stub(sut, 'is_listener_registered').returns false
                sut.set_socket_events(io, listener_name, client_mock)
                done()

            it 'should set up the message transmission for that listener', (done) ->
                sinon.assert.calledWith(sut.setup_message_transmission, io, listener_name, client_mock)
                done()

        describe 'and there is a listener for that room', ->

            beforeEach (done) ->
                listener_name = "/api/room/#{room_id}/messages"
                sinon.stub(sut, 'is_listener_registered').returns true
                sut.set_socket_events(io, listener_name, client_mock)
                done()

            it 'should not set up the message transmission for that listener', (done) ->
                sinon.assert.notCalled(sut.setup_message_transmission)
                done()
