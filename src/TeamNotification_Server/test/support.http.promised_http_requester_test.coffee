expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

https_mock =
    request: sinon.stub()

sut = module_loader.require('../support/http/promised_http_requester', {
    requires:
        'https': https_mock
})

describe 'Promised HTTP Requester', ->

    describe 'request', ->

        result = request = response = data = null

        beforeEach (done) ->
            data = 'data'
            parameters = 'parameters'
            request =
                end: sinon.spy()
            https_mock.request.returns(request)
            response =
                on: sinon.stub()
                setEncoding: sinon.spy()
            https_mock.request.callsArgWith(1, response)
            result = sut.request(data, parameters)
            done()

        it 'should call the request end with the data', (done) ->
            sinon.assert.calledWith(request.end, data)
            done()

        it 'should set the encoding', (done) ->
            sinon.assert.calledWith(response.setEncoding, 'utf8')
            done()

        it 'should return a promise', (done) ->
            expect(result.then).not.to.be undefined
            done()
