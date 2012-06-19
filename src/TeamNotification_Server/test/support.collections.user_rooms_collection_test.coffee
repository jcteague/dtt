expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    when: sinon.spy()

repository_class_mock = sinon.stub()
underscore_mock = 
    bindAll: sinon.stub()

UserRoomsCollection = module_loader.require('../support/collections/user_rooms_collection', {
    requires:
        'q': q_mock
        'underscore': underscore_mock
        '../repository': repository_class_mock
})

describe 'User Rooms Collection', ->

    sut = null
    user_id = null
    chat_rooms_repository = null
    promise_then = null

    beforeEach (done) ->
        user_id = 1
        chat_rooms_repository =
            find: sinon.stub()

        repository_class_mock.withArgs('ChatRoom').returns(chat_rooms_repository)
        promise_then =
            then: sinon.stub()
        chat_rooms_repository.find.withArgs({owner_id: user_id}).returns(promise_then)
        sut = new UserRoomsCollection user_id
        done()

    describe 'constructor', ->

        collection_promise = null

        beforeEach (done) ->
            collection_promise = 'blah promise'
            promise_then.then.withArgs(sut.set_collection).returns(collection_promise)
            sut.constructor(user_id)
            done()

        it 'should have the current user id value set inside the object', (done) ->
            expect(sut.user_id).to.equal(user_id)
            done()

        it 'should bind all the methods to the sut', (done) ->
            sinon.assert.calledWith(underscore_mock.bindAll, sut)
            done()

        it 'should set the collection with the repository result for the chat rooms of the user', (done) ->
            expect(sut.collection).to.equal collection_promise
            done()

        it 'should initialize a repository for the chat_rooms', (done) ->
            expect(repository_class_mock.calledWithNew()).to.be true
            expect(sut.repository).to.equal chat_rooms_repository
            done()

    describe 'set_collection', ->

        db_promise = null
        chat_rooms = null
        result  = null

        beforeEach (done) ->
            sut.user_id = 1
            chat_rooms = [ { id:1, name:'happy place', owner_id:1 },{id:2, name:'somewhere', owner_id:1 }]
            result = sut.set_collection(chat_rooms)
            done()
        
        it 'should return the chatroom collection to parsed to json', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"name":"self", "rel": "self", "href": "/user/1/rooms"}
            expect(links[1]).to.eql {"name":"#{chat_rooms[0].name}",  "rel": chat_rooms[0].name, "href": "/room/#{chat_rooms[0].id}"}
            expect(links[2]).to.eql {"name":"#{chat_rooms[1].name}",  "rel": chat_rooms[1].name, "href": "/room/#{chat_rooms[1].id}"}

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

