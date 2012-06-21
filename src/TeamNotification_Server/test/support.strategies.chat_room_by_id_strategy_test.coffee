expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    when: sinon.spy()

repository_class_mock = sinon.stub()
sut = module_loader.require('../support/strategies/chat_room_by_id_strategy', {
    requires:
        'q': q_mock
        '../repository': repository_class_mock
})

describe 'Room By Id Strategy', ->

    describe 'strategy', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            room_id = 100
            repository =
                get_by_id: sinon.stub()
            repository_class_mock.withArgs('ChatRoom').returns(repository)

            room = 'blah room'
            repository.get_by_id.withArgs(room_id).returns(room)

            expected_result = room
            result = sut(room_id)
            done()

        it 'should return the room repository result for the room id', (done) ->
            expect(result).to.equal expected_result
            done()

