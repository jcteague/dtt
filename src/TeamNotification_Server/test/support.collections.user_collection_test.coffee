expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserCollection = module_loader.require('../support/collections/user_collection', {})

describe 'User Collection', ->

    sut = null

    beforeEach (done) ->
        options = {}
        sut = new UserCollection(options)
        sinon.spy(sut, 'set_collection')
        done()

    describe 'constructor', ->

        user_id = null

        beforeEach (done) ->
            user_id = 1
            sut.constructor(user_id)
            done()

        it 'should set the collection with the constructor values', (done) ->
            sinon.assert.calledWith(sut.set_collection, user_id)
            done()

    describe 'set_collection', ->

        user_id = null

        beforeEach (done) ->
            user_id = 10
            sut.set_collection(user_id)
            done()

        it 'should set the correct links for the user model', (done) ->
            links = sut.collection['links']
            expect(links[0]).to.eql {"rel": "self", "name": "self", "href": "/user/#{user_id}"}
            expect(links[1]).to.eql {"rel": "rooms", "name": "rooms", "href": "/user/#{user_id}/rooms"}
            done()

    describe 'fetch_to', ->

        callback = null

        beforeEach (done) ->
            sut.collection = 'blah'
            callback = sinon.spy()
            sut.fetch_to callback
            done()

        it 'should call the callback with the collection values', (done) ->
            sinon.assert.calledWith(callback, sut.collection)
            done()
