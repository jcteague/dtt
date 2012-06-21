expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

repository_class_mock = sinon.stub()
sut = module_loader.require('../support/strategies/chat_rooms_by_owner_id_strategy', {
    requires:
        '../repository': repository_class_mock
})

describe 'Chat Rooms By Owner Id Strategy', ->

    describe 'strategy', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            owner_id = 100
            repository =
                find: sinon.stub()
            repository_class_mock.withArgs('ChatRoom').returns(repository)

            rooms = ['blah room']
            repository.find.withArgs(owner_id: owner_id).returns(rooms)

            expected_result = rooms
            result = sut(owner_id)
            done()

        it 'should return the room repository result for the room id', (done) ->
            expect(result).to.equal expected_result
            done()

