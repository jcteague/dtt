expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    when: sinon.spy()

repository_class_mock = sinon.stub()

sut = module_loader.require('../support/strategies/invitations_by_room_id_strategy', {
    requires:
        'q': q_mock
        '../repository': repository_class_mock
})

describe 'InvitationsByRoomId Strategy', ->

    describe 'strategy', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            room_id = 100
            repository =
                find: sinon.stub()
            
            repository_class_mock.withArgs('ChatRoomInvitation').returns(repository)
            callback =
                'then': () -> 
                    return expected_result
                    

            invitations = 'some invitations'           
            repository.find.withArgs({chat_room_id:room_id, accepted:0}).returns(callback)
            
            expected_result = { "room_id":room_id, "result": invitations}

            result = sut(room_id)
            done()

        it 'should return the invitations repository result for the room id', (done) ->
            expect(JSON.stringify(result)).to.equal JSON.stringify(expected_result)
            done()
