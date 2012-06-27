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
            done()

    describe 'methods', ->

        describe 'for a room with id', ->

            collection_factory = null
            collection = null
            collection_value = null
            res = null
            req = null
            room_id = null

            beforeEach (done) ->
                collection_value = 'blah collection'
                collection =
                    to_json: ->
                        collection_value

                collection_factory =
                    for: sinon.stub()
                req =
                    param: sinon.stub()
                room_id = 10
                req.param.withArgs('id').returns(room_id)
                res = 
                    json: sinon.spy()

                done()

            describe 'get_room_by_id', ->

                beforeEach (done) ->
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

        describe 'post_room', ->

            res = null
            req = null

            beforeEach (done) ->
                res =  send: sinon.spy()
                req =
                    param: sinon.stub()

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
                    request_values = {name: req.body.name, owner_id: 1}
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
           
        ###
        describe 'get_room_messages', ->
            collection_factory = null
            messages = null
            room_id = 1
            json_data = null
            beforeEach (done) ->
                #routes_service_mock.build.withArgs('room_message_collection').returns(collection_factory)
                #room_messages_collection =
               #     fetch_to: (callback) ->
               #         callback(collection)
                res = 
                    json: (json) ->
                        json_data = json
                
                req =
                    param: ()->
                    body:
                        room_id: room_id
                req.param.withArgs('id').returns(room_id)
                sut.methods.get_room_messages(req, res)
                messages = json_data['messages']
                done()

            it 'should show all messages from a given room', (done) ->
                expect(messages).to.have.length(1)
                expect(messages['data']).to.have.length(1)
                for messagedata in messages['data']
                    expect(messagedata).to.keys(['name','value'])
                done()
        ###
            
