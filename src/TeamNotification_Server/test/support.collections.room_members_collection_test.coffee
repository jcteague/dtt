expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    when: sinon.spy()

repository_class_mock = sinon.stub()
underscore_mock = 
    bindAll: sinon.stub()

RoomMembersCollection = module_loader.require('../support/collections/room_members_collection', {
    requires:
        'q': q_mock
        'underscore': underscore_mock
        '../repository': repository_class_mock
})

describe 'Room Members Collection', ->

    sut = null
    room_id = null
    promise_then = null
    chat_room_members_repository = null

    beforeEach (done) ->
        room_id = 1
        chat_room_members_repository =
            find: sinon.stub()

        repository_class_mock.withArgs('ChatRoom').returns(chat_room_members_repository)
        promise_then =
            then: sinon.stub()
        chat_room_members_repository.find.withArgs({id: room_id}).returns(promise_then)
        sut = new RoomMembersCollection(room_id)
        done()

    describe 'constructor', ->

        collection_promise = null

        beforeEach (done) ->
            collection_promise = 'blah promise'
            promise_then.then.withArgs(sut.set_collection).returns(collection_promise)
            sut.constructor(room_id)
            done()

        it 'should have the current room id value set inside the object', (done) ->
            expect(sut.room_id).to.equal(room_id)
            done()

        it 'should bind all the methods to the sut', (done) ->
            sinon.assert.calledWith(underscore_mock.bindAll, sut)
            done()

        it 'should set the collection with the repository result for the chat room members of the room', (done) ->
            expect(sut.collection).to.equal collection_promise
            done()

        it 'should initialize a repository for the chat_rooms', (done) ->
            expect(repository_class_mock.calledWithNew()).to.be true
            expect(sut.repository).to.equal chat_room_members_repository
            done()

    describe 'set_collection', ->

        chat_rooms = null
        result = null
        room_id = null

        beforeEach (done) ->
            room_id = 10
            sut.room_id = room_id
            chat_rooms = [ { id: sut.room_id, name: 'blah room', users: [{id: 1, name: 'foo'}, {id: 2, name: 'bar'}] } ]
            result = sut.set_collection(chat_rooms)
            done()
        
        it 'should return the chat room members in the data members field', (done) ->
            members = result['members']
            users = chat_rooms[0].users
            expect(members[0]).to.eql {"href": "/user/#{users[0].id}", "data": [{"name": "id", "value": users[0].id}, {"name": "name", "value": users[0].name}]}
            expect(members[1]).to.eql {"href": "/user/#{users[1].id}", "data": [{"name": "id", "value": users[1].id}, {"name": "name", "value": users[1].name}]}
            done()

        it 'should return a self link in the collection links', (done) ->
            expect(result['links']).to.eql {"name": "self", "rel": "RoomMembers", "href": "/room/#{room_id}/users"}
            done()

    describe 'fetch to', ->

        callback = null

        beforeEach (done) ->
            callback = sinon.spy()
            sut.collection = 'blah promise'
            sut.fetch_to callback
            done()

        it 'should call the callback with the collection', (done) ->
            sinon.assert.calledWith(q_mock.when, sut.collection, callback)
            done()

