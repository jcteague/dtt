expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserRoomsCollection = module_loader.require('../support/collections/user_rooms_collection', {})

describe 'User Rooms Collection', ->

    chat_rooms = null
    sut = null

    beforeEach (done) ->
        chat_rooms = [ { id:1, name:'happy place', owner_id:1 },{id:2, name:'somewhere', owner_id:1 }]
        sut = new UserRoomsCollection chat_rooms
        done()

    describe 'constructor', ->

        it 'should have the current rooms value set inside the object', (done) ->
            expect(sut.rooms).to.equal(chat_rooms)
            done()

    describe 'to_json', ->

        result  = null

        beforeEach (done) ->
            sut.rooms = chat_rooms
            result = sut.to_json()
            done()
        
        it 'should return the user rooms collection as json', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"name":"self", "rel": "self", "href": "/user/#{chat_rooms[0].owner_id}/rooms"}
            expect(links[1]).to.eql {"name":"#{chat_rooms[0].name}",  "rel": chat_rooms[0].name, "href": "/room/#{chat_rooms[0].id}"}
            expect(links[2]).to.eql {"name":"#{chat_rooms[1].name}",  "rel": chat_rooms[1].name, "href": "/room/#{chat_rooms[1].id}"}

            done()
