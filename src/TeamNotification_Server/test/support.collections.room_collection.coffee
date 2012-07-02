expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    when: sinon.stub()
underscore_mock =
    bindAll: sinon.stub()

repository_class_mock = sinon.stub()
RoomCollection = module_loader.require('../support/collections/room_collection', {
    requires:
        'q': q_mock
        'underscore': underscore_mock
        '../repository': repository_class_mock
})

describe 'Room Collection', ->

    sut = null
    room = null
    beforeEach (done) ->
        room = { id: 10, name: 'blah room', users: [{id: 1, name: 'foo'}, {id: 2, name: 'bar'}] }
        sut = new RoomCollection(room)
        done()

    describe 'constructor', ->

        it 'should have the current room id value set inside the object', (done) ->
            expect(sut.room).to.equal(room)
            done()

    describe 'to_json', ->

        result = null
        room_id = null

        beforeEach (done) ->
            sut.room = room
            result = sut.to_json()
            done()
        
        it 'should return the manage members link along with the self link', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"name":"self", "rel": "Room", "href": "/room/#{room.id}"}
            expect(links[1]).to.eql {"name": "Manage Members", "rel": "RoomMembers", "href": "/room/#{room.id}/users"}
            expect(links[2]).to.eql {"name": "Room Messages", "rel": "RoomMessages", "href": "/room/#{room.id}/messages"}
            done()

        it 'should return all the room members in the members field', (done) ->
            members = result['members']
            users = room.users
            expect(members[0]).to.eql {"href": "/room/#{room.id}/users", "data": [{"name": users[0].name, "rel": "User", "href": "/user/#{users[0].id}"}, {"name": users[1].name, "rel": "User", "href": "/user/#{users[1].id}"}]}
            done()
