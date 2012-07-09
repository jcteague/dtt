expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock = 
    core:
        entity_factory: 
            create: sinon.stub()

repository_class_mock = sinon.stub()
express_mock =
    bodyParser: sinon.stub()

routes_service_mock =
    build: sinon.stub()
    add_user_to_chat_room: sinon.stub()
    is_user_in_room: sinon.stub()

sut = module_loader.require('../routes/room.js', {
    requires:
        '../support/core': support_mock
        'express': express_mock
        '../support/routes_service': routes_service_mock
        '../support/repository': repository_class_mock
})

describe 'Room', ->

    describe 'build_routes', ->
        app = null
        body_parser_result = null

        beforeEach (done) ->
            app = get:sinon.spy(), post:sinon.spy()
            body_parser_result = 'blah'
            express_mock.bodyParser.returns(body_parser_result)
            sut.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/room',sut.methods.get_room)
            sinon.assert.calledWith(app.get,'/room/:id',sut.methods.get_room_by_id)
            sinon.assert.calledWith(app.get,'/room/:id/messages',sut.methods.get_room_messages)
            sinon.assert.calledWith(app.get,'/room/:id/users',sut.methods.manage_room_members)
            sinon.assert.calledWith(app.post,'/room', body_parser_result, sut.methods.post_room)
            sinon.assert.calledWith(app.post,'/room/:id/users', body_parser_result, sut.methods.post_room_user)
            sinon.assert.calledWith(app.post,'/room/:id/messages',body_parser_result, sut.methods.post_room_message)
            done()

    describe 'methods', ->

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
                    done()

                describe 'and the user is in the room', ->

                    beforeEach (done) ->
                        in_room_promise =
                            then: (callback) ->
                                callback(true)
                        routes_service_mock.is_user_in_room.withArgs(user_id, room_id).returns(in_room_promise)
                        routes_service_mock.build.withArgs('room_collection').returns(collection_factory)
                        room_collection =
                            fetch_to: (callback) ->
                                callback(collection)

                        collection_factory.for.withArgs(room_id).returns(room_collection)
                        sut.methods.get_room_by_id(req, res)
                        done()

                    it 'should return the built collection for the room model', (done) ->
                        sinon.assert.calledWith(res.json, collection_value)
                        done()

                describe 'and the user is not in the room', ->

                    beforeEach (done) ->
                        in_room_promise =
                            then: (callback) ->
                                callback(false)
                        routes_service_mock.is_user_in_room.withArgs(user_id, room_id).returns(in_room_promise)
                        sut.methods.get_room_by_id(req, res)
                        done()

                    it 'should redirect the user to the root path', (done) ->
                        sinon.assert.calledWith(res.redirect, '/')
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

                json_data = data = null

                beforeEach (done) ->
                    routes_service_mock.build.withArgs('room_messages_collection').returns(collection_factory)
                    room_messages_collection =
                        fetch_to: (callback) ->
                            callback(collection)
                    req.param.withArgs('id').returns(room_id)
                    
                    collection_factory.for.withArgs(room_id).returns(room_messages_collection)
                    sut.methods.get_room_messages(req, res)
                    done()

                it 'should return the built collection for the room model', (done) ->
                    sinon.assert.calledWith(res.json, collection_value)
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

                it 'should notify the user the room was created', (done) ->
                    sinon.assert.calledWith(res.send,{ success:true, newMessage:saved_message})
                    done()

                it 'should have asked to create the ChatRoomMessage with the correct request values', (done) ->
                    sinon.assert.called(support_mock.core.entity_factory.create, 'ChatRoomMessage', request_values)
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
            room_id = null
            req = null
            res = null
            promise = null
            service_message = null

            beforeEach (done) ->
                user_id = 1
                req =
                    body:
                        id: user_id
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
                sinon.assert.calledWith(routes_service_mock.add_user_to_chat_room, user_id, room_id)
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
