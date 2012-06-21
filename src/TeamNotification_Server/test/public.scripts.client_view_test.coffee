expect = require('expect.js')
sinon = require('sinon')

requirejs = require('requirejs')

requirejs.config {
    #nodeRequire: require
    baseUrl: __dirname
}

requirejs ['../public/scripts/client_view'], (ClientView) ->
    console.log 'hola', ClientView

return

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
        sut = new client_view()

    describe 'initialize', ->

        beforeEach (done) ->
            done()
