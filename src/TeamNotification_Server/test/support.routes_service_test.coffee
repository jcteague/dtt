expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

collection_action_resolver_class_mock = sinon.spy()
repository_class_mock = sinon.stub()
q_mock = 
    defer: sinon.stub()

sut = module_loader.require('../support/routes_service', {
    requires:
        './collection_action_resolver': collection_action_resolver_class_mock
        './repository': repository_class_mock
        'q': q_mock
})

describe 'Routes Service', ->

    describe 'build', ->

        collection_type = null
        result = null

        beforeEach (done) ->
            result = sut.build collection_type
            done()

        it 'should return a collection action resolver using the collection type passed', (done) ->
            expect(collection_action_resolver_class_mock.calledWithNew()).to.be true
            sinon.assert.calledWith(collection_action_resolver_class_mock, collection_type)
            done()

    describe 'add_user_to_chat_room', ->

        chat_room = null
        user = null
        user_id = null
        room_id = null
        users_repository = null
        deferred = null
        callback = null
        expected_result = null
        result = null

        beforeEach (done) ->
            user_id = 1
            room_id = 2

            chat_room_repository =
                get_by_id: sinon.stub()
            users_repository =
                get_by_id: sinon.stub()

            repository_class_mock.withArgs('ChatRoom').returns(chat_room_repository)
            repository_class_mock.withArgs('User').returns(users_repository)

            chat_room =
                addUsers: (any, callback) ->
                    callback()
                save: sinon.spy()

            sinon.spy(chat_room, 'addUsers')
            chat_room_promise =
                then: (callback) ->
                    callback(chat_room)
            chat_room_repository.get_by_id.withArgs(room_id).returns(chat_room_promise)

            deferred =
                resolve: sinon.spy()
                promise: 'blah promise'
            q_mock.defer.returns(deferred)

            expected_result = deferred.promise
            done()

        describe 'and the user exists', ->

            beforeEach (done) ->
                user =
                    id: user_id
                user_promise =
                    then: (callback) ->
                        callback(user)
                users_repository.get_by_id.withArgs(user_id).returns(user_promise)
                done()

            describe 'and the user is not in the room', ->

                beforeEach (done) ->
                    chat_room['users'] = [{id: 100}]
                    result = sut.add_user_to_chat_room(user_id, room_id)
                    done()

                it 'should create the user on the database', (done) ->
                    sinon.assert.calledWith(chat_room.addUsers, user)
                    done()

                it 'should return a promise', (done) ->
                    expect(result).to.eql expected_result
                    done()

                it 'should resolve the promise with the sucessful message', (done) ->
                    sinon.assert.calledWith(deferred.resolve, {success: true, messages: ["user #{user_id} added"]})
                    done()

            describe 'and the user is already in the room', ->

                beforeEach (done) ->
                    chat_room['users'] = [{id: user_id}]
                    result = sut.add_user_to_chat_room(user_id, room_id)
                    done()

                it 'should not create the user on the database', (done) ->
                    sinon.assert.notCalled(chat_room.addUsers)
                    done()

                it 'should return a promise', (done) ->
                    expect(result).to.eql expected_result
                    done()

                it 'should resolve the promise with the not sucessful message', (done) ->
                    sinon.assert.calledWith(deferred.resolve, {success: false, messages: ["user #{user_id} is already in the room"]})
                    done()

        describe 'and the user does not exist', ->

            beforeEach (done) ->
                user = null
                user_promise =
                    then: (callback) ->
                        callback(user)
                users_repository.get_by_id.withArgs(user_id).returns(user_promise)

                result = sut.add_user_to_chat_room(user_id, room_id)
                done()

            it 'should not create the user on the database', (done) ->
                sinon.assert.notCalled(chat_room.addUsers)
                done()

            it 'should return a promise', (done) ->
                expect(result).to.eql expected_result
                done()

            it 'should resolve the promise with the user does not exist message', (done) ->
                sinon.assert.calledWith(deferred.resolve, {success: false, messages: ["user #{user_id} does not exist"]})
                done()





