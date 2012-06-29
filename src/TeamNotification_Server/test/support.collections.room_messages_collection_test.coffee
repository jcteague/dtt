expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomMessagesCollection = module_loader.require('../support/collections/room_messages_collection', {})

describe 'Room Messages Collection', ->

    room_messages = null
    sut = null

    beforeEach (done) ->
        room_messages = [{ id: 10, body: 'blah!', date:'2012-06-29 11:11', user_id:1, user: {id: 1, name: 'etoribio', email: 'etoribio@aol.com'}, room_id:1, room:{ id:1, name:'The real chatroom', owner_id:1 } }]
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
            #console.log  room_messages
            sut.room_messages = room_messages
            result = sut.to_json()
            done()
        it 'should contain a messages property', (done) ->
            expect(result).to.have.key('messages')
            done()
        it 'should return the chat room messages in the data messages field', (done) ->
            message = result['messages'][0]
            expect(message['data']).to.eql [{ 'name':'user', 'value': room_messages[0].user.name}, { 'name':'body', 'value': room_messages[0].body}, { 'name':'datetime', 'value':room_messages[0].date }]
            done()

        it 'should return a self link in the collection links', (done) ->
            expect(result['links'][0]).to.eql {"name": "self", "rel": "Room Messages", "href": "/room/#{room_messages[0].room_id}/messages"}
            done()
            
