expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

repository_class_mock = sinon.stub()

user_repository_mock =
    save: sinon.stub()
repository_class_mock.withArgs('User').returns(user_repository_mock)

chat_room_invitation_repository_mock =
    save: sinon.stub()
repository_class_mock.withArgs('ChatRoomInvitation').returns(chat_room_invitation_repository_mock)

chat_room_user_repository_mock =
    save: sinon.stub()
repository_class_mock.withArgs('ChatRoomUser').returns(chat_room_user_repository_mock)

sut = module_loader.require('../support/registration_service', {
    requires:
        './repository': repository_class_mock
})

describe 'Registration Service', ->

    result = req = res = password = null

    describe 'create_user', ->

        beforeEach (done) ->
            result = sut.create_user user_data
            done()

        it 'should save the user in the repository', (done) ->
            done()

        it 'should update the pending invitations status in the database', (done) ->
            done()

        it 'should add the user to the chat rooms that s/he had been invited', (done) ->
            done()

        it 'should return a promise', (done) ->
            done()
