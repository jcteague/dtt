expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserCollection = module_loader.require('../support/collections/user_collection', {})

describe 'User Collection', ->

    sut = null

    beforeEach (done) ->
        options = {}
        sut = new UserCollection(options)
        done()

    describe 'fetch_to', ->

        callback = null

        beforeEach (done) ->
            sut.value = 'blah'
            callback = sinon.spy()
            sut.fetch_to callback
            done()

        it 'should call the callback with the collection values', (done) ->
            sinon.assert.calledWith(callback, sut.value)
            done()
