expect = require('expect.js')
sinon = require('sinon')
return

requirejs = require('requirejs')
config = require('./client.config')

requirejs.config config

requirejs ['form_template_renderer'], (FormTemplateRenderer) ->

    describe 'Form Template Renderer', ->

        sut = null

        beforeEach (done) ->
            sut = new FormTemplateRenderer()
            done()

        describe 'get_hello', ->

            result = null

            beforeEach (done) ->
                result = sut.get_hello()
                done()

            it 'should return the blah hello', (done) ->
                #expect(result).to.equal 'blah hello'
                done()
