expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomCollection = module_loader.require('../support/collections/room_collection', {})

describe 'Room Collection', ->

    sut = room = room_id = user_id = null

    beforeEach (done) ->
        user_id = 99
        room_id = 10
        room = user_id: user_id, room: { id: room_id, owner_id: 9, name: 'blah room', users: [{id: 1, first_name: 'foo'}, {id: 2, first_name: 'bar'}] }
        sut = new RoomCollection(room)
        done()

    describe 'constructor', ->

        it 'should have the current room id value set inside the object', (done) ->
            expect(sut.room).to.equal(room)
            done()

    describe 'to_json', ->

        result = null

        describe 'and the user is the owner of the room', ->

            beforeEach (done) ->
                sut.room.room.owner_id = user_id
                result = sut.to_json()
                done()

            it 'should return an href property pointing to the current url', (done) ->
                expect(result['href']).to.eql "/room/#{room_id}/users"#{"name":"self", "rel": "Room", "href": "/room/#{room_id}"}
                done()

            it 'should return the manage members link along with the self link', (done) ->
                links = result['links']
                expect(links[0]).to.eql {"name":"self", "rel": "Room", "href": "/room/#{room_id}"}
                expect(links[1]).to.eql {"name": "Pending Invitations", "rel": "Invitations", "href": "/room/#{room_id}/invitations"}
                expect(links[2]).to.eql {"name": "Manage Members", "rel": "RoomMembers", "href": "/room/#{room_id}/users"}
                expect(links[3]).to.eql {"name": "Associate Repository", "rel": "Repository", "href": "/github/oauth", "external": true}
                expect(links[4]).to.eql {"name": "Room Messages", "rel": "RoomMessages", "href": "/room/#{room_id}/messages"}
                done()

            #it 'should return all the room members in the members field', (done) ->
             #   members = result['members']
             #   users = room.room.users
             #   expect(members[0]).to.eql {"href": "/room/#{room_id}/users", "data": [{"name": users[0].first_name, "rel": "User", "href": "/user/#{users[0].id}"}, {"name": users[1].first_name, "rel": "User", "href": "/user/#{users[1].id}"}]}
              #  done()

        describe 'and the user is not the owner of the room', ->

            beforeEach (done) ->
                sut.room.room.owner_id = 19092
                result = sut.to_json()
                done()

            it 'should return an href property pointing to the current url', (done) ->
                expect(result['href']).to.eql "/room/#{room_id}/users" #{"name":"self", "rel": "Room", "href": "/room/#{room_id}"}
                done()

            it 'should return the messages link along with the self link', (done) ->
                links = result['links']
                expect(links[0]).to.eql {"name":"self", "rel": "Room", "href": "/room/#{room_id}"}
                expect(links[1]).to.eql {"name": "Room Messages", "rel": "RoomMessages", "href": "/room/#{room_id}/messages"}
                done()

            #it 'should return all the room members in the members field', (done) ->
            #    members = result['members']
            #    users = room.room.users
             #   expect(members[0]).to.eql {"href": "/room/#{room_id}/users", "data": [{"name": users[0].first_name, "rel": "User", "href": "/user/#{users[0].id}"}, {"name": users[1].first_name, "rel": "User", "href": "/user/#{users[1].id}"}]}
              #  done()
