expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

PluginCollection = module_loader.require('../support/collections/plugin_collection', {})

describe 'Plugin Collection', ->

    sut = plugin_collection_data = null

    beforeEach (done) ->
        plugin_collection_data = 
            name: 'blah name'
            version: 2
            file_name: 'blah filename'

        sut = new PluginCollection(plugin_collection_data)
        done()

    describe 'constructor', ->

        it 'should set data with the collection with the constructor values', (done) ->
            expect(sut.data).to.equal plugin_collection_data
            done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            result = sut.to_json()
            done()

        it 'should return a href property pointing to the current url', (done) ->
            expect(result['href']).to.equal "/plugin"
            done()

        it 'should return the plugin property as part of the collection', (done) ->
            expect(result.plugin.data[0]).to.eql {"name": "name", "value": plugin_collection_data.name}
            expect(result.plugin.data[1]).to.eql {"name": "version", "value": plugin_collection_data.version}
            expect(result.plugin.links[0]).to.eql {"rel": "Plugin", "name": plugin_collection_data.name, "href": "/plugin/download/#{plugin_collection_data.file_name}"}
            done()

    describe 'fetch_to', ->

        result = value = null

        beforeEach (done) ->
            value = 'blah to_json value'
            sinon.stub(sut, 'to_json').returns(value)
            callback = (val) -> val

            result = sut.fetch_to callback
            done()

        it 'should resolve the to_json promise with the callback', (done) ->
            result.then (promise_value) ->
                expect(promise_value).to.eql value
                done()
