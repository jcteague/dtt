expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock = sinon.spy()
repository_class_mock = sinon.stub()

sut = module_loader.require('../support/strategies/invitations_by_user_id_strategy', {
    requires:
        'q': q_mock
        '../repository': repository_class_mock
})

describe 'Invitations By User Id Strategy', ->

    describe 'strategy', ->

        result = expected_result = repository = user_id = null

        beforeEach (done) ->
            repository =
                find: sinon.stub()
            
            repository_class_mock.withArgs('ChatRoomInvitation').returns(repository)
            done()
            
        describe 'and there are invitations', ->
            
            beforeEach (done) ->
                user_id = 100
                invitations = [{email:'some invitations', chat_room_id:1}]
                promise =
                    then: (callback) ->
                        callback(invitations)
                
                repository.find.withArgs({user_id:user_id, accepted:0}).returns(promise)
                expected_result = "user_id":user_id, "result": invitations

                result = sut(user_id)
                done()

            it 'should return the invitations repository result for the user id', (done) ->
                expect(JSON.stringify(result)).to.equal JSON.stringify(expected_result)
                done()
