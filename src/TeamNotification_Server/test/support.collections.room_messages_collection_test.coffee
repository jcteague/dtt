expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomMessagesCollection = module_loader.require('../support/collections/room_messages_collection', {})

describe 'Room Messages Collection', ->

    sut = room_id = room_messages = body = null

    beforeEach (done) ->
        room_id = 1
        body = {message: 'blah!'}
        room_messages = [
            { 
                id: 10
                body: JSON.stringify(body)
                date:'2012-06-29 11:11'
                user_id:1
                user:
                    id: 1
                    name: 'etoribio'
                    email: 'etoribio@aol.com'
                room_id: room_id
                room:
                    id:1
                    name:'The real chatroom'
                    owner_id:1
            }
        ]
        sut = new RoomMessagesCollection(room_messages)
        done()

    describe 'constructor', ->

        it 'should have the current room_messages value set inside the object', (done) ->
            expect(sut.room_messages).to.equal(room_messages)
            done()

    describe 'to_json', ->

        result = null
        room = null

        beforeEach (done) ->
            sut.room_messages = room_messages
            result = sut.to_json()
            done()

        it 'should contain a messages property', (done) ->
            expect(result).to.have.key('messages')
            done()

        it 'should contain an href property pointing to the current url', (done) ->
            expect(result['href']).to.equal "/room/#{room_id}/messages"
            done()

        it 'should return the chat room messages in the data messages field', (done) ->
            message = result['messages'][0]
            expect(message['data']).to.eql [{ 'name':'user', 'value': room_messages[0].user.name}, { 'name':'body', 'value': body.message}, { 'name':'datetime', 'value':room_messages[0].date }]
            done()

        it 'should return a links array in the collection links', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"name": "self", "rel": "RoomMessages", "href": "/room/#{room_id}/messages"}
            expect(links[1]).to.eql {"name": "Room", "rel": "Room", "href": "/room/#{room_id}"}
            done()

        it 'should contain an href property pointing to the current url', (done) ->
            template = result['template']['data']
            expect(template[0]).to.eql {'name': 'message', 'label': 'Message', 'type': 'string-big', 'maxlength': 100}
            done()
