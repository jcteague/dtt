expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock = 
    core:
        entity_factory: 
            create: sinon.stub()

express_mock =
    bodyParser: sinon.stub()


sut = module_loader.require('../routes/room.js', {
    requires:
        'support': support_mock
        'express': express_mock
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
            sinon.assert.calledWith(app.post,'/room', body_parser_result, sut.methods.post_room)
            sinon.assert.calledWith(app.get,'/rooms/:id',sut.methods.get_room_by_id)
            sinon.assert.calledWith(app.get,'/room',sut.methods.get_room)
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

                    sinon.spy(chat_room, 'save')
                    support_mock.core.entity_factory.create.returns(chat_room)
                    req.param.withArgs('chat_room').returns('blah chat_room')
                    sut.methods.post_room(req,res)
                    done()

                it 'should notify the user the room was created', (done) ->
                    sinon.assert.calledWith(res.send,"room #{chat_room_id} created")
                    done()

                it 'should create the user on the database', (done) ->
                    sinon.assert.called(chat_room.save)
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
                expect(links).not.to.be.empty
                done()

            it 'should return the template with the fields', (done) ->
                expect(data).not.to.be.empty
                done()

            it 'should contain the name field in the template data', (done) ->
                fields = (field.name for field in data)
                expect(fields).to.contain 'name'
                done()
