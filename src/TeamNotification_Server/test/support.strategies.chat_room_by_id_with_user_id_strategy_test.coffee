expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

repository_class_mock = sinon.stub()
sut = module_loader.require('../support/strategies/chat_room_by_id_with_user_id_strategy', {
    requires:
        '../repository': repository_class_mock
})

describe 'Chat Room By Id With User Id Strategy', ->

    describe 'strategy', ->

        result = expected_result = null

        beforeEach (done) ->
            user_id = 10
            room_id = 1
            repository =
                get_by_id: sinon.stub()
            repository_class_mock.withArgs('ChatRoom').returns(repository)

            room = 'blah room'
            promise =
                then: (callback) ->
                    callback(room)
            repository.get_by_id.withArgs(room_id).returns(promise)

            expected_result =
                user_id: user_id
                room: room
            result = sut(room_id: room_id, user_id: user_id)
            done()

        it 'should return the room repository result for the rooms that have that user as an owner or member', (done) ->
            expect(result).to.eql expected_result
            done()
