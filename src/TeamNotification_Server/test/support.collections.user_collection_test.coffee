expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserCollection = module_loader.require('../support/collections/user_collection', {})

describe 'User Collection', ->

    sut = null
    user_id = null

    beforeEach (done) ->
        user_id = 10
        sut = new UserCollection(user_id)
        done()

    describe 'constructor', ->

        it 'should set the collection with the constructor values', (done) ->
            expect(sut.user_id).to.equal user_id
            done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            sut.user_id = user_id
            result = sut.to_json()
            done()

        it 'should set the correct links for the user model', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"rel": "self", "name": "self", "href": "/user/#{user_id}"}
            expect(links[1]).to.eql {"rel": "rooms", "name": "rooms", "href": "/user/#{user_id}/rooms"}
            done()
