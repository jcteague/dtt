expect = require('expect.js')
sinon = require('sinon')
{db: db, entities: entities} = require('./helpers/specs_helper')

module_loader = require('sandboxed-module')
Browser = require('zombie').Browser

users =
    name: 'users'
    entities: [
        {
            id: 1
            name: "'etoribio'"
            email: "'etoribio@aol.com'"
        }
    ]

chat_rooms =
    name: 'chat_room'
    entities: [
        {
            id: 1
            name: "'The real chatroom'"
            owner_id: 1
        }
    ]
mock_message = { id: 1, body: "'"+JSON.stringify({message:"The real test"})+"'", date:"'2012-06-29 11:11'", user_id:1, room_id:1} # user: {id: 1, name: 'etoribio', email: 'etoribio@aol.com'}, room:{ id:1, name:'The real chatroom', owner_id:1 } }
messages =
    name: 'chat_room_messages'
    entities: []

for i in [1..50]
    mock_message.id = i
    messages.entities.push(mock_message)


describe 'Room Messages', ->

    browser = null

    describe 'Set Up', ->

        browser = null
        beforeEach (done) ->
            browser = new Browser()
            db.handle db.clear('users', 'chat_room','chat_room_messages'), db.create(entities.users, entities.chat_rooms,entities.chat_room_messages), db.save(users, chat_rooms,messages), done
        
        describe 'When a user visits the client#/room/:id/messages page', ->

            beforeEach (done) ->
                browser.
                    visit('http://localhost:3000/client#/room/1/messages').
                    then(done, done)

            it 'should contain an input with a "name" name', (done) ->
                expect(browser.html('div[id="messages-container"]')).to.not.be.empty()
                done()

          #  it 'should contain a input submit', (done) ->
           #     expect(browser.html('input[type="submit"]')).to.not.be.empty()
           #     done()
