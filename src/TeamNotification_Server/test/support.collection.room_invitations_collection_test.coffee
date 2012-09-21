expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomInvitationsCollection = module_loader.require('../support/collections/room_invitations_collection', {})

describe 'User Invitations Members Collection', ->
    sut = null
    curr_date = null
    curr_user = null
    accepted_invitation = null
    to_accept_invitation = null
    room_invitations = null
    beforeEach (done) ->
        curr_date = "2012-09-20 12:12:10.494376"
        curr_user = 
            id: 1
            email:"myemail@aol.com"
            first_name: 'foo'
       
        curr_chat_room = 
            id:1
            name:"My chatroom name"
            owner_id:1
        
        accepted_invitation = 
            id: 42
            email:"someone@example.com"
            date: curr_date
            user_id:curr_user.id 
            chat_room_id: 1 
            accepted:1  
            user: curr_user 
            chat_room: curr_chat_room
        
        to_accept_invitation = 
            id: 42
            email:"someoneelse@example.com"
            date: curr_date
            user_id:curr_user.id
            chat_room_id: 1
            accepted:0
            user: curr_user
            chat_room: curr_chat_room
        
        room_invitations = { room_id:curr_user.id, result: [accepted_invitation, to_accept_invitation] }
        sut = new RoomInvitationsCollection(room_invitations)
        done()
    describe 'constructor', ->

        it 'should have the current invitations set inside the object', (done) ->
            expect(sut.invitations).to.eql room_invitations
            done()

    describe 'to_json', ->
        json_data = null
        beforeEach (done) ->
            json_data = sut.to_json()
            done()
        it 'should have a well formatted href resource', (done) ->
            expect(json_data).to.have.key 'href'
            expect(json_data.href).to.eql "/user/#{room_invitations.user_id}/invitations"
            done()
            
        it 'should have a links resource', (done) ->
            expect(json_data).to.have.key 'links'
            done()
        
        it 'should have a self resource that links to itself', (done) ->
            expect(json_data.links[0]).to.eql  {"name": "self", "rel": "Invitations", "href": "/room/#{curr_user.id}/invitations"}
            done()
        
        it 'should have an invitations resource', (done) ->
            expect(json_data).to.have.key 'invitations'
            done()        
            
        it 'should contain the right structure inside the key invitations', (done) ->
            expect(json_data['invitations'][0].data).to.eql [{name:"email", value:room_invitations.result[0].email}, {name:"accepted", value:room_invitations.result[0].accepted}, {name:"chat_room_name", value:room_invitations.result[0].chat_room.name},{name:"chat_room_id", value:room_invitations.result[0].chat_room_id}, {name:"date", value:room_invitations.result[0].date} ]
            expect(json_data['invitations'][1].data).to.eql [{name:"email", value:room_invitations.result[1].email}, {name:"accepted", value:room_invitations.result[1].accepted}, {name:"chat_room_name", value:room_invitations.result[1].chat_room.name}, {name:"chat_room_id", value:room_invitations.result[1].chat_room_id}, {name:"date", value:room_invitations.result[1].date}  ]
            done()
            
            
            
            
            
            
            
            
            
            
