expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserRoomsCollection = module_loader.require('../support/collections/user_rooms_collection', {})

describe 'User Rooms Collection', ->

    sut = chat_rooms = user_id = null

    beforeEach (done) ->
        user_id = 1
        chat_rooms = [ { id:1, name:'happy place', owner_id:user_id },{id:2, name:'somewhere', owner_id:user_id}]
        sut = new UserRoomsCollection user_id: user_id, rooms: chat_rooms
        done()

    describe 'constructor', ->

        it 'should have the current rooms value set inside the object', (done) ->
            expect(sut.rooms).to.eql user_id: user_id, rooms: chat_rooms
            done()

    describe 'to_json', ->

        result  = null

        describe 'and the user has rooms', ->

            beforeEach (done) ->
                sut.rooms = user_id: user_id, rooms: chat_rooms
                result = sut.to_json()
                done()
            
            it 'should return the user rooms collection as json', (done) ->
                links = result['links']
                expect(links[0]).to.eql {"name":"self", "rel": "self", "href": "/user/#{user_id}/rooms"}
                expect(links[2]).to.eql {"name":"#{chat_rooms[0].name}",  "rel": chat_rooms[0].name, "href": "/room/#{chat_rooms[0].id}"}
                expect(links[3]).to.eql {"name":"#{chat_rooms[1].name}",  "rel": chat_rooms[1].name, "href": "/room/#{chat_rooms[1].id}"}

                done()

        describe 'and the user does not have rooms', ->

            beforeEach (done) ->
                sut.rooms = user_id: user_id, rooms: []
                result = sut.to_json()
                done()

            it 'should return a link to the root path', (done) ->
                links = result['links']
                expect(links[0]).to.eql {"name":"self", "rel": "self", "href": "/user/#{user_id}/rooms"}
                expect(links[1]).to.eql {"name":"User", "rel": "User", "href": "/user/#{user_id}"}
                expect(links.length).to.equal 2
                done()
