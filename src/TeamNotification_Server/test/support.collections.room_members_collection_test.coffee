expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomMembersCollection = module_loader.require('../support/collections/room_members_collection', {})

describe 'Room Members Collection', ->

    sut = null
    chat_room = null

    beforeEach (done) ->
        chat_room = { id: 10, name: 'blah room', users: [{id: 1, name: 'foo'}, {id: 2, name: 'bar'}] }
        sut = new RoomMembersCollection(chat_room)
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
        
        it 'should return the chat room members in the data members field', (done) ->
            members = result['members']
            users = chat_room.users
            expect(members[0]).to.eql {"href": "/user/#{users[0].id}", "data": [{"name": "id", "value": users[0].id}, {"name": "name", "value": users[0].name}]}
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
