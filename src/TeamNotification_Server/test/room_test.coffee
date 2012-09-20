expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock = 
    core:
        entity_factory: 
            create: sinon.stub()

repository_class_mock = sinon.stub()

middleware_mock = socket_io: sinon.stub()
express_mock =
    bodyParser: sinon.stub()

routes_service_mock =
    build: sinon.stub()
    add_user_to_chat_room: sinon.stub()
    is_user_in_room: sinon.stub()

redis_mock =
    open: sinon.stub()

client_mock =
    auth: sinon.stub()
    publish: sinon.stub()
    subscribe: sinon.stub()
    on: sinon.stub()
    zadd: sinon.stub()

config = require('../config')()
redis_mock.open.returns(client_mock)

sut = module_loader.require('../routes/room.js', {
    requires:
        '../support/core': support_mock
        'express': express_mock
        '../support/redis/redis_gateway': redis_mock
        '../support/routes_service': routes_service_mock
        '../support/repository': repository_class_mock
        '../support/middlewares': middleware_mock
})


describe 'Room', ->

    beforeEach (done) ->
        client_mock[key].reset() for key, val of client_mock
        done()

    describe 'when required', ->

        it 'should have created the clients for redis', (done) ->
            sinon.assert.callCount(redis_mock.open, 3)
            done()

    describe 'build_routes', ->
        app = null
        io = null
        body_parser_result = null
        socket_middleware_result = null
        beforeEach (done) ->
            app = get:sinon.spy(), post:sinon.spy()
            io = sinon.stub()
            body_parser_result = 'blah'
            socket_middleware_result = 'Awesome!'
            express_mock.bodyParser.returns(body_parser_result)
            middleware_mock.socket_io.withArgs(io).returns(socket_middleware_result)
            sut.build_routes(app, io)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/room', sut.methods.get_room)
            sinon.assert.calledWith(app.get,'/room/:id', sut.methods.user_authorized_in_room, sut.methods.get_room_by_id)
            sinon.assert.calledWith(app.get,'/room/:id/messages', sut.methods.user_authorized_in_room, socket_middleware_result, sut.methods.get_room_messages)
            sinon.assert.calledWith(app.get,'/room/:id/users', sut.methods.user_authorized_in_room, sut.methods.manage_room_members)
            sinon.assert.calledWith(app.post,'/room', body_parser_result, sut.methods.post_room)
            sinon.assert.calledWith(app.post,'/room/:id/users', sut.methods.user_authorized_in_room, body_parser_result, sut.methods.post_room_user)
            sinon.assert.calledWith(app.post,'/room/:id/messages', sut.methods.user_authorized_in_room, body_parser_result, sut.methods.post_room_message)
            done()

    describe 'methods', ->
           
        describe 'user_authorized_in_room', ->

            next = req = res = room_id = user_id = null

            beforeEach (done) ->
                user_id = 10
                room_id = 1
                req =
                    param: sinon.stub()
                    user:
                        id: user_id
                req.param.withArgs('id').returns(room_id)
                res =
                    redirect: sinon.spy()
                next = sinon.spy()
                done()

            describe 'and the user is authorized', ->

                beforeEach (done) ->
                    user_in_promise =
                        then: (callback) ->
                            callback(true)
                    routes_service_mock.is_user_in_room.withArgs(user_id, room_id).returns(user_in_promise)
                    sut.methods.user_authorized_in_room(req, res, next)
                    done()

                it 'should call the next function', (done) ->
                    sinon.assert.called(next)
                    done()

            describe 'and the user is not authorized', ->

                beforeEach (done) ->
                    user_in_promise =
                        then: (callback) ->
                            callback(false)
                    routes_service_mock.is_user_in_room.withArgs(user_id, room_id).returns(user_in_promise)
                    sut.methods.user_authorized_in_room(req, res, next)
                    done()

                it 'should call the next function', (done) ->
                    sinon.assert.calledWith(res.redirect, '/')
                    done()

        describe 'set_up_message_transmission', ->
            io = room_id = listener_name = room_channel = null

            beforeEach (done) ->
                io =
                    of: sinon.stub()
                room_id = 1
                listener_name = 'blah listener'

                namespace_io = 
                    send: sinon.stub()
                io.of.withArgs(listener_name).returns(namespace_io)

                room_channel = "chat #{room_id}"
                sut.methods.set_up_message_transmission(io, room_id, listener_name)
                done()

            it 'should subscribe to the room channel', (done) ->
                sinon.assert.calledWith(client_mock.subscribe, room_channel)
                done()

            it 'should set up the message event', (done) ->
                sinon.assert.calledWith(client_mock.on, 'message', sinon.match.func)
                done()

        describe 'set_socket_events', ->

            io = room_id = listener_name = null

            beforeEach (done) ->
                io = 'socket io'
                room_id = 9
                listener_name = "/room/#{room_id}/messages"
                sinon.stub(sut.methods, 'set_up_message_transmission')
                done()

            afterEach (done) ->
                sut.methods.is_listener_registered.restore()
                sut.methods.set_up_message_transmission.restore()
                done()

            describe 'and there is not a listener for that room', ->

                beforeEach (done) ->
                    sinon.stub(sut.methods, 'is_listener_registered').returns false
                    sut.methods.set_socket_events(io, room_id)
                    done()

                it 'should set up the message transmission for that listener', (done) ->
                    sinon.assert.calledWith(sut.methods.set_up_message_transmission, io, room_id, listener_name)
                    done()

            describe 'and there is a listener for that room', ->

                beforeEach (done) ->
                    sinon.stub(sut.methods, 'is_listener_registered').returns true
                    sut.methods.set_socket_events(io, room_id)
                    done()

                it 'should not set up the message transmission for that listener', (done) ->
                    sinon.assert.notCalled(sut.methods.set_up_message_transmission)
                    done()
                
        describe 'for a room with id', ->

            collection_factory = null
            collection = null
            collection_value = null
            res = null
            req = null
            room_id = null
            json_data = null

            beforeEach (done) ->
                collection =
                    to_json: ->
                        collection_value
                collection_factory =
                    for: sinon.stub()
                req =
                    param: sinon.stub()
                room_id = 1
                req.param.withArgs('id').returns(room_id)
                res = 
                    json: sinon.spy()
                    redirect: sinon.spy()

                done()

            describe 'get_room_by_id', ->

                user_id = null

                beforeEach (done) ->
                    user_id = 10
                    req.user =
                        id: user_id
                    routes_service_mock.build.withArgs('room_collection').returns(collection_factory)
                    room_collection =
                        fetch_to: (callback) ->
                            callback(collection)

                    collection_factory.for.withArgs(room_id: room_id, user_id: user_id).returns(room_collection)
                    sut.methods.get_room_by_id(req, res)
                    done()

                it 'should return the built collection for the room model', (done) ->
                    sinon.assert.calledWith(res.json, collection_value)
                    done()


            describe 'manage_room_members', ->

                beforeEach (done) ->
                    routes_service_mock.build.withArgs('room_members_collection').returns(collection_factory)
                    room_members_collection =
                        fetch_to: (callback) ->
                            callback(collection)

                    collection_factory.for.withArgs(room_id).returns(room_members_collection)
                    sut.methods.manage_room_members(req, res)
                    done()

                it 'should return the built collection for the room members model', (done) ->
                    sinon.assert.calledWith(res.json, collection_value)
                    done()

            describe 'get_room_messages', ->

                json_data = data = listener_name = socket_mock = null
                
                beforeEach (done) ->
                    req.user =
                        id: 9

                    routes_service_mock.build.withArgs('room_messages_collection').returns(collection_factory)
                    socket_mock = {on: (event, callback) -> callback() }
                    listener_name = "/room/#{room_id}/messages"                    
                    room_messages_collection =
                        fetch_to: (callback) ->
                            callback(collection)
                    
                    req.param.withArgs('id').returns(room_id)
                    
                    req.socket_io = {of: sinon.stub() }
                    req.socket_io.of.withArgs(listener_name).returns(socket_mock)

                    sinon.stub(sut.methods, 'set_socket_events')
                    
                    collection_factory.for.withArgs(room_id: room_id, user: req.user).returns(room_messages_collection)
                    sut.methods.get_room_messages(req, res)
                    done()

                afterEach (done) ->
                    sut.methods.set_socket_events.restore()
                    done()

                it 'should return the built collection for the room model', (done) ->
                    sinon.assert.calledWith(res.json, collection_value)
                    done()

                it 'should set the socket events with the room and the socket_io in the request', (done) ->
                    sinon.assert.calledWith(sut.methods.set_socket_events, req.socket_io, room_id)
                    done()

        describe 'post_room_message', ->

            res = null
            req = null
            room_id = 1

            beforeEach (done) ->
                res =  send: sinon.spy()
                req =  
                    param: sinon.stub()
                    user:
                        id: 2
                req.param.withArgs('id').returns(room_id)
                done()

            describe 'when all correct parameters where sent to create the message', ->

                user_id = null
                chat_room_id = null
                body = null
                room_message = null
                saved_message = null
                request_values = null

                beforeEach (done) ->
                    saved_message = 'blah message'
                    room_message = 
                        save: (callback) ->
                            callback(false, saved_message)
                    
                    req.body = {message: 'Dolorem Ipsum'}
                    message = JSON.stringify(req.body)
                    request_values =
                        body: message
                        room_id: room_id
                        user_id: req.user.id
                        date: ''

                    sinon.spy(room_message, 'save')
                    support_mock.core.entity_factory.create.returns(room_message)
                    sut.methods.post_room_message(req,res)
                    done()

                it 'should notify the message was created', (done) ->
                    sinon.assert.calledWith(res.send,{ success:true, newMessage:saved_message})
                    done()

                it 'should have asked to create the ChatRoomMessage with the correct request values', (done) ->
                    sinon.assert.called(support_mock.core.entity_factory.create, 'ChatRoomMessage', request_values)
                    done()

                it 'should publish the message in the chat room', (done) ->
                    sinon.assert.calledWith(client_mock.publish, "chat #{room_id}", sinon.match.string)
                    done()

                it 'should add the message in redis with the corresponding key', (done) ->
                    sinon.assert.calledWith(client_mock.zadd, "room:#{room_id}:messages", sinon.match.number, sinon.match.string)
                    done()

                it 'should create the message on the database', (done) ->
                    sinon.assert.called(room_message.save)
                    done()
            
            
        describe 'post_room', ->

            req = res = owner_id = null

            beforeEach (done) ->
                owner_id = 3
                res =  send: sinon.spy()
                req =
                    param: sinon.stub()
                    user:
                        id: owner_id

                done()

            describe 'when all correct parameters where sent to create the room', ->

                json_data = null
                chat_room = null
                chat_room_id = null

                beforeEach (done) ->
                    chat_room = 
                        save: (callback) ->
                            callback(false, {id: chat_room_id})

                    req.body = {name: 'blah'}
                    request_values = {name: req.body.name, owner_id: owner_id}
                    sinon.spy(chat_room, 'save')
                    support_mock.core.entity_factory.create.withArgs('ChatRoom', request_values).returns(chat_room)
                    sut.methods.post_room(req,res)
                    done()

                it 'should notify the user the room was created', (done) ->
                    sinon.assert.calledWith(res.send,"room #{chat_room_id} created")
                    done()

                it 'should create the user on the database', (done) ->
                    sinon.assert.called(chat_room.save)
                    done()

        describe 'post_room_user', ->

            user_id = null
            user_email = 'foo@zap.com'
            room_id = null
            req = null
            res = null
            promise = null
            service_message = null

            beforeEach (done) ->
                user_id = 1
                email = 
                req =
                    user:
                        id: 18
                    body:
                        id: user_id
                        email: user_email
                    param: sinon.stub()
                res =
                    send: sinon.spy()

                room_id = 10
                req.param.withArgs('id').returns(room_id)
                service_message = 'blah for the user'
                promise =
                    then: (callback) ->
                        callback(service_message)

                routes_service_mock.add_user_to_chat_room.returns(promise)
                sut.methods.post_room_user(req, res)
                done()

            it 'should call the add user to chat room', (done) ->
                sinon.assert.calledWith(routes_service_mock.add_user_to_chat_room, req.user, user_email, room_id)
                done()

            it 'should return the user exists response', (done) ->
                sinon.assert.calledWith(res.send, service_message)
                done()

        describe 'get_room', ->

            json_data = null
            req = null
            template = null
            data = null
            links = null
            beforeEach (done) ->
                res = 
                    json: (json) ->
                        json_data = json
                req =  
                    param: sinon.stub()
                room_id = 1
                req.param.withArgs('id').returns(room_id)
                sut.methods.get_room(req, res)
                template = json_data['template']
                links = json_data['links']
                data = template.data
                done()

            it 'should return the list of links', (done) ->
                expect(links[0]).to.eql {"name": "self", "rel": "Room", "href": "/room"}
                done()

            it 'should return the template with the fields', (done) ->
                expect(data).not.to.be.empty
                done()

            it 'should contain the name field in the template data', (done) ->
                field = (field for field in data when field.name is 'name')[0]
                expect(field.name).to.equal 'name'
                expect(field.label).to.equal 'Chatroom Name'
                expect(field.type).to.equal 'string'
                done()

            it 'should contain the owner id field in the template data', (done) ->
                field = (field for field in data when field.name is 'owner_id')[0]
                expect(field.name).to.equal 'owner_id'
                expect(field.type).to.equal 'hidden'
                done()

            it 'should contain the fields with name, type and label properties in the template data', (done) ->
                for field in data
                    expect(field).to.have.keys(['name', 'label', 'type'])
                done()   
