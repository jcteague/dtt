# TODO: Make it so that requirejs doesn't break test
return
expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

DTT = {}
client_view = module_loader.require('../public/scripts/client_view.js', {
    globals:
        'DTT': DTT
})

describe 'Client View', ->

    sut = null

    beforeEach (done) ->
        console.log DTT
        sut = new ClientView()

    describe 'initialize', ->

        beforeEach (done) ->
            done()
