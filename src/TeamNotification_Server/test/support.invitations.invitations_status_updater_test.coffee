expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

sut = module_loader.require('../support/invitations/invitations_status_updater', {})

describe 'Invitations Status Updater', ->

    result = user = invitations = invitation1 = invitation2 = invitation3 = invitation4 = null

    beforeEach (done) ->
        user =
            id: 8

        invitation1 = {chat_room_id: 2, accepted: 0, save: sinon.spy()}
        invitation2 = {chat_room_id: 3, accepted: 0, save: sinon.spy()}
        invitation3 = {chat_room_id: 4, accepted: 0, save: sinon.spy()}
        invitation4 = {chat_room_id: 2, accepted: 0, save: sinon.spy()}
        invitations = [invitation1, invitation2, invitation3, invitation4]
        result = sut.update(user, invitations)
        done()

    it 'should update the accepted status of all the invitations', (done) ->
        for invitation in invitations
            expect(invitation.accepted).to.equal 1
        done()

    it 'should save each invitation status in the database', (done) ->
        for invitation in invitations
            sinon.assert.called(invitation.save)
        done()

    it 'should return an array of unique user_id and chat_room_id of each invitation', (done) ->
        expect(result.length).to.equal 3
        expect(result[0]).to.eql invitation1
        expect(result[1]).to.eql invitation2
        expect(result[2]).to.eql invitation3
        done()
