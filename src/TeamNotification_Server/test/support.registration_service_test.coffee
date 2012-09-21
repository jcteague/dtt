expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

Q = require('q')

repository_class_mock = sinon.stub()

user_repository_mock =
    save: sinon.stub()
repository_class_mock.withArgs('User').returns(user_repository_mock)

chat_room_invitation_repository_mock =
    find: sinon.stub()
repository_class_mock.withArgs('ChatRoomInvitation').returns(chat_room_invitation_repository_mock)

chat_room_user_repository_mock =
    save: sinon.stub()
repository_class_mock.withArgs('ChatRoomUser').returns(chat_room_user_repository_mock)

invitation_status_updater_mock =
    update: sinon.stub()

sut = module_loader.require('../support/registration_service', {
    requires:
        './repository': repository_class_mock
        './invitations/invitations_status_updater': invitation_status_updater_mock
})

describe 'Registration Service', ->

    result = user_data = created_user = invitation1 = invitation2 = null
    user_save_spy = null

    make_promise_for = (return_value, spy = null, args_to_verify = null) ->
        spy(args_to_verify) if spy?
        Q.fcall () -> return_value

    describe 'create_user', ->

        beforeEach (done) ->
            user_data =
                first_name: 'foo'
                email: 'foo@bar.com'

            created_user =
                id: 2
                first_name: 'foo'
                email: 'foo@bar.com'

            user_save_spy = sinon.spy()
            user_promise = make_promise_for created_user, user_save_spy, user_data
            user_repository_mock.save.returns(user_promise)
            done()

        afterEach (done) ->
            chat_room_user_repository_mock.save.reset()
            done()

        describe 'and the user has invitations', ->

            invitation_updater = invitations = null

            beforeEach (done) ->
                invitation1 = {chat_room_id: 2, accepted: 0}
                invitation2 = {chat_room_id: 8, accepted: 0}
                invitations = [invitation1, invitation2]
                invitations_promise = make_promise_for invitations
                chat_room_invitation_repository_mock.find.withArgs(email: created_user.email).returns(invitations_promise)

                invitation_status_updater_mock.update.withArgs(created_user, invitations).returns([{user_id: created_user.id, chat_room_id: invitation1.chat_room_id}, {user_id: created_user.id, chat_room_id: invitation2.chat_room_id}])
                result = sut.create_user user_data
                done()

            it 'should save the user in the repository', (done) ->
                sinon.assert.calledWith(user_save_spy, user_data)
                done()

            it 'should update the pending invitations using the updater', (done) ->
                sinon.assert.calledWith(invitation_status_updater_mock.update, created_user, invitations)
                done()

            it 'should add the user to the chat rooms that s/he had been invited', (done) ->
                sinon.assert.calledWith(chat_room_user_repository_mock.save, {user_id: created_user.id, chat_room_id: invitation1.chat_room_id})
                sinon.assert.calledWith(chat_room_user_repository_mock.save, {user_id: created_user.id, chat_room_id: invitation2.chat_room_id})
                done()

            it 'should return a promise with the created user', (done) ->
                Q.when result, (user) ->
                    expect(user).to.eql created_user
                    done()

        describe 'and the user does not have any invitations', ->

            beforeEach (done) ->
                invitations_promise = make_promise_for null
                chat_room_invitation_repository_mock.find.withArgs(email: created_user.email).returns(invitations_promise)

                result = sut.create_user user_data
                done()

            it 'should save the user in the repository', (done) ->
                sinon.assert.calledWith(user_save_spy, user_data)
                done()

            it 'should not attempt to add the user to the chat rooms', (done) ->
                sinon.assert.notCalled(chat_room_user_repository_mock.save)
                done()

            it 'should return a promise with the created user', (done) ->
                Q.when result, (user) ->
                    expect(user).to.eql created_user
                    done()
