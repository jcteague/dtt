expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomMessagesCollection = module_loader.require('../support/collections/room_messages_collection', {})

describe 'Room Messages Collection', ->

    chat_room = null
    sut = null

    beforeEach (done) ->
        chat_room = { id: 10, name: 'blah room', messages: [{id: 1, message: 'foo message', user_id: 1}, {id: 2, message: 'bar message', user_id: 2}] }
        sut = new RoomMessagesCollection(chat_room)
        done()

