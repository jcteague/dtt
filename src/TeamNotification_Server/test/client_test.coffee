should = require 'should'
sinon = require 'sinon'

# TODO: Find the way to use this node_module correctly
module_loader = require 'sandboxed-module'

http_mock =
    get: sinon.stub()

sut = require '../routes/client.coffee'

describe 'Client', ->

    describe 'build_routes', ->

        app = {}

        beforeEach (done) ->
            app =
                get: sinon.spy()

            sut.build_routes(app)
            done()

        it 'should return the right route with the corresponding callback', (done) ->
            sinon.assert.calledWith(app.get, '/client', sut.methods.get_client)
            done()

    describe 'methods', ->

        describe 'get_client', ->

            res = null

            beforeEach (done) ->
                res = render: sinon.spy()
                sut.methods.get_client(null, res)
                done()

            it 'should render the client template', (done) ->
                sinon.assert.calledWith(res.render, 'client.jade')
                done()
