expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

repository_class_mock = sinon.stub()
sut = module_loader.require('../support/strategies/chat_rooms_by_owner_id_or_member_strategy', {
    requires:
        '../repository': repository_class_mock
})

describe 'Chat Rooms By Owner Id Or Member Strategy', ->

    describe 'strategy', ->

        result = expected_result = repository = user_id = null

        beforeEach (done) ->
            user_id = 10
            repository =
                find: sinon.stub()

            repository_class_mock.withArgs('ChatRoom').returns(repository)
            done()

        describe 'and there are rooms', ->

            beforeEach (done) ->
                room = owner_id: 1, users: [{id: 10}, {id: 3}]
                rooms = [room]
                promise =
                    then: (callback) ->
                        callback(rooms)

                repository.find.returns(promise)
                expected_result = user_id: user_id, rooms: rooms
                result = sut(user_id)
                done()

            it 'should return the room repository result for the rooms that have that user as an owner or member', (done) ->
                expect(result).to.eql expected_result
                done()

        describe 'and there are no rooms', ->

            beforeEach (done) ->
                rooms = null
                promise =
                    then: (callback) ->
                        callback(rooms)

                repository.find.returns(promise)
                expected_result = user_id: user_id, rooms: []
                result = sut(user_id)
                done()

            it 'should return the room repository result for the rooms that have that user as an owner or member', (done) ->
                expect(result).to.eql expected_result
                done()

