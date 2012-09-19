expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

collection_action_resolver_class_mock = sinon.spy()
repository_class_mock = sinon.stub()
q_mock = 
    defer: sinon.stub()
email_sender_mock =
    send: sinon.stub()
email_template_mock =
    for:
        invitation:
            using: sinon.stub()

sut = module_loader.require('../support/routes_service', {
    requires:
        'q': q_mock
        './collection_action_resolver': collection_action_resolver_class_mock
        './repository': repository_class_mock
        './email/email_sender': email_sender_mock
        './email/templates/email_template': email_template_mock
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

        chat_room = user = user_id = user_email = room_id = users_repository = deferred = callback = expected_result = result = null

        beforeEach (done) ->
            user_id = 8
            user_email = 'foo@bar.com'
            room_id = 2

            chat_room_repository =
                get_by_id: sinon.stub()
            users_repository =
                find: sinon.stub()
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
                user = id: user_id
                users = [user, {id: 10}]
                user_promise =
                    then: (callback) ->
                        callback(users)
                users_repository.find.withArgs(email: user_email).returns(user_promise)
                done()

            describe 'and the user is not in the room', ->

                beforeEach (done) ->
                    chat_room['users'] = [{id: 100}]
                    result = sut.add_user_to_chat_room(user_email, room_id)
                    done()

                it 'should create the user on the database', (done) ->
                    sinon.assert.calledWith(chat_room.addUsers, user)
                    done()

                it 'should return a promise', (done) ->
                    expect(result).to.eql expected_result
                    done()

                it 'should resolve the promise with the sucessful message', (done) ->
                    sinon.assert.calledWith(deferred.resolve, {success: true, messages: ["user added"]})
                    done()

            describe 'and the user is not the owner of the room', ->

                beforeEach (done) ->
                    chat_room['users'] = [{id: 100}]
                    chat_room['owner_id'] = user_id
                    result = sut.add_user_to_chat_room(user_email, room_id)
                    done()

                it 'should not create the user on the database', (done) ->
                    sinon.assert.notCalled(chat_room.addUsers)
                    done()

                it 'should return a promise', (done) ->
                    expect(result).to.eql expected_result
                    done()

                it 'should resolve the promise with the not sucessful message', (done) ->
                    sinon.assert.calledWith(deferred.resolve, {success: false, messages: ["user is already in the room"]})
                    done()

            describe 'and the user is already in the room', ->

                beforeEach (done) ->
                    chat_room['users'] = [{id: user_id}]
                    result = sut.add_user_to_chat_room(user_email, room_id)
                    done()

                it 'should not create the user on the database', (done) ->
                    sinon.assert.notCalled(chat_room.addUsers)
                    done()

                it 'should return a promise', (done) ->
                    expect(result).to.eql expected_result
                    done()

                it 'should resolve the promise with the not sucessful message', (done) ->
                    sinon.assert.calledWith(deferred.resolve, {success: false, messages: ["user is already in the room"]})
                    done()

        describe 'and the user does not exist', ->

            inexistent_email = invitation_email_template = null

            beforeEach (done) ->
                inexistent_email = 'inexistent@email.com'
                users = null
                user_promise =
                    then: (callback) ->
                        callback(users)
                users_repository.find.withArgs(email: inexistent_email).returns(user_promise)

                invitation_email_template = 'blah invitation email'
                email_template_mock.for.invitation.using.withArgs(email: inexistent_email, chat_room: chat_room).returns(invitation_email_template)

                result = sut.add_user_to_chat_room(inexistent_email, room_id)
                done()

            it 'should not create the user on the database', (done) ->
                sinon.assert.notCalled(chat_room.addUsers)
                done()

            it 'should return a promise', (done) ->
                expect(result).to.eql expected_result
                done()

            it 'should send an invitation email to the email address passed', (done) ->
                sinon.assert.called(email_sender_mock.send, invitation_email_template)
                done()

            it 'should resolve the promise with the user does not exist message', (done) ->
                sinon.assert.calledWith(deferred.resolve, {success: false, messages: ["An email invitation has been sent to #{inexistent_email}"]})
                done()

    describe 'is_user_in_room', ->

        result = expected_result = deferred = chat_room_repository = users_repository = room_id = user_id = null

        beforeEach (done) ->
            deferred =
                resolve: sinon.spy()
                promise: 'blah promise'
            q_mock.defer.returns(deferred)
            expected_result = deferred.promise

            user_id = 10
            room_id = 1
            chat_room_repository =
                get_by_id: sinon.stub()
            users_repository =
                get_by_id: sinon.stub()

            repository_class_mock.withArgs('ChatRoom').returns(chat_room_repository)
            repository_class_mock.withArgs('User').returns(users_repository)

            user =
                id: user_id
            user_promise =
                then: (callback) ->
                    callback(user)
            users_repository.get_by_id.withArgs(user_id).returns(user_promise)
            done()

        describe 'and the user is in the room', ->

            beforeEach (done) ->
                room =
                    id: room_id
                    users: [{id: user_id}]
                room_promise =
                    then: (callback) ->
                        callback(room)

                chat_room_repository.get_by_id.withArgs(room_id).returns(room_promise)
                result = sut.is_user_in_room user_id, room_id
                done()

            it 'should resolve the promise with true', ->
                sinon.assert.calledWith(deferred.resolve, true)

            it 'should return a promise', (done) ->
                expect(result).to.eql expected_result
                done()

        describe 'and the user is the owner of the room', ->

            beforeEach (done) ->
                room =
                    id: room_id
                    owner_id: user_id
                    users: []
                room_promise =
                    then: (callback) ->
                        callback(room)

                chat_room_repository.get_by_id.withArgs(room_id).returns(room_promise)
                result = sut.is_user_in_room user_id, room_id
                done()

            it 'should resolve the promise with true', ->
                sinon.assert.calledWith(deferred.resolve, true)

            it 'should return a promise', (done) ->
                expect(result).to.eql expected_result
                done()


        describe 'and the user is not in the room', ->

            beforeEach (done) ->
                room =
                    id: room_id
                    users: []
                room_promise =
                    then: (callback) ->
                        callback(room)

                chat_room_repository.get_by_id.withArgs(room_id).returns(room_promise)
                result = sut.is_user_in_room user_id, room_id
                done()

            it 'should resolve the promise with false', ->
                sinon.assert.calledWith(deferred.resolve, false)

            it 'should return a promise', (done) ->
                expect(result).to.eql expected_result
                done()
