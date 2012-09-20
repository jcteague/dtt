expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserCollection = module_loader.require('../support/collections/user_collection', {})

describe 'User Collection', ->

    sut = data = user_id = rooms = room = null

    beforeEach (done) ->
        user_id = 10
        room =
            id: 1
            name: 'blah room'
        rooms = [room]
        data =
            user_id: user_id
            rooms: rooms
        sut = new UserCollection(data)
        done()

    describe 'constructor', ->

        it 'should set the collection with the constructor values', (done) ->
            expect(sut.data).to.equal data
            done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            sut.data = data
            result = sut.to_json()
            done()

        it 'should set the correct links for the user model', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"rel": "User", "name": "self", "href": "/user/#{user_id}"}
            expect(links[1]).to.eql {"rel": "UserEdit", "name": "edit", "href": "/user/#{user_id}/edit"}
            expect(links[2]).to.eql {"rel": "UserRooms", "name": "rooms", "href": "/user/#{user_id}/rooms"}
            expect(links[3]).to.eql {"rel": "Room", "name": "Create Room", "href": "/room"}
            expect(links[4]).to.eql {"rel":"Invitations", "name": "Sent Invitations", "href": "/user/#{user_id}/invitations" }
            done()

        it 'should return a href property pointing to the current url', (done) ->
            expect(result['href']).to.equal "/user/#{user_id}"
            done()

        it 'should return the rooms property as part of the collection', (done) ->
            first_room = result['rooms'][0]
            links = first_room['links']
            expect(links[0]).to.eql {"rel": "Room", "name": room.name, "href": "/room/#{room.id}"}
            data = first_room['data']
            expect(data[0]).to.eql {"name": "id", "value": room.id}
            expect(data[1]).to.eql {"name": "name", "value": room.name}
            done()
