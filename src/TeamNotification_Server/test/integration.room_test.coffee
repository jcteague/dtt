expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')
Browser = require('zombie').Browser

describe 'Client Room', ->

    browser = null

    beforeEach (done) ->
        browser = new Browser()
        done()

    describe 'When a user visits the client#/room page', ->

        beforeEach (done) ->
            browser.
                visit('http://localhost:3000/client#/room').
                then(done, done)

        it 'should contain an input with a "name" name', (done) ->
            expect(browser.html('input[name="name"]')).to.not.be.empty()
            done()

        it 'should contain a input submit', (done) ->
            expect(browser.html('input[type="submit"]')).to.not.be.empty()
            done()
