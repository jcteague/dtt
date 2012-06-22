expect = require('expect.js')
sinon = require('sinon')

requirejs = require('requirejs')
config = require('./client.config')

requirejs.config config
return

requirejs ['client_view'], (ClientView) ->

    describe 'Client View', ->

        sut = null

        beforeEach (done) ->
            sut = new ClientView()
            done()

        describe 'render', ->

            result = null

            beforeEach (done) ->
                sinon.stub(sut.$el, 'empty')
                result = sut.render()
                done()

            it 'should empty the view', (done) ->
                sinon.assert.called(sut.$el.empty)
                done()
