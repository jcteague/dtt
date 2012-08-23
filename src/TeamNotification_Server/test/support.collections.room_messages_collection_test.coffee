expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RoomMessagesCollection = module_loader.require('../support/collections/room_messages_collection', {})

describe 'Room Messages Collection', ->
        
    sut = room_id = room_messages = messages = body = data = null

    describe 'and there are room messages', ->

        beforeEach (done) ->
            room_id = 1
            body = {message: 'blah!'}
            room_messages = [ 
                JSON.stringify({ "id":10, "body":JSON.stringify(body), "date":'2012-06-29 11:11', "user_id":1, "name":"james", "room_id":room_id })
                JSON.stringify({ "id":11, "body":JSON.stringify(body), "date":'2012-06-29 11:12', "user_id":1, "name":"jhon", "room_id":room_id })
                ]
            user = {id:1, name:'James'}
            rooms = []
            members= []
            data = {room_id:room_id, messages:room_messages, user_id:user.id, name:user.name, chat_rooms:rooms, members:members }
            sut = new RoomMessagesCollection(data)
            done()

        describe 'constructor', ->

            it 'should have the current room_messages value set inside the object', (done) ->
                expect(sut.room_messages).to.eql room_messages
                done()

        describe 'to_json', ->

            result = room = null

            describe 'and the room has messages', ->

                beforeEach (done) ->
                    sut.room_messages = room_messages
                    result = sut.to_json()
                    done()

                it 'should contain a messages property', (done) ->
                    expect(result).to.have.key('messages')
                    done()

                it 'should contain an href property pointing to the current url', (done) ->
                    expect(result['href']).to.equal "/room/#{room_id}/messages"
                    done()

                it 'should return the chat room messages in the data messages field', (done) ->
                    message = result['messages'][0]
                    room_message_parsed = JSON.parse(room_messages[0])
                    
                    expect(message['data']).to.eql [{ 'name':'user_id', 'value': room_message_parsed.user_id}, { 'name':'user', 'value': room_message_parsed.name}, { 'name':'body', 'value': JSON.stringify(body)}, { 'name':'datetime', 'value':room_message_parsed.date }]
                    done()

                it 'should return a links array in the collection links', (done) ->
                    links = result['links']
                    expect(links[0]).to.eql {"name": "self", "rel": "RoomMessages", "href": "/room/#{room_id}/messages"}
                    expect(links[1]).to.eql {"name": "Room", "rel": "Room", "href": "/room/#{room_id}"}
                    done()

                it 'should contain template data for the message', (done) ->
                    template = result['template']['data']
                    expect(template[0]).to.eql {'name': 'message', 'label': 'Send Message', 'type': 'string-big', 'maxlength': 100}
                    done()

    describe 'and there are not any room messages', ->

        beforeEach (done) ->
            room_id = 1
            data.is_empty = true
            data.messages = []
            room_messages = []
            sut = new RoomMessagesCollection(data)
            done()

        describe 'constructor', ->

            it 'should have the current room_messages value set inside the object', (done) ->
                expect(sut.data.messages).to.eql room_messages
                done()

        describe 'to_json', ->

            result = room = null

            beforeEach (done) ->
                #sut.room_messages = is_empty: true, room_id: room_id
                result = sut.to_json()
                done()

            it 'should contain an href property pointing to the current url', (done) ->
                expect(result['href']).to.equal "/room/#{room_id}/messages"
                done()

            it 'should return a links array in the collection links', (done) ->
                links = result['links']
                expect(links[0]).to.eql {"name": "self", "rel": "RoomMessages", "href": "/room/#{room_id}/messages"}
                expect(links[1]).to.eql {"name": "Room", "rel": "Room", "href": "/room/#{room_id}"}
                done()

            it 'should contain an empty messages property', (done) ->
                expect(result).to.have.key('messages')
                expect(result['messages'].length).to.equal 0
                done()

            it 'should contain template data for the message', (done) ->
                template = result['template']['data']
                expect(template[0]).to.eql {'name': 'message', 'label': 'Send Message', 'type': 'string-big', 'maxlength': 100}
                done()
