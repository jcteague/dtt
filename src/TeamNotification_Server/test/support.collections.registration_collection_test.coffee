expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RegistrationCollection = module_loader.require('../support/collections/registration_collection', {})

describe 'Registration Collection', ->

    sut = null

    beforeEach (done) ->
        sut = new RegistrationCollection()
        done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            result = sut.to_json()
            done()

        it 'should return the href property pointing to the current url', (done) ->
            expect(result['href']).to.eql "/registration"
            done()

        it 'should return the list of links', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"name": "self", "rel": "Registration", "href": "/registration"}
            done()

        it 'should return a template with the first name, last name, email, password and confirm password fields', (done) ->
            template_data = result['template']['data']
            expect(template_data[0]).to.eql {'name': 'first_name', 'label': 'First Name', 'type': 'string'}
            expect(template_data[1]).to.eql {'name': 'last_name', 'label': 'Last Name', 'type': 'string'}
            expect(template_data[2]).to.eql {'name': 'email', 'label': 'Email', 'type': 'string'}
            expect(template_data[3]).to.eql {'name': 'password', 'label': 'Password', 'type': 'password'}
            expect(template_data[4]).to.eql {'name': 'confirm_password', 'label': 'Confirm Password', 'type': 'password'}
            done()
