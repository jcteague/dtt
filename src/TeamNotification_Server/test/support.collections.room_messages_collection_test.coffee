return
expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomMessagesCollection = module_loader.require('../support/collections/room_messages_collection', {})

describe 'Room Messages Collection', ->

    chat_room = null
    sut = null

    beforeEach (done) ->
        chat_room = { id: 10, name: 'blah room', messages: [{id: 1, message: 'foo message', user_id: 1}, {id: 2, message: 'bar message', user_id: 2}] }
        sut = new RoomMessagesCollection(chat_room)
        done()

    describe 'constructor', ->

        it 'should have the current room value set inside the object', (done) ->
            expect(sut.room).to.equal(chat_room)
            done()

    describe 'to_json', ->

        chat_room = null
        result = null
        room = null

        beforeEach (done) ->
            sut.room = chat_room
            result = sut.to_json()
            done()
        
        it 'should return the chat room messages in the data messages field', (done) ->
            members = result['messages']
            messages = chat_room.messages
            expect(messages[0]).to.eql {"href": "/user/#{users[0].id}", "data": [{"name": "id", "value": users[0].id}, {"name": "name", "value": users[0].name}]}
            expect(members[1]).to.eql {"href": "/user/#{users[1].id}", "data": [{"name": "id", "value": users[1].id}, {"name": "name", "value": users[1].name}]}
            done()

        it 'should return a self link in the collection links', (done) ->
            expect(result['links'][0]).to.eql {"name": "self", "rel": "RoomMembers", "href": "/room/#{chat_room.id}/users"}
            done()
            
        it 'should return a query to search for users', (done) ->
            expect(result['queries']).to.have.length(1)
            done()
            
        it 'should return a query to search for users with the right format', (done) ->
            expect(result['queries'][0]).to.have.property('href', '/users/query')
            expect(result['queries'][0]).to.have.property('rel', 'users')
            expect(result['queries'][0]).to.have.property('type', 'autocomplete')
            expect(result['queries'][0]).to.have.property('submit', "/room/#{chat_room.id}/users")
            expect(result['queries'][0]['data']).to.have.length(1)
            expect(result['queries'][0]['data'][0]).to.have.property('name', 'name')
            expect(result['queries'][0]['data'][0]).to.have.property('value')
                       
            done()
