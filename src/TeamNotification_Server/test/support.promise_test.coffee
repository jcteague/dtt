expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    when: sinon.stub()
Promise = module_loader.require('../support/promise', {
    requires:
        'q': q_mock
})

describe 'Promise', ->

    class_instance_values = null
    sut = null

    beforeEach (done) ->
        class_constructor = (values) ->
            this.values = values

        q_mock['when'] = (promise_or_value, callback) ->
            callback(promise_or_value)

        promise = 'blah promise'
        class_instance_values = promise
        sut = new Promise(class_constructor, promise)
        done()

    describe 'constructor', ->

        it 'should return a promise for the class instance', (done) ->
            expect(sut.promised_class_instance.values).to.equal class_instance_values
            done()

    describe 'fetch_to', ->

        callback = null

        beforeEach (done) ->
            sut.promised_class_instance.then = sinon.spy()
            callback = 'blah callback'
            sut.fetch_to callback
            done()

        it 'should call the promised class instance promise then with the callback', (done) ->
            sinon.assert.calledWith(sut.promised_class_instance.then, callback)
            done()
